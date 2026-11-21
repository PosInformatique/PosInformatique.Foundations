//
// Base64Decoder.cs
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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MimeKit.Encodings {
	/// <summary>
	/// Incrementally decodes content encoded with the base64 encoding.
	/// </summary>
	/// <remarks>
	/// Base64 is an encoding often used in MIME to encode binary content such
	/// as images and other types of multimedia to ensure that the data remains
	/// intact when sent via 7bit transports such as SMTP.
	/// </remarks>
	internal class Base64Decoder : IMimeDecoder
	{
		static ReadOnlySpan<byte> base64_rank => new byte[256] {
			255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,
			255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,
			255,255,255,255,255,255,255,255,255,255,255, 62,255,255,255, 63,
			 52, 53, 54, 55, 56, 57, 58, 59, 60, 61,255,255,255,  0,255,255,
			255,  0,  1,  2,  3,  4,  5,  6,  7,  8,  9, 10, 11, 12, 13, 14,
			 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25,255,255,255,255,255,
			255, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
			 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51,255,255,255,255,255,
			255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,
			255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,
			255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,
			255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,
			255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,
			255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,
			255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,
			255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,
		};

		int previous;
		uint saved;
		byte bytes;

		/// <summary>
		/// Initialize a new instance of the <see cref="Base64Decoder"/> class.
		/// </summary>
		/// <remarks>
		/// Creates a new base64 decoder.
		/// </remarks>
		public Base64Decoder ()
		{
		}
#if NET6_0_OR_GREATER
		[SkipLocalsInit]
#endif
		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		unsafe int Decode (ref byte table, byte* input, byte* inend, byte* output)
		{
			byte* outptr = output;
			byte* inptr = input;

			// decode every quartet into a triplet
			while (inptr < inend) {
				byte c = *inptr++;
				byte rank = Unsafe.Add (ref table, c);

				if (rank != 0xFF) {
					previous = (previous << 8) | c;
					saved = (saved << 6) | rank;
					bytes++;

					if (bytes == 4) {
						if ((previous & 0xFF0000) != ((byte) '=') << 16) {
							*outptr++ = (byte) ((saved >> 16) & 0xFF);
							if ((previous & 0xFF00) != ((byte) '=') << 8) {
								*outptr++ = (byte) ((saved >> 8) & 0xFF);
								if ((previous & 0xFF) != (byte) '=')
									*outptr++ = (byte) (saved & 0xFF);
							}
						}
						saved = 0;
						bytes = 0;
					}
				}
			}

			return (int) (outptr - output);
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
			ref byte table = ref MemoryMarshal.GetReference (base64_rank);

			return Decode (ref table, input, input + length, output);
		}


		internal unsafe int Flush (byte* output)
		{
			int count = 0;

			if (bytes >= 2) {
				int first = (previous >> ((bytes - 1) * 8)) & 0xff;
				int second = (previous >> ((bytes - 2) * 8)) & 0xff;

				if (first != '=' && second != '=') {
					output[count++] = (byte) (saved >> ((bytes - 2) * 6 + 4));

					if (bytes == 3 && (previous & 0xff) != '=')
						output[count++] = (byte) (saved >> 2);
				}
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
			previous = 0;
			saved = 0;
			bytes = 0;
		}

	}
}
