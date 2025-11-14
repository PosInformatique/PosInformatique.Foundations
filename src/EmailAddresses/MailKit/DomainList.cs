//
// DomainList.cs
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
using System.Collections;
using System.Globalization;
using System.Collections.Generic;

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
		readonly static byte[] DomainSentinels = new [] { (byte) ',', (byte) ':' };
		IList<string> domains;

		/// <summary>
		/// Initialize a new instance of the <see cref="DomainList"/> class.
		/// </summary>
		/// <remarks>
		/// Creates a new <see cref="DomainList"/> based on the domains provided.
		/// </remarks>
		/// <param name="domains">A domain list.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="domains"/> is <see langword="null"/>.
		/// </exception>
		public DomainList (IEnumerable<string> domains)
		{
			if (domains is null)
				throw new ArgumentNullException (nameof (domains));

			this.domains = new List<string> (domains);
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

		/// <summary>
		/// Get an enumerator for the list of domains.
		/// </summary>
		/// <remarks>
		/// Gets an enumerator for the list of domains.
		/// </remarks>
		/// <returns>The enumerator.</returns>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return domains.GetEnumerator ();
		}

		#endregion

		/// <summary>
		/// Try to parse a list of domains.
		/// </summary>
		/// <remarks>
		/// Attempts to parse a <see cref="DomainList"/> from the text buffer starting at the
		/// specified index. The index will only be updated if a <see cref="DomainList"/> was
		/// successfully parsed.
		/// </remarks>
		/// <returns><see langword="true" /> if a <see cref="DomainList"/> was successfully parsed;
		/// otherwise, <see langword="false" />.</returns>
		/// <param name="buffer">The buffer to parse.</param>
		/// <param name="index">The index to start parsing.</param>
		/// <param name="endIndex">An index of the end of the input.</param>
		/// <param name="throwOnError">A flag indicating whether an
		/// exception should be thrown on error.</param>
		/// <param name="route">The parsed DomainList.</param>
		internal static bool TryParse (byte[] buffer, ref int index, int endIndex, bool throwOnError, out DomainList route)
		{
			var domains = new List<string> ();
			int startIndex = index;

			route = null;

			do {
				// skip over the '@'
				index++;

				if (index >= endIndex) {
					if (throwOnError)
						throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Incomplete domain-list at offset: {0}", startIndex), startIndex, index);

					return false;
				}

				if (!ParseUtils.TryParseDomain (buffer, ref index, endIndex, DomainSentinels, throwOnError, out var domain))
					return false;

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

			route = new DomainList (domains);

			return true;
		}
	}
}
