//
// InternetAddressList.cs
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
using System.Collections.Generic;
using System.ComponentModel;

#if ENABLE_SNM
using System.Net.Mail;
#endif

using MimeKit.Utils;

namespace MimeKit {
	/// <summary>
	/// A list of email addresses. 
	/// </summary>
	/// <remarks>
	/// <para>An <see cref="InternetAddressList"/> may contain any number of addresses of any type
	/// defined by the original Internet Message specification.</para>
	/// <para>There are effectively two (2) types of addresses: mailboxes and groups.</para>
	/// <para>Mailbox addresses are what are most commonly known as email addresses and are
	/// represented by the <see cref="MailboxAddress"/> class.</para>
	/// <para>Group addresses are themselves lists of addresses and are represented by the
	/// <see cref="GroupAddress"/> class. While rare, it is still important to handle these
	/// types of addresses. They typically only contain mailbox addresses, but may also
	/// contain other group addresses.</para>
	/// </remarks>
	internal class InternetAddressList
	{
		readonly List<InternetAddress> list;

		/// <summary>
		/// Initialize a new instance of the <see cref="InternetAddressList"/> class.
		/// </summary>
		/// <remarks>
		/// Creates a new <see cref="InternetAddressList"/> containing the supplied addresses.
		/// </remarks>
		/// <param name="addresses">An initial list of addresses.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="addresses"/> is <see langword="null"/>.
		/// </exception>
		public InternetAddressList (IEnumerable<InternetAddress> addresses)
		{
			if (addresses is null)
				throw new ArgumentNullException (nameof (addresses));

			if (addresses is IList<InternetAddress> lst)
				list = new List<InternetAddress> (lst.Count);
			else
				list = new List<InternetAddress> ();

			foreach (var address in addresses) {
				list.Add (address);
			}
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="InternetAddressList"/> class.
		/// </summary>
		/// <remarks>
		/// Creates a new, empty, <see cref="InternetAddressList"/>.
		/// </remarks>
		public InternetAddressList ()
		{
			list = new List<InternetAddress> ();
		}

		internal static bool TryParse (AddressParserFlags flags, ParserOptions options, byte[] text, ref int index, int endIndex, bool isGroup, int groupDepth, out List<InternetAddress> addresses)
		{
			bool throwOnError = (flags & AddressParserFlags.ThrowOnError) != 0;
			var list = new List<InternetAddress> ();

			addresses = null;

			if (!ParseUtils.SkipCommentsAndWhiteSpace (text, ref index, endIndex, throwOnError))
				return false;

			if (index == endIndex) {
				if (throwOnError)
					throw new ParseException ("No addresses found.", index, index);

				return false;
			}

			while (index < endIndex) {
				if (isGroup && text[index] == (byte) ';')
					break;

				if (!InternetAddress.TryParse (flags, options, text, ref index, endIndex, groupDepth, out var address)) {
					if ((flags & AddressParserFlags.Internal) == 0) {
						// Note: If flags contains the ThrowOnError flag, then InternetAddress.TryParse() would have thrown.
						return false;
					}

					// skip this address...
					while (index < endIndex && text[index] != (byte) ',' && (!isGroup || text[index] != (byte) ';'))
						index++;
				} else {
					list.Add (address);
				}

				// Note: we loop here in case there are any extraneous commas
				bool skippedComma = false;

				do {
					if (!ParseUtils.SkipCommentsAndWhiteSpace (text, ref index, endIndex, throwOnError))
						return false;

					if (index >= endIndex)
						break;

					if (isGroup && text[index] == (byte) ';')
						break;

					if (text[index] != (byte) ',') {
						if (skippedComma)
							break;

						if (options.AddressParserComplianceMode == RfcComplianceMode.Strict) {
							if (throwOnError) {
								if (isGroup)
									throw new ParseException ("Expected ',' between addresses or ';' to denote the end of a group of addresses.", index, index);
								else
									throw new ParseException ("Expected ',' between addresses.", index, index);
							}

							return false;
						} else {
							// start of a new address?
							break;
						}
					}

					skippedComma = true;
					index++;
				} while (true);
			}

			addresses = list;

			return true;
		}
	}
}
