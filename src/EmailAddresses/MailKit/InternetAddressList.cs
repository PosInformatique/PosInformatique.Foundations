//
// InternetAddressList.cs
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
	/// <example>
	/// <code language="c#" source="Examples\InternetAddressExamples.cs" region="ParseAddressList"/>
	/// </example>
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
				address.Changed += AddressChanged;
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

		// Note: This .ctor gets used by the Parse and TryParse methods and is meant to reduce duplicating an
		// existing List<InternetAddress> for no reason.


		#region IList implementation





		#endregion

		#region ICollection implementation









		#endregion

		#region IEnumerable implementation


		#endregion

		#region IEnumerable implementation


		#endregion

		#region IEquatable implementation


		#endregion

		#region IComparable implementation


		#endregion







		internal event EventHandler<InternetAddressList, EventArgs>? Changed;

		void OnChanged ()
		{
			Changed?.Invoke (this, EventArgs.Empty);
		}

		void AddressChanged (object? sender, EventArgs e)
		{
			OnChanged ();
		}


















	}
}
