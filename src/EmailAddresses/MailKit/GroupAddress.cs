//
// GroupAddress.cs
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
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections.Generic;

using MimeKit.Utils;

namespace MimeKit {
	/// <summary>
	/// An address group, as specified by rfc0822.
	/// </summary>
	/// <remarks>
	/// Group addresses are rarely used anymore. Typically, if you see a group address,
	/// it will be of the form: <c>"undisclosed-recipients: ;"</c>.
	/// </remarks>
	internal class GroupAddress : InternetAddress
	{
		/// <summary>
		/// Initialize a new instance of the <see cref="GroupAddress"/> class.
		/// </summary>
		/// <remarks>
		/// Creates a new <see cref="GroupAddress"/> with the specified name and list of addresses. The
		/// specified text encoding is used when encoding the name according to the rules of rfc2047.
		/// </remarks>
		/// <param name="encoding">The character encoding to be used for encoding the name.</param>
		/// <param name="name">The name of the group.</param>
		/// <param name="addresses">A list of addresses.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="encoding"/> is <see langword="null"/>.
		/// </exception>
		public GroupAddress (Encoding encoding, string name, IEnumerable<InternetAddress> addresses) : base (encoding, name)
		{
			Members = new InternetAddressList (addresses);
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="GroupAddress"/> class.
		/// </summary>
		/// <remarks>
		/// Creates a new <see cref="GroupAddress"/> with the specified name. The specified
		/// text encoding is used when encoding the name according to the rules of rfc2047.
		/// </remarks>
		/// <param name="encoding">The character encoding to be used for encoding the name.</param>
		/// <param name="name">The name of the group.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="encoding"/> is <see langword="null"/>.
		/// </exception>
		public GroupAddress (Encoding encoding, string name) : base (encoding, name)
		{
			Members = new InternetAddressList ();
		}

		/// <summary>
		/// Get the members of the group.
		/// </summary>
		/// <remarks>
		/// <para>Represents the member addresses of the group. If the group address properly conforms
		/// to the internet standards, every group member should be of the <see cref="MailboxAddress"/>
		/// variety. When handling group addresses constructed by third-party software, it is possible
		/// for groups to contain members of the <see cref="GroupAddress"/> variety.</para>
		/// <para>When constructing new messages, it is recommended that address groups not contain
		/// anything other than <see cref="MailboxAddress"/> members in order to comply with internet
		/// standards.</para>
		/// </remarks>
		/// <value>The list of members.</value>
		public InternetAddressList Members {
			get; private set;
		}
	}
}
