//
// MimeUtils.cs
//
// Author: Jeffrey Stedfast <jestedfa@microsoft.com>
//
// Copyright (c) 2013-2025 .NET Foundation and Contributors
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
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace MimeKit.Utils {
	/// <summary>
	/// MIME utility methods.
	/// </summary>
	/// <remarks>
	/// Various utility methods that don't belong anywhere else.
	/// </remarks>
	internal static class MimeUtils
	{
		static readonly char[] UnquoteChars = new[] { '\r', '\n', '\t', '\\', '"' };

		/// <summary>
		/// A string comparer that performs a case-insensitive ordinal string comparison.
		/// </summary>
		/// <remarks>
		/// A string comparer that performs a case-insensitive ordinal string comparison.
		/// </remarks>
		public static readonly IEqualityComparer<string> OrdinalIgnoreCase;

		static MimeUtils ()
		{
#if NETFRAMEWORK || NETSTANDARD2_0
			OrdinalIgnoreCase = new OptimizedOrdinalIgnoreCaseComparer ();
#else
			OrdinalIgnoreCase = StringComparer.OrdinalIgnoreCase;
#endif
		}

#if !NET6_0_OR_GREATER
		internal static void GetRandomBytes (byte[] buffer)
		{
#if NETSTANDARD2_1 || NET5_0_OR_GREATER
			RandomNumberGenerator.Fill (buffer);
#else
			using (var random = RandomNumberGenerator.Create ())
				random.GetBytes (buffer);
#endif
		}
#endif

		/// <summary>
		/// Unquote the specified text.
		/// </summary>
		/// <remarks>
		/// Unquotes the specified text, removing any escaped backslashes within.
		/// </remarks>
		/// <returns>The unquoted text.</returns>
		/// <param name="text">The text to unquote.</param>
		/// <param name="convertTabsToSpaces"><see langword="true" /> if tab characters should be converted to a space; otherwise, <see langword="false" />.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="text"/> is <see langword="null"/>.
		/// </exception>
		public static string Unquote (string text, bool convertTabsToSpaces = false)
		{
			if (text is null)
				throw new ArgumentNullException (nameof (text));

			int index = text.IndexOfAny (UnquoteChars);

			if (index == -1)
				return text;

			var builder = new ValueStringBuilder (text.Length);
			bool escaped = false;
			bool quoted = false;

			for (int i = 0; i < text.Length; i++) {
				switch (text[i]) {
				case '\r':
				case '\n':
					escaped = false;
					break;
				case '\t':
					builder.Append (convertTabsToSpaces ? ' ' : '\t');
					escaped = false;
					break;
				case '\\':
					if (escaped)
						builder.Append ('\\');
					escaped = !escaped;
					break;
				case '"':
					if (escaped) {
						builder.Append ('"');
						escaped = false;
					} else {
						quoted = !quoted;
					}
					break;
				default:
					builder.Append (text[i]);
					escaped = false;
					break;
				}
			}

			return builder.ToString ();
		}

		internal static byte[] Unquote (byte[] text, int startIndex, int length, bool convertTabsToSpaces = false)
		{
			using var builder = new ByteArrayBuilder (length);
			bool escaped = false;
			bool quoted = false;

			for (int i = startIndex; i < startIndex + length; i++) {
				switch ((char) text[i]) {
				case '\r':
				case '\n':
					escaped = false;
					break;
				case '\t':
					builder.Append ((byte) (convertTabsToSpaces ? ' ' : '\t'));
					escaped = false;
					break;
				case '\\':
					if (escaped)
						builder.Append ((byte) '\\');
					escaped = !escaped;
					break;
				case '"':
					if (escaped) {
						builder.Append ((byte) '"');
						escaped = false;
					} else {
						quoted = !quoted;
					}
					break;
				default:
					builder.Append (text[i]);
					escaped = false;
					break;
				}
			}

			return builder.ToArray ();
		}
	}
}
