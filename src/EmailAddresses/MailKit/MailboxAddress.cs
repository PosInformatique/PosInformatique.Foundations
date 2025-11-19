//
// MailboxAddress.cs
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
using System.Globalization;
using System.Collections.Generic;

#if ENABLE_SNM
using System.Net.Mail;
#endif

using MimeKit.Utils;
using MimeKit.Encodings;

namespace MimeKit {
	/// <summary>
	/// A mailbox address, as specified by rfc822.
	/// </summary>
	/// <remarks>
	/// Represents a mailbox address (commonly referred to as an email address)
	/// for a single recipient.
	/// </remarks>
	internal class MailboxAddress : InternetAddress
	{
		/// <summary>
		/// Get or set the punycode implementation that should be used for encoding and decoding mailbox addresses.
		/// </summary>
		/// <remarks>
		/// Gets or sets the punycode implementation that should be used for encoding and decoding mailbox addresses.
		/// </remarks>
		/// <value>The punycode implementation.</value>
		public static IPunycode IdnMapping { get; set; }

		string address;
		int at;

		static MailboxAddress ()
		{
			IdnMapping = new Punycode ();
		}

		internal MailboxAddress (Encoding encoding, string name, IEnumerable<string> route, string address, int at) : base (encoding, name)
		{
			Route = new DomainList (route);

			this.address = address;
			this.at = at;
		}

		internal MailboxAddress (Encoding encoding, string name, string address, int at) : base (encoding, name)
		{
			Route = new DomainList ();

			this.address = address;
			this.at = at;
		}

		/// <summary>
		/// Get the mailbox route.
		/// </summary>
		/// <remarks>
		/// A route is convention that is rarely seen in modern email systems, but is supported
		/// for compatibility with email archives.
		/// </remarks>
		/// <value>The mailbox route.</value>
		public DomainList Route {
			get; private set;
		}

		/// <summary>
		/// Get or set the mailbox address.
		/// </summary>
		/// <remarks>
		/// Represents the actual email address and is in the form of <c>user@domain.com</c>.
		/// </remarks>
		/// <value>The mailbox address.</value>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="value"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="ParseException">
		/// <paramref name="value"/> is malformed.
		/// </exception>
		public string Address {
			get { return address; }
		}

		/// <summary>
		/// Get the local-part of the email address.
		/// </summary>
		/// <remarks>
		/// Gets the local-part of the email address, sometimes referred to as the "user" portion of an email address.
		/// </remarks>
		/// <value>The local-part portion of the mailbox address.</value>
		public string LocalPart {
			get {
				return at != -1 ? address.Substring (0, at) : address;
			}
		}

		/// <summary>
		/// Get the domain of the email address.
		/// </summary>
		/// <remarks>
		/// Gets the domain of the email address.
		/// </remarks>
		/// <value>The domain portion of the mailbox address.</value>
		public string Domain {
			get {
				return at != -1 ? address.Substring (at + 1) : string.Empty;
			}
		}

		/// <summary>
		/// Get whether the address is an international address.
		/// </summary>
		/// <remarks>
		/// <para>International addresses are addresses that contain international
		/// characters in either their local-parts or their domains.</para>
		/// <para>For more information, see section 3.2 of
		/// <a href="https://tools.ietf.org/html/rfc6532#section-3.2">rfc6532</a>.</para>
		/// </remarks>
		/// <value><see langword="true" /> if the address is an international address; otherwise, <see langword="false" />.</value>
		public bool IsInternational {
			get {
				if (string.IsNullOrEmpty (address))
					return false;

				if (ParseUtils.IsInternational (address))
					return true;

				foreach (var domain in Route) {
					if (ParseUtils.IsInternational (domain))
						return true;
				}

				return false;
			}
		}

		internal static bool TryParse (ParserOptions options, byte[] text, ref int index, int endIndex, bool throwOnError, out MailboxAddress mailbox)
		{
			var flags = AddressParserFlags.AllowMailboxAddress;

			if (throwOnError)
				flags |= AddressParserFlags.ThrowOnError;

			if (!InternetAddress.TryParse (flags, options, text, ref index, endIndex, 0, out var address)) {
				mailbox = null;
				return false;
			}

			mailbox = (MailboxAddress) address;

			return true;
		}

		/// <summary>
		/// Try to parse the given text into a new <see cref="MailboxAddress"/> instance.
		/// </summary>
		/// <remarks>
		/// Parses a single <see cref="MailboxAddress"/>. If the address is not a mailbox address or
		/// there is more than a single mailbox address, then parsing will fail.
		/// </remarks>
		/// <returns><see langword="true" /> if the address was successfully parsed; otherwise, <see langword="false" />.</returns>
		/// <param name="options">The parser options to use.</param>
		/// <param name="text">The text.</param>
		/// <param name="mailbox">The parsed mailbox address.</param>
		public static bool TryParse (ParserOptions options, string text, out MailboxAddress mailbox)
		{
			if (!ArgumentValidator.TryValidate (options, text)) {
				mailbox = null;
				return false;
			}

			var buffer = Encoding.UTF8.GetBytes (text);
			int endIndex = buffer.Length;
			int index = 0;

			if (!TryParse (options, buffer, ref index, endIndex, false, out mailbox))
				return false;

			if (!ParseUtils.SkipCommentsAndWhiteSpace (buffer, ref index, endIndex, false) || index != endIndex) {
				mailbox = null;
				return false;
			}

			return true;
		}
	}
}
