//
// DomainList.cs
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
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using MimeKit.Utils;

namespace MimeKit {
	/// <summary>
	/// A domain list.
	/// </summary>
	/// <remarks>
	/// Represents a list of domains, such as those that an email was routed through.
	/// </remarks>
	internal class DomainList : IEnumerable<string>
	{
		readonly static byte[] DomainSentinels = new[] { (byte) ',', (byte) ':' };
		IList<string> domains;


		internal DomainList (IEnumerable<string> domains, bool validate)
		{
			if (domains is null)
				throw new ArgumentNullException (nameof (domains));

			var list = new List<string> (domains);

			if (validate) {
				foreach (var domain in list) {
					if (!IsValidDomain (domain))
						throw new ArgumentException ("The list contains an invalid domain.", nameof (domains));
				}
			}

			this.domains = list;
		}

		static bool IsValidDomain (string? domain)
		{
			if (string.IsNullOrEmpty (domain))
				return false;

			bool literal = domain[0] == '[';
			int endIndex = domain.Length;
			int atomLength = 0;

			if (literal) {
				if (domain[endIndex - 1] != ']')
					return false;

				endIndex--;
			}

			for (int i = literal ? 1 : 0; i < endIndex; i++) {
				char c = domain[i];

				if (!literal && c == '.') {
					if (atomLength == 0)
						return false;

					atomLength = 0;
					continue;
				}

				if (c < 128) {
					if (literal ? !((byte) c).IsDomain () : !((byte) c).IsAtom ())
						return false;
				} else {
					if (char.IsWhiteSpace (c) || char.IsControl (c))
						return false;

					if (char.IsSurrogate (c)) {
						if (!char.IsHighSurrogate (c) || i + 1 >= endIndex || !char.IsLowSurrogate (domain[i + 1]))
							return false;

						i++;
					}
				}

				atomLength++;
			}

			return literal || atomLength > 0;
		}

		static void ValidateDomain (string domain, string parameterName)
		{
			if (domain is null)
				throw new ArgumentNullException (parameterName);

			if (!IsValidDomain (domain))
				throw new ArgumentException ("Invalid domain syntax.", parameterName);
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="DomainList"/> class.
		/// </summary>
		/// <remarks>
		/// Creates a new <see cref="DomainList"/>.
		/// </remarks>
		public DomainList ()
		{
			domains = Array.Empty<string> ();
		}


		#region IList implementation





		#endregion

		#region ICollection implementation








		#endregion

		#region IEnumerable implementation

		/// <summary>
		/// Get an enumerator for the list of domains.
		/// </summary>
		/// <remarks>
		/// Gets an enumerator for the list of domains.
		/// </remarks>
		/// <returns>The enumerator.</returns>
		public IEnumerator<string> GetEnumerator ()
		{
			return domains.GetEnumerator ();
		}

		#endregion

		#region IEnumerable implementation

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return domains.GetEnumerator ();
		}

		#endregion



		internal event EventHandler? Changed;

		void OnChanged ()
		{
			Changed?.Invoke (this, EventArgs.Empty);
		}


		internal static bool TryParse<TInput> (TInput buffer, ref int index, int endIndex, bool throwOnError, [NotNullWhen (true)] out DomainList? route) where TInput : struct, IAddressParserInput
		{
			var domains = new List<string> ();
			int startIndex = index;

			route = null;

			do {
				// skip over the '@'
				index++;

				if (index >= endIndex) {
					if (throwOnError)
						throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Incomplete domain-list at offset: {0}", buffer.GetByteOffset (startIndex)), buffer.GetByteOffset (startIndex), buffer.GetByteOffset (index));

					return false;
				}

				if (!ParseUtils.TryParseDomain (buffer, ref index, endIndex, DomainSentinels, throwOnError, out var domain))
					return false;

				// Note: Encode() and ToString() both skip whitespace-only domains (which can be parsed when they consist
				// of non-ASCII whitespace characters such as U+2008), so skip them here as well to keep things consistent.
				if (!string.IsNullOrWhiteSpace (domain))
					domains.Add (domain);

				// Note: obs-domain-list allows for null domains between commas
				do {
					if (!ParseUtils.SkipCommentsAndWhiteSpace (buffer, ref index, endIndex, throwOnError))
						return false;

					if (index >= endIndex || buffer[index] != (byte) ',')
						break;

					index++;
				} while (true);

				if (!ParseUtils.SkipCommentsAndWhiteSpace (buffer, ref index, endIndex, throwOnError))
					return false;
			} while (index < buffer.Length && buffer[index] == (byte) '@');

			route = new DomainList (domains, false);

			return true;
		}

	}
}
