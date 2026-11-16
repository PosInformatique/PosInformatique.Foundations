//
// InternetAddress.cs
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

using MimeKit.Utils;

namespace MimeKit {
	/// <summary>
	/// An abstract internet address, as specified by rfc0822.
	/// </summary>
	/// <remarks>
	/// <para>An <see cref="InternetAddress"/> can be any type of address defined by the
	/// original Internet Message specification.</para>
	/// <para>There are effectively two (2) types of addresses: mailboxes and groups.</para>
	/// <para>Mailbox addresses are what are most commonly known as email addresses and are
	/// represented by the <see cref="MailboxAddress"/> class.</para>
	/// <para>Group addresses are themselves lists of addresses and are represented by the
	/// <see cref="GroupAddress"/> class. While rare, it is still important to handle these
	/// types of addresses. They typically only contain mailbox addresses, but may also
	/// contain other group addresses.</para>
	/// </remarks>
	internal abstract class InternetAddress
	{
		Encoding encoding;
		string? name;

		/// <summary>
		/// Initialize a new instance of the <see cref="InternetAddress"/> class.
		/// </summary>
		/// <remarks>
		/// Initializes the <see cref="Encoding"/> and <see cref="Name"/> properties of the internet address.
		/// </remarks>
		/// <param name="encoding">The character encoding to be used for encoding the name.</param>
		/// <param name="name">The name of the mailbox or group.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="encoding"/> is <see langword="null"/>.
		/// </exception>
		protected InternetAddress (Encoding encoding, string? name)
		{
			if (encoding is null)
				throw new ArgumentNullException (nameof (encoding));

			this.encoding = encoding;
			this.name = ParseUtils.TrimWhiteSpace (name);
		}




		#region IComparable implementation



		#endregion

		#region IEquatable implementation


		#endregion








		internal event EventHandler? Changed;

		/// <summary>
		/// Raise the internal changed event used by <see cref="MimeMessage"/> to keep headers in sync.
		/// </summary>
		/// <remarks>
		/// This method is called whenever a property of the internet address is changed.
		/// </remarks>
		protected virtual void OnChanged ()
		{
			Changed?.Invoke (this, EventArgs.Empty);
		}


















	}
}
