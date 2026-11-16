//
// MailboxAddress.cs
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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using MimeKit.Utils;

namespace MimeKit {
	/// <summary>
	/// A mailbox address, as specified by rfc822.
	/// </summary>
	/// <remarks>
	/// Represents a mailbox address (commonly referred to as an email address)
	/// for a single recipient.
	/// </remarks>
	/// <example>
	/// <code language="c#" source="Examples\InternetAddressExamples.cs" region="ParseMailbox"/>
	/// </example>
	internal class MailboxAddress : InternetAddress
	{


		string address;
		int at;


		internal MailboxAddress (Encoding encoding, string? name, IEnumerable<string> route, string address, int at) : base (encoding, name)
		{
			Route = new DomainList (route, false);
			Route.Changed += RouteChanged;

			this.address = address;
			this.at = at;
		}

		internal MailboxAddress (Encoding encoding, string? name, string address, int at) : base (encoding, name)
		{
			Route = new DomainList ();
			Route.Changed += RouteChanged;

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
		/// <para>Represents the actual email address and is in the form of <c>user@domain.com</c>.</para>
		/// <para>The domain is preserved exactly as it was parsed or set: an IDN-encoded domain (e.g. <c>xn--bcher-kva.example</c>)
		/// is not decoded and an internationalized domain (e.g. <c>bücher.example</c>) is not encoded. Use
		/// <see cref="GetAddress(bool)"/> to get the address with the domain in a specific form.</para>
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
		/// Get whether the address is an international address.
		/// </summary>
		/// <remarks>
		/// <para>International addresses are addresses that contain international
		/// characters in either their local-parts or their domains.</para>
		/// <para>For more information, see section 3.2 of
		/// <a href="https://tools.ietf.org/html/rfc6532#section-3.2">rfc6532</a>.</para>
		/// </remarks>
		/// <example>
		/// <code language="c#" source="Examples\InternetAddressExamples.cs" region="InternationalAddresses"/>
		/// </example>
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










		#region IComparable implementation


		#endregion

		#region IEquatable implementation


		#endregion

		void RouteChanged (object? sender, EventArgs e)
		{
			OnChanged ();
		}








		/// <summary>
		/// Try to parse the given text into a new <see cref="MailboxAddress"/> instance.
		/// </summary>
		/// <remarks>
		/// Parses a single <see cref="MailboxAddress"/>. If the address is not a mailbox address or
		/// there is more than a single mailbox address, then parsing will fail.
		/// </remarks>
		/// <example>
		/// <code language="c#" source="Examples\InternetAddressExamples.cs" region="TryParseMailbox"/>
		/// </example>
		/// <returns><see langword="true" /> if the address was successfully parsed; otherwise, <see langword="false" />.</returns>
		/// <param name="options">The parser options to use.</param>
		/// <param name="text">The text.</param>
		/// <param name="mailbox">The parsed mailbox address.</param>
		public static bool TryParse (ParserOptions? options, string? text, [NotNullWhen (true)] out MailboxAddress? mailbox)
		{
			if (!ArgumentValidator.TryValidate (options, text)) {
				mailbox = null;
				return false;
			}

			var parser = AddressParser.Create (options, text);

			if (!parser.TryParseCompleteAddress (AddressParserFlags.AllowMailboxAddress, out var address)) {
				mailbox = null;
				return false;
			}

			mailbox = (MailboxAddress) address;
			return true;
		}










	}
}
