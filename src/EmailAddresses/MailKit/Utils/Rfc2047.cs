//
// Rfc2047.cs
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
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using MimeKit.Encodings;

namespace MimeKit.Utils {
	/// <summary>
	/// Utility methods for encoding and decoding rfc2047 encoded-word tokens.
	/// </summary>
	/// <remarks>
	/// Utility methods for encoding and decoding rfc2047 encoded-word tokens.
	/// </remarks>
	internal static class Rfc2047
	{
		readonly struct Token
		{
			const char SevenBit = '7';
			const char EightBit = '8';

			public readonly int StartIndex;
			public readonly int Length;
			public readonly char Encoding;

#if REDUCE_TOKEN_SIZE
			// Note: .NET codepages range from ~34-65001 which all fit within a ushort, but we also need '-1'
			// to denote "unknown" and 0 to denote an unencoded ascii word.
			readonly ushort codePage;

			public int CodePage { get { return codePage == ushort.MaxValue ? -1 : codePage; } }
#else
			public readonly int CodePage;
#endif

			public bool Is8bit { get { return CodePage == 0 && Encoding == EightBit; } }

			public bool IsEncoded { get { return CodePage != 0; } }

			public Token (string charset, string culture, char encoding, int startIndex, int length)
			{
#if REDUCE_TOKEN_SIZE
				int cp = CharsetUtils.GetCodePage (charset);
				codePage = cp < 0 ? ushort.MaxValue : (ushort) cp;
#else
				CodePage = CharsetUtils.GetCodePage (charset);
#endif
				StartIndex = startIndex;
				Length = length;
				Encoding = encoding;
			}

			public Token (int startIndex, int length, bool is8bit = false)
			{
				Encoding = is8bit ? EightBit : SevenBit;
				StartIndex = startIndex;
				Length = length;
#if REDUCE_TOKEN_SIZE
				codePage = 0;
#else
				CodePage = 0;
#endif
			}
		}

		struct CodePageCount
		{
			public readonly int CodePage;
			public int Count;

			public CodePageCount (int codepage)
			{
				CodePage = codepage;
				Count = 1;
			}
		}

		interface ITokenWriter : IDisposable
		{
			bool IgnoreWhitespaceBetweenEncodedWords { get; }
			void Write (ref ValueStringBuilder output, ref Token token);
			void Flush (ref ValueStringBuilder output);
		}

		class TokenDecoder : ITokenWriter
		{
			readonly ParserOptions options;
			readonly byte[] input, scratch;
			CodePageCount[] codepages;
			QuotedPrintableDecoder qp;
			Base64Decoder base64;
			IMimeDecoder decoder;
			int codepageIndex;
			int scratchLength;
			char encoding;
			int codepage;

			public TokenDecoder (ParserOptions options, byte[] input, byte[] scratch)
			{
				this.options = options;
				this.scratch = scratch;
				this.input = input;

				codepages = ArrayPool<CodePageCount>.Shared.Rent (16);
				base64 = null;
				qp = null;

				decoder = null;
				codepageIndex = 0;
				scratchLength = 0;
				encoding = '\0';
				codepage = 0;
			}

			public bool IgnoreWhitespaceBetweenEncodedWords {
				get { return true; }
			}

			void AddCodePage (int codepage)
			{
				for (int i = 0; i < codepageIndex; i++) {
					if (codepages[i].CodePage == codepage) {
						codepages[i].Count++;
						return;
					}
				}

				if (codepageIndex == codepages.Length) {
					var resized = ArrayPool<CodePageCount>.Shared.Rent (codepages.Length * 2);
					codepages.AsSpan ().CopyTo (resized);
					ArrayPool<CodePageCount>.Shared.Return (codepages);
					codepages = resized;
				}

				codepages[codepageIndex++] = new CodePageCount (codepage);
			}

			public unsafe void Write (ref ValueStringBuilder output, ref Token token)
			{
				if (decoder != null && (token.CodePage != codepage || char.ToUpperInvariant (token.Encoding) != encoding)) {
					// We've reached the end of a series of encoded-word tokens using identical charsets & encodings.
					//
					// In order to work around broken mailers, we need to combine the raw decoded content of runs of
					// identically encoded-word tokens before converting to unicode strings.
					Flush (ref output);
				}

				if (token.IsEncoded) {
					// Save encoded-word state so that we can treat consecutive encoded-word payloads with identical
					// charsets & encodings as one continuous block, thus allowing us to handle cases where a
					// hex-encoded triplet of a quoted-printable encoded payload is split between 2 or more
					// encoded-word tokens.
					encoding = char.ToUpperInvariant (token.Encoding);
					codepage = token.CodePage;
					if (encoding == 'Q')
						decoder = qp ??= new QuotedPrintableDecoder (true);
					else
						decoder = base64 ??= new Base64Decoder ();

					AddCodePage (codepage);

					fixed (byte* inbuf = input, outbuf = scratch) {
						int n = decoder.Decode (inbuf + token.StartIndex, token.Length, outbuf + scratchLength);
						scratchLength += n;
					}
				} else if (token.Is8bit) {
					// *sigh* I hate broken mailers...
					var unicode = CharsetUtils.ConvertToUnicode (options, input, token.StartIndex, token.Length, out int length);
					output.Append (unicode.AsSpan (0, length));
				} else {
					// pure 7bit ascii, a breath of fresh air...
					int endIndex = token.StartIndex + token.Length;
					for (int i = token.StartIndex; i < endIndex; i++)
						output.Append ((char) input[i]);
				}
			}

