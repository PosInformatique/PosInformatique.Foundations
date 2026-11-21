//
// MimeUtils.cs
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
using System.Collections.Generic;

namespace MimeKit.Utils {
	/// <summary>
	/// MIME utility methods.
	/// </summary>
	/// <remarks>
	/// Various utility methods that don't belong anywhere else.
	/// </remarks>
	internal static class MimeUtils
	{
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

		internal static void UnquoteTo (ReadOnlySpan<char> text, bool convertTabsToSpaces, ref ValueStringBuilder output)
		{
			bool escaped = false;
			bool quoted = false;

			for (int i = 0; i < text.Length; i++) {
				switch (text[i]) {
				case '\r':
				case '\n':
					escaped = false;
					break;
				case '\t':
					output.Append (convertTabsToSpaces ? ' ' : '\t');
					escaped = false;
					break;
				case '\\':
					if (escaped)
						output.Append ('\\');
					escaped = !escaped;
					break;
				case '"':
					if (escaped) {
						output.Append ('"');
						escaped = false;
					} else {
						quoted = !quoted;
					}
					break;
				default:
					output.Append (text[i]);
					escaped = false;
					break;
				}
			}
		}


		internal static void UnquoteTo (ReadOnlySpan<byte> text, bool convertTabsToSpaces, ref ByteArrayBuilder output)
		{
			bool escaped = false;
			bool quoted = false;

			for (int i = 0; i < text.Length; i++) {
				switch ((char) text[i]) {
				case '\r':
				case '\n':
					escaped = false;
					break;
				case '\t':
					output.Append ((byte) (convertTabsToSpaces ? ' ' : '\t'));
					escaped = false;
					break;
				case '\\':
					if (escaped)
						output.Append ((byte) '\\');
					escaped = !escaped;
					break;
				case '"':
					if (escaped) {
						output.Append ((byte) '"');
						escaped = false;
					} else {
						quoted = !quoted;
					}
					break;
				default:
					output.Append (text[i]);
					escaped = false;
					break;
				}
			}
		}

	}
}
