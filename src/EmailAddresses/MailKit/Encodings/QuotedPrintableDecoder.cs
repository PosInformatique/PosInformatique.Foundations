//
// QuotedPrintableDecoder.cs
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

using MimeKit.Utils;

namespace MimeKit.Encodings {
	/// <summary>
	/// Incrementally decodes content encoded with the quoted-printable encoding.
	/// </summary>
	/// <remarks>
	/// Quoted-Printable is an encoding often used in MIME to textual content outside
	/// the ASCII range in order to ensure that the text remains intact when sent
	/// via 7bit transports such as SMTP.
	/// </remarks>
	internal class QuotedPrintableDecoder : IMimeDecoder
	{
		const int ScalarScanLength = 16;

		enum QpDecoderState : byte {
			PassThrough,
			EqualSign,
			SoftBreak,
			DecodeByte
		}

		readonly bool rfc2047;
		QpDecoderState state;
		byte saved;

		/// <summary>
		/// Initialize a new instance of the <see cref="QuotedPrintableDecoder"/> class.
		/// </summary>
		/// <remarks>
		/// Creates a new quoted-printable decoder.
		/// </remarks>
		/// <param name="rfc2047"><see langword="true" /> if this decoder will be used to decode rfc2047 encoded-word tokens; otherwise, <see langword="false" />.</param>
		public QuotedPrintableDecoder (bool rfc2047)
		{
			this.rfc2047 = rfc2047;
		}






		/// <summary>
		/// Decode the specified input into the output buffer.
		/// </summary>
		/// <remarks>
		/// <para>Decodes the specified input into the output buffer.</para>
		/// <para>The output buffer should be large enough to hold all the
		/// decoded input. For estimating the size needed for the output buffer,
		/// see <see cref="EstimateOutputLength"/>.</para>
		/// </remarks>
		/// <returns>The number of bytes written to the output buffer.</returns>
		/// <param name="input">A pointer to the beginning of the input buffer.</param>
		/// <param name="length">The length of the input buffer.</param>
		/// <param name="output">A pointer to the beginning of the output buffer.</param>
		internal unsafe int Decode (byte* input, int length, byte* output)
		{
			byte* inend = input + length;
			byte* outptr = output;
			byte* inptr = input;
			byte c;

			while (inptr < inend) {
				switch (state) {
				case QpDecoderState.PassThrough:
					if (rfc2047) {
						while (inptr < inend) {
							c = *inptr++;

							if (c == '=') {
								state = QpDecoderState.EqualSign;
								break;
							} else if (c == '_') {
								*outptr++ = (byte) ' ';
							} else {
								*outptr++ = c;
							}
						}
						break;
					}

					// Scan the first few bytes one at a time because heavily-encoded content (e.g. non-Latin text)
					// tends to have very short runs between '=' characters and the fixed overhead of IndexOf()
					// and MemoryCopy() outweighs their benefit for short runs.
					byte* scanEnd = inend - inptr > ScalarScanLength ? inptr + ScalarScanLength : inend;

					while (inptr < scanEnd) {
						c = *inptr++;

						if (c == '=') {
							state = QpDecoderState.EqualSign;
							break;
						}

						*outptr++ = c;
					}

					if (state == QpDecoderState.PassThrough && inptr < inend) {
						int count = (int) (inend - inptr);
						int index = new ReadOnlySpan<byte> (inptr, count).IndexOf ((byte) '=');

						if (index != -1) {
							state = QpDecoderState.EqualSign;
							count = index;
						}

						// Note: Buffer.MemoryCopy() handles overlapping buffers, so decoding in-place still works.
						Buffer.MemoryCopy (inptr, outptr, count, count);
						outptr += count;
						inptr += count;

						if (state == QpDecoderState.EqualSign)
							inptr++;
					}
					break;
				case QpDecoderState.EqualSign:
					c = *inptr++;

					if (c.IsXDigit ()) {
						state = QpDecoderState.DecodeByte;
						saved = c;
					} else if (c == '=') {
						// invalid encoded sequence - pass it through undecoded
						*outptr++ = (byte) '=';
					} else if (c == '\r') {
						state = QpDecoderState.SoftBreak;
					} else if (c == '\n') {
						state = QpDecoderState.PassThrough;
					} else {
						// invalid encoded sequence - pass it through undecoded
						state = QpDecoderState.PassThrough;
						*outptr++ = (byte) '=';
						*outptr++ = c;
					}
					break;
				case QpDecoderState.SoftBreak:
					state = QpDecoderState.PassThrough;
					c = *inptr++;

					if (c != '\n') {
						// invalid encoded sequence - pass it through undecoded
						*outptr++ = (byte) '=';
						*outptr++ = (byte) '\r';
						*outptr++ = c;
					}
					break;
				case QpDecoderState.DecodeByte:
					c = *inptr++;
					if (c.IsXDigit ()) {
						saved = saved.ToXDigit ();
						c = c.ToXDigit ();

						*outptr++ = (byte) ((saved << 4) | c);
					} else {
						// invalid encoded sequence - pass it through undecoded
						*outptr++ = (byte) '=';
						*outptr++ = saved;
						*outptr++ = c;
					}

					state = QpDecoderState.PassThrough;
					break;
				}
			}

			return (int) (outptr - output);
		}



		internal unsafe int Flush (byte* output)
		{
			int count = 0;

			if (state != QpDecoderState.PassThrough) {
				output[count++] = (byte) '=';

				if (state == QpDecoderState.DecodeByte)
					output[count++] = saved;
				else if (state == QpDecoderState.SoftBreak)
					output[count++] = (byte) '\r';
			}

			Reset ();

			return count;
		}

		/// <summary>
		/// Reset the decoder.
		/// </summary>
		/// <remarks>
		/// Resets the state of the decoder.
		/// </remarks>
		public void Reset ()
		{
			state = QpDecoderState.PassThrough;
			saved = 0;
		}

	}
}