			public void Flush (ref ValueStringBuilder output)
			{
				// Reset any base64/quoted-printable decoder state
				decoder?.Reset ();
				decoder = null;

				if (scratchLength > 0) {
					// Convert any decoded encoded-word payloads into unicode and append to our 'decoded' buffer.
					var unicode = CharsetUtils.ConvertToUnicode (options, codepage, scratch, 0, scratchLength, out int length);
					output.Append (unicode.AsSpan (0, length));
					scratchLength = 0;
				}
			}

			public int GetMostCommonCodePage ()
			{
				int codepage = Encoding.UTF8.CodePage;
				int max = 0;

				for (int i = 0; i < codepageIndex; i++) {
					if (codepages[i].Count > max) {
						codepage = codepages[i].CodePage;
						max = codepages[i].Count;
					}
				}

				return codepage;
			}

			public void Dispose ()
			{
				ArrayPool<CodePageCount>.Shared.Return (codepages);
			}
		}

		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		static bool IsAscii (byte c)
		{
			return c < 128;
		}

		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		static bool IsAsciiAtom (byte c)
		{
			return c.IsAsciiAtom ();
		}

		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		static bool IsAtom (byte c)
		{
			return c.IsAtom ();
		}

		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		static bool IsBbQq (byte c)
		{
			return c == 'B' || c == 'b' || c == 'Q' || c == 'q';
		}

		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		static bool IsLwsp (byte c)
		{
			return c.IsWhitespace ();
		}

		static unsafe bool TryGetEncodedWordToken (byte* input, byte* word, int length, out Token token)
		{
			token = default;

			if (length < 7)
				return false;

			byte* inend = word + length - 2;
			byte* inptr = word;

			// check if this could even be an encoded-word token
			if (*inptr++ != '=' || *inptr++ != '?' || *inend++ != '?' || *inend++ != '=')
				return false;

			inend -= 2;

			if (*inptr == '?' || *inptr == '*') {
				// this would result in an empty charset
				return false;
			}

			string charset, culture;

			using (var buffer = new ValueStringBuilder (32)) {
				// find the end of the charset name
				while (*inptr != '?' && *inptr != '*') {
					if (!IsAsciiAtom (*inptr))
						return false;

					buffer.Append ((char) *inptr);
					inptr++;
				}

				charset = buffer.ToString ();
			}

			if (*inptr == '*') {
				// we found a language code...
				inptr++;

				using (var buffer = new ValueStringBuilder (32)) {
					// find the end of the language code
					while (*inptr != '?') {
						if (!IsAsciiAtom (*inptr))
							return false;

						buffer.Append ((char) *inptr);
						inptr++;
					}

					culture = buffer.ToString ();
				}
			} else {
				culture = null;
			}

			// skip over the '?' to get to the encoding
			inptr++;

			char encoding;
			if (*inptr == 'B' || *inptr == 'b' || *inptr == 'Q' || *inptr == 'q') {
				encoding = (char) *inptr++;
			} else {
				return false;
			}

			if (*inptr != '?' || inptr == inend)
				return false;

			// skip over the '?' to get to the payload
			inptr++;

			int start = (int) (inptr - input);
			int len = (int) (inend - inptr);

			token = new Token (charset, culture, encoding, start, len);

			return true;
		}

