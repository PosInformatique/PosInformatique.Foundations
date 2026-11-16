//
// ParseUtils.cs
//
// Author: Jeffrey Stedfast <jestedfa@microsoft.com>
//
// Copyright (c) 2013-2026 .NET Foundation and Contributors
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using System;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MimeKit.Utils {
	static class ParseUtils
	{
		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		public static bool IsWhiteSpace (char c)
		{
			return c is ' ' or '\t' or '\r' or '\n';
		}

		[return: NotNullIfNotNull (nameof (text))]
		public static string? TrimWhiteSpace (string? text)
		{
			if (text is null)
				return null;

			int startIndex = 0, endIndex = text.Length;

			while (startIndex < endIndex && IsWhiteSpace (text[startIndex]))
				startIndex++;

			while (endIndex > startIndex && IsWhiteSpace (text[endIndex - 1]))
				endIndex--;

			return startIndex == 0 && endIndex == text.Length ? text : text.Substring (startIndex, endIndex - startIndex);
		}




		public static bool SkipWhiteSpace<TInput> (TInput text, ref int index, int endIndex) where TInput : struct, IAddressParserInput
		{
			int startIndex = index;

			while (index < endIndex && text.IsWhitespace (index))
				index++;

			return index > startIndex;
		}



		public static bool SkipComment<TInput> (TInput text, ref int index, int endIndex) where TInput : struct, IAddressParserInput
		{
			bool escaped = false;
			int depth = 1;

			index++;

			while (index < endIndex && depth > 0) {
				if (text[index] == (byte) '\\') {
					escaped = !escaped;
				} else if (!escaped) {
					if (text[index] == (byte) '(')
						depth++;
					else if (text[index] == (byte) ')')
						depth--;
				} else {
					escaped = false;
				}

				index++;
			}

			return depth == 0;
		}



		public static bool SkipCommentsAndWhiteSpace<TInput> (TInput text, ref int index, int endIndex, bool throwOnError) where TInput : struct, IAddressParserInput
		{
			SkipWhiteSpace (text, ref index, endIndex);

			while (index < endIndex && text[index] == (byte) '(') {
				int startIndex = index;

				if (!SkipComment (text, ref index, endIndex)) {
					if (throwOnError)
						throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Incomplete comment token at offset {0}", text.GetByteOffset (startIndex)), text.GetByteOffset (startIndex), text.GetByteOffset (index));

					return false;
				}

				SkipWhiteSpace (text, ref index, endIndex);
			}

			return true;
		}



		public static bool SkipQuoted<TInput> (TInput text, ref int index, int endIndex, bool throwOnError) where TInput : struct, IAddressParserInput
		{
			int startIndex = index;
			bool escaped = false;

			// skip over leading '"'
			index++;

			while (index < endIndex) {
				if (text[index] == (byte) '\\') {
					escaped = !escaped;
				} else if (!escaped) {
					if (text[index] == (byte) '"')
						break;
				} else {
					escaped = false;
				}

				index++;
			}

			if (index >= endIndex) {
				if (throwOnError)
					throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Incomplete quoted-string token at offset {0}", text.GetByteOffset (startIndex)), text.GetByteOffset (startIndex), text.GetByteOffset (index));

				return false;
			}

			// skip over the closing '"'
			index++;

			return true;
		}


		public static bool SkipAtom<TInput> (TInput text, ref int index, int endIndex) where TInput : struct, IAddressParserInput
		{
			int start = index;

			while (index < endIndex && text.IsAtom (index))
				index++;

			return index > start;
		}

		// Note: a "phrase atom" is a more lenient atom (e.g. mailbox display-name phrase atom)

		public static bool SkipPhraseAtom<TInput> (TInput text, ref int index, int endIndex) where TInput : struct, IAddressParserInput
		{
			int start = index;

			while (index < endIndex && text.IsPhraseAtom (index))
				index++;

			return index > start;
		}



		public static bool SkipWord<TInput> (TInput text, ref int index, int endIndex, bool throwOnError) where TInput : struct, IAddressParserInput
		{
			if (text[index] == (byte) '"')
				return SkipQuoted (text, ref index, endIndex, throwOnError);

			if (text.IsAtom (index))
				return SkipAtom (text, ref index, endIndex);

			return false;
		}

		public static bool IsSentinel (char c, ReadOnlySpan<byte> sentinels)
		{
			for (int i = 0; i < sentinels.Length; i++) {
				if (c == sentinels[i])
					return true;
			}

			return false;
		}

		static bool TryParseDotAtom<TInput> (TInput text, ref int index, int endIndex, ReadOnlySpan<byte> sentinels, bool throwOnError, string tokenType, [NotNullWhen (true)] out string? dotatom) where TInput : struct, IAddressParserInput
		{
			using var token = new ValueStringBuilder (128);
			int startIndex = index;
			int comment;

			dotatom = null;

			do {
				if (!text.IsAtom (index)) {
					if (throwOnError)
						throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Invalid {0} token at offset {1}", tokenType, text.GetByteOffset (startIndex)), text.GetByteOffset (startIndex), text.GetByteOffset (index));

					return false;
				}

				int start = index;
				while (index < endIndex && text.IsAtom (index))
					index++;

				try {
					token.Append (text.GetUtf8String (start, index - start));
				} catch (DecoderFallbackException ex) {
					if (throwOnError)
						throw new ParseException ("Internationalized domains may only contain UTF-8 characters.", text.GetByteOffset (start), text.GetByteOffset (start), ex);

					return false;
				}

				comment = index;
				if (!SkipCommentsAndWhiteSpace (text, ref index, endIndex, throwOnError))
					return false;

				if (index >= endIndex || text[index] != (byte) '.') {
					index = comment;
					break;
				}

				// skip over the '.'
				index++;

				if (!SkipCommentsAndWhiteSpace (text, ref index, endIndex, throwOnError))
					return false;

				// allow domains to end with a '.', but strip it off
				if (index >= endIndex || IsSentinel (text[index], sentinels))
					break;

				token.Append ('.');
			} while (true);

			dotatom = token.ToString ();

			return true;
		}

		static bool TryParseDomainLiteral<TInput> (TInput text, ref int index, int endIndex, bool throwOnError, [NotNullWhen (true)] out string? domain) where TInput : struct, IAddressParserInput
		{
			using var token = new ValueStringBuilder (128);
			int startIndex = index++;

			domain = null;

			token.Append ('[');

			SkipWhiteSpace (text, ref index, endIndex);

			do {
				while (index < endIndex && text.IsDomain (index)) {
					token.Append (text[index]);
					index++;
				}

				SkipWhiteSpace (text, ref index, endIndex);

				if (index >= endIndex) {
					if (throwOnError)
						throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Incomplete domain literal token at offset {0}", text.GetByteOffset (startIndex)), text.GetByteOffset (startIndex), text.GetByteOffset (index));

					return false;
				}

				if (text[index] == (byte) ']')
					break;

				if (!text.IsDomain (index)) {
					if (throwOnError)
						throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Invalid domain literal token at offset {0}", text.GetByteOffset (startIndex)), text.GetByteOffset (startIndex), text.GetByteOffset (index));

					return false;
				}
			} while (true);

			token.Append (']');
			index++;

			domain = token.ToString ();

			return true;
		}


		public static bool TryParseDomain<TInput> (TInput text, ref int index, int endIndex, ReadOnlySpan<byte> sentinels, bool throwOnError, [NotNullWhen (true)] out string? domain) where TInput : struct, IAddressParserInput
		{
			if (text[index] == (byte) '[')
				return TryParseDomainLiteral (text, ref index, endIndex, throwOnError, out domain);

			return TryParseDotAtom (text, ref index, endIndex, sentinels, throwOnError, "domain", out domain);
		}

		public static bool IsInternational (string value, int startIndex, int count)
		{
			int endIndex = startIndex + count;

			for (int i = startIndex; i < endIndex; i++) {
				if (value[i] > 127)
					return true;
			}

			return false;
		}

		public static bool IsInternational (string value, int startIndex)
		{
			return IsInternational (value, startIndex, value.Length - startIndex);
		}

		public static bool IsInternational (string value)
		{
			return IsInternational (value, 0, value.Length);
		}


    }
}