		static unsafe void TokenizePhrase (ParserOptions options, ITokenWriter writer, ref ValueStringBuilder decoded, byte* inbuf, int startIndex, int length)
		{
			byte* text, word, inptr = inbuf + startIndex;
			byte* inend = inptr + length;
			var lwsp = new Token (0, 0);
			bool encoded = false;
			Token token;
			bool ascii;
			int n;

			while (inptr < inend) {
				text = inptr;
				while (inptr < inend && IsLwsp (*inptr))
					inptr++;

				lwsp = new Token ((int) (text - inbuf), (int) (inptr - text));

				word = inptr;
				ascii = true;
				if (inptr < inend && IsAsciiAtom (*inptr)) {
					if (options.Rfc2047ComplianceMode == RfcComplianceMode.Loose) {
						// Make an extra effort to detect and separate encoded-word
						// tokens that have been merged with other words.
						bool is_rfc2047 = false;

						if (inptr + 2 < inend && *inptr == '=' && *(inptr + 1) == '?') {
							inptr += 2;

							// skip past the charset (if one is even declared, sigh)
							while (inptr < inend && *inptr != '?') {
								ascii = ascii && IsAscii (*inptr);
								inptr++;
							}

							// sanity check encoding type
							if (inptr + 3 >= inend || *inptr != '?' || !IsBbQq (*(inptr + 1)) || *(inptr + 2) != '?') {
								ascii = true;
								goto non_rfc2047;
							}

							inptr += 3;

							// find the end of the rfc2047 encoded word token
							while (inptr + 2 < inend && !(*inptr == '?' && *(inptr + 1) == '=')) {
								ascii = ascii && IsAscii (*inptr);
								inptr++;
							}

							if (inptr + 2 > inend || *inptr != '?' || *(inptr + 1) != '=') {
								// didn't find an end marker...
								inptr = word + 2;
								ascii = true;

								goto non_rfc2047;
							}

							is_rfc2047 = true;
							inptr += 2;
						}

					non_rfc2047:
						if (!is_rfc2047) {
							// stop if we encounter a possible rfc2047 encoded
							// token even if it's inside another word, sigh.
							while (inptr < inend && IsAtom (*inptr)) {
								if (inptr + 2 < inend && *inptr == '=' && *(inptr + 1) == '?')
									break;
								ascii = ascii && IsAscii (*inptr);
								inptr++;
							}
						}
					} else {
						// encoded-word tokens are atoms
						while (inptr < inend && IsAsciiAtom (*inptr)) {
							//ascii = ascii && IsAscii (*inptr);
							inptr++;
						}
					}

					n = (int) (inptr - word);
					if (TryGetEncodedWordToken (inbuf, word, n, out token)) {
						// rfc2047 states that you must ignore all whitespace between
						// encoded-word tokens
						if ((!encoded || !writer.IgnoreWhitespaceBetweenEncodedWords) && lwsp.Length > 0) {
							// previous token was not encoded, so preserve whitespace
							writer.Write (ref decoded, ref lwsp);
						}

						writer.Write (ref decoded, ref token);
						encoded = true;
					} else {
						// append the lwsp and atom tokens
						if (lwsp.Length > 0)
							writer.Write (ref decoded, ref lwsp);

						token = new Token ((int) (word - inbuf), n, !ascii);
						writer.Write (ref decoded, ref token);

						encoded = false;
					}
				} else {
					// append the lwsp token
					if (lwsp.Length > 0)
						writer.Write (ref decoded, ref lwsp);

					// append the non-ascii atom token
					ascii = true;
					while (inptr < inend && !IsLwsp (*inptr) && !IsAsciiAtom (*inptr)) {
						ascii = ascii && IsAscii (*inptr);
						inptr++;
					}

					token = new Token ((int) (word - inbuf), (int) (inptr - word), !ascii);
					writer.Write (ref decoded, ref token);

					encoded = false;
				}
			}

			writer.Flush (ref decoded);
		}

		internal static string DecodePhrase (ParserOptions options, byte[] phrase, int startIndex, int count, out int codepage)
		{
			codepage = Encoding.UTF8.CodePage;

			if (count == 0)
				return string.Empty;

			unsafe {
				fixed (byte* inbuf = phrase) {
					var scratch = count < 2048 ? ArrayPool<byte>.Shared.Rent (count) : new byte[count];
					var decoder = new TokenDecoder (options, phrase, scratch);
					var decoded = new ValueStringBuilder (count);

					try {
						TokenizePhrase (options, decoder, ref decoded, inbuf, startIndex, count);
						codepage = decoder.GetMostCommonCodePage ();
						return decoded.ToString ();
					} finally {
						if (count < 2048)
							ArrayPool<byte>.Shared.Return (scratch);
						decoder.Dispose ();
					}
				}
			}
		}

		/// <summary>
		/// Decode a phrase.
		/// </summary>
		/// <remarks>
		/// Decodes the phrase(s) starting at the given index and spanning across
		/// the specified number of bytes using the supplied parser options.
		/// </remarks>
		/// <returns>The decoded phrase.</returns>
		/// <param name="options">The parser options to use.</param>
		/// <param name="phrase">The phrase to decode.</param>
		/// <param name="startIndex">The starting index.</param>
		/// <param name="count">The number of bytes to decode.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <para><paramref name="options"/> is <see langword="null"/>.</para>
		/// <para>-or-</para>
		/// <para><paramref name="phrase"/> is <see langword="null"/>.</para>
		/// </exception>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// <paramref name="startIndex"/> and <paramref name="count"/> do not specify
		/// a valid range in the byte array.
		/// </exception>
		public static string DecodePhrase (ParserOptions options, byte[] phrase, int startIndex, int count)
		{
			if (options is null)
				throw new ArgumentNullException (nameof (options));

			if (phrase is null)
				throw new ArgumentNullException (nameof (phrase));

			if (startIndex < 0 || startIndex > phrase.Length)
				throw new ArgumentOutOfRangeException (nameof (startIndex));

			if (count < 0 || startIndex + count > phrase.Length)
				throw new ArgumentOutOfRangeException (nameof (count));

			return DecodePhrase (options, phrase, startIndex, count, out _);
		}
	}
}
