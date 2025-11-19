//
// InternetAddress.cs
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
using System.ComponentModel;

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
		const string AtomSpecials = "()<>@,;:\\\".[]";
		Encoding encoding;
		string name;

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
		protected InternetAddress (Encoding encoding, string name)
		{
			if (encoding is null)
				throw new ArgumentNullException (nameof (encoding));

			Encoding = encoding;
			Name = name;
		}

		/// <summary>
		/// Get or set the character encoding to use when encoding the name of the address.
		/// </summary>
		/// <remarks>
		/// The character encoding is used to convert the <see cref="Name"/> property, if it is set,
		/// to a stream of bytes when encoding the internet address for transport.
		/// </remarks>
		/// <value>The character encoding.</value>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="value"/> is <see langword="null"/>.
		/// </exception>
		public Encoding Encoding {
			get { return encoding; }
			set {
				if (value is null)
					throw new ArgumentNullException (nameof (value));

				if (value == encoding)
					return;

				encoding = value;
			}
		}

		/// <summary>
		/// Get or set the display name of the address.
		/// </summary>
		/// <remarks>
		/// <para>A name is optional and is typically set to the name of the person
		/// or group that own the internet address.</para>
		/// <para>For example, the <see cref="Name"/> property of the following <see cref="MailboxAddress"/> would be <c>"John Smith"</c>.</para>
		/// <para><c>John Smith &lt;j.smith@example.com&gt;</c></para>
		/// <para>Likewise, the <see cref="Name"/> property of the following <see cref="GroupAddress"/> would be <c>"undisclosed-recipients"</c>.</para>
		/// <para><c>undisclosed-recipients: Alice &lt;alice@wonderland.com&gt;, Bob &lt;bob@the-builder.com&gt;;</c></para>
		/// </remarks>
		/// <value>The name of the address.</value>
		public string Name {
			get { return name; }
			set {
				if (value == name)
					return;

				name = value;
			}
		}

		internal static bool TryParseLocalPart (byte[] text, ref int index, int endIndex, RfcComplianceMode compliance, bool skipTrailingCfws, bool throwOnError, out string localpart)
		{
			using var token = new ValueStringBuilder (128);
			int startIndex = index;

			localpart = null;

			do {
				bool escapedAt = false;
				int start = index;

				if (text[index] == (byte) '"') {
					if (!ParseUtils.SkipQuoted (text, ref index, endIndex, throwOnError))
						return false;
				} else if (text[index].IsAtom ()) {
					if (!ParseUtils.SkipAtom (text, ref index, endIndex))
						return false;

					if (compliance == RfcComplianceMode.Looser) {
						// Allow local-parts that include escaped '@' symbols.
						// See https://github.com/jstedfast/MimeKit/issues/1043 for details.
						while (index + 1 < endIndex && text[index] == (byte) '\\' && text[index + 1] == (byte) '@') {
							// track that we've encountered an escaped @ symbol
							escapedAt = true;

							// skip over the '\\' and '@' characters
							index += 2;

							if (!ParseUtils.SkipAtom (text, ref index, endIndex))
								break;
						}
					}
				} else {
					if (throwOnError)
						throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Invalid local-part at offset {0}", startIndex), startIndex, index);

					return false;
				}

				string word;

				try {
					word = CharsetUtils.UTF8.GetString (text, start, index - start);
				} catch (DecoderFallbackException ex) {
					if (compliance == RfcComplianceMode.Strict) {
						if (throwOnError)
							throw new ParseException ("Internationalized local-part tokens may only contain UTF-8 characters.", start, start, ex);

						return false;
					}

					word = CharsetUtils.Latin1.GetString (text, start, index - start);
				}

				if (escapedAt)
					word = word.Replace ("\\@", "%40");

				token.Append (word);

				int cfws = index;
				if (!ParseUtils.SkipCommentsAndWhiteSpace (text, ref index, endIndex, throwOnError))
					return false;

				if (index >= endIndex || text[index] != (byte) '.') {
					if (!skipTrailingCfws)
						index = cfws;
					break;
				}

				do {
					token.Append ('.');
					index++;

					if (!ParseUtils.SkipCommentsAndWhiteSpace (text, ref index, endIndex, throwOnError))
						return false;

					if (index >= endIndex) {
						if (throwOnError)
							throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Incomplete local-part at offset {0}", startIndex), startIndex, index);

						return false;
					}
				} while (compliance == RfcComplianceMode.Looser && text[index] == (byte) '.');

				if (compliance == RfcComplianceMode.Looser && (index >= endIndex || text[index] == (byte) '@'))
					break;
			} while (true);

			localpart = token.ToString ();

			return true;
		}

		static ReadOnlySpan<byte> CommaGreaterThanOrSemiColon => ",>;"u8;

		internal static bool TryParseAddrspec (byte[] text, ref int index, int endIndex, ReadOnlySpan<byte> sentinels, RfcComplianceMode compliance, bool throwOnError, out string addrspec, out int at)
		{
			int startIndex = index;

			addrspec = null;
			at = -1;

			if (!TryParseLocalPart (text, ref index, endIndex, compliance, true, throwOnError, out var localpart))
				return false;

			if (index >= endIndex || ParseUtils.IsSentinel (text[index], sentinels)) {
				addrspec = localpart;
				return true;
			}

			if (text[index] != (byte) '@') {
				if (throwOnError)
					throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Invalid addr-spec token at offset {0}", startIndex), startIndex, index);

				return false;
			}

			index++;
			if (index >= endIndex) {
				if (throwOnError)
					throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Incomplete addr-spec token at offset {0}", startIndex), startIndex, index);

				return false;
			}

			if (!ParseUtils.SkipCommentsAndWhiteSpace (text, ref index, endIndex, throwOnError))
				return false;

			if (index >= endIndex) {
				if (throwOnError)
					throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Incomplete addr-spec token at offset {0}", startIndex), startIndex, index);

				return false;
			}

			if (!ParseUtils.TryParseDomain (text, ref index, endIndex, sentinels, throwOnError, out var domain))
				return false;

			if (ParseUtils.IsIdnEncoded (domain))
				domain = MailboxAddress.IdnMapping.Decode (domain);

			addrspec = localpart + "@" + domain;
			at = localpart.Length;

			return true;
		}

		internal static bool TryParseMailbox (ParserOptions options, byte[] text, int startIndex, ref int index, int endIndex, string name, int codepage, bool throwOnError, out InternetAddress address)
		{
			var encoding = CharsetUtils.GetEncodingOrDefault (codepage, Encoding.UTF8);
			DomainList route = null;

			address = null;

			// skip over the '<'
			index++;

			// Note: check for excessive angle brackets like the example described in section 7.1.2 of rfc7103...
			if (index < endIndex && text[index] == (byte) '<') {
				if (options.AddressParserComplianceMode == RfcComplianceMode.Strict) {
					if (throwOnError)
						throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Excessive angle brackets at offset {0}", index), startIndex, index);

					return false;
				}

				do {
					index++;
				} while (index < endIndex && text[index] == '<');
			}

			if (!ParseUtils.SkipCommentsAndWhiteSpace (text, ref index, endIndex, throwOnError))
				return false;

			if (index >= endIndex) {
				if (throwOnError)
					throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Incomplete mailbox at offset {0}", startIndex), startIndex, index);

				return false;
			}

			if (text[index] == (byte) '@') {
				// Note: we always pass 'false' as the throwOnError argument here so that we can throw a more informative exception on error
				if (!DomainList.TryParse (text, ref index, endIndex, false, out route)) {
					if (throwOnError)
						throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Invalid route in mailbox at offset {0}", startIndex), startIndex, index);

					return false;
				}

				if (index >= endIndex || text[index] != (byte) ':') {
					if (throwOnError)
						throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Incomplete route in mailbox at offset {0}", startIndex), startIndex, index);

					return false;
				}

				// skip over ':'
				index++;

				if (!ParseUtils.SkipCommentsAndWhiteSpace (text, ref index, endIndex, throwOnError))
					return false;

				if (index >= endIndex) {
					if (throwOnError)
						throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Incomplete mailbox at offset {0}", startIndex), startIndex, index);

					return false;
				}
			}

			// Note: The only syntactically correct sentinel token here is the '>', but alas... to deal with the first example
			// in section 7.1.5 of rfc7103, we need to at least handle ',' as a sentinel and might as well handle ';' as well
			// in case the mailbox is within a group address.
			//
			// Example: <third@example.net, fourth@example.net>
			if (!TryParseAddrspec (text, ref index, endIndex, CommaGreaterThanOrSemiColon, options.AddressParserComplianceMode, throwOnError, out string addrspec, out int at))
				return false;

			if (!ParseUtils.SkipCommentsAndWhiteSpace (text, ref index, endIndex, throwOnError))
				return false;

			if (index >= endIndex || text[index] != (byte) '>') {
				if (options.AddressParserComplianceMode == RfcComplianceMode.Strict) {
					if (throwOnError)
						throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Unexpected end of mailbox at offset {0}", startIndex), startIndex, index);

					return false;
				}
			} else {
				// skip over the '>'
				index++;

				// Note: check for excessive angle brackets like the example described in section 7.1.2 of rfc7103...
				if (index < endIndex && text[index] == (byte) '>') {
					if (options.AddressParserComplianceMode == RfcComplianceMode.Strict) {
						if (throwOnError)
							throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Excessive angle brackets at offset {0}", index), startIndex, index);

						return false;
					}

					do {
						index++;
					} while (index < endIndex && text[index] == '>');
				}
			}

			if (route != null)
				address = new MailboxAddress (encoding, name, route, addrspec, at);
			else
				address = new MailboxAddress (encoding, name, addrspec, at);

			return true;
		}

		static bool TryParseGroup (AddressParserFlags flags, ParserOptions options, byte[] text, int startIndex, ref int index, int endIndex, int groupDepth, string name, int codepage, out InternetAddress address)
		{
			var encoding = CharsetUtils.GetEncodingOrDefault (codepage, Encoding.UTF8);
			bool throwOnError = (flags & AddressParserFlags.ThrowOnError) != 0;

			// skip over the ':'
			index++;

			while (index < endIndex && (text[index] == ':' || text[index].IsBlank ()))
				index++;

			if (InternetAddressList.TryParse (flags | AddressParserFlags.AllowMailboxAddress, options, text, ref index, endIndex, true, groupDepth, out var members))
				address = new GroupAddress (encoding, name, members);
			else
				address = new GroupAddress (encoding, name);

			if (index >= endIndex || text[index] != (byte) ';') {
				if (throwOnError && options.AddressParserComplianceMode == RfcComplianceMode.Strict)
					throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Expected to find ';' at offset {0}", index), startIndex, index);

				while (index < endIndex && text[index] != (byte) ';')
					index++;
			} else {
				index++;
			}

			return true;
		}

		internal static bool TryParse (AddressParserFlags flags, ParserOptions options, byte[] text, ref int index, int endIndex, int groupDepth, out InternetAddress address)
		{
			bool throwOnError = (flags & AddressParserFlags.ThrowOnError) != 0;

			address = null;

			if (!ParseUtils.SkipCommentsAndWhiteSpace (text, ref index, endIndex, throwOnError))
				return false;

			if (index == endIndex) {
				if (throwOnError)
					throw new ParseException ("No address found.", index, index);

				return false;
			}

			// keep track of the start & length of the phrase
			bool trimLeadingQuote = false;
			int startIndex = index;
			int length = 0;
			int words = 0;

			while (index < endIndex) {
				bool quoted = text[index] == (byte) '"';

				if (options.AddressParserComplianceMode == RfcComplianceMode.Strict) {
					if (!ParseUtils.SkipWord (text, ref index, endIndex, throwOnError))
						break;
				} else if (text[index] == (byte) '"') {
					int qstringIndex = index;

					if (!ParseUtils.SkipQuoted (text, ref index, endIndex, false)) {
						index = qstringIndex + 1;

						ParseUtils.SkipWhiteSpace (text, ref index, endIndex);

						if (!ParseUtils.SkipPhraseAtom (text, ref index, endIndex)) {
							if (throwOnError)
								throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Incomplete quoted-string token at offset {0}", qstringIndex), qstringIndex, endIndex);

							break;
						}

						if (startIndex == qstringIndex)
							trimLeadingQuote = true;
					}
				} else {
					if (!ParseUtils.SkipPhraseAtom (text, ref index, endIndex))
						break;
				}

				length = index - startIndex;

				do {
					if (!ParseUtils.SkipCommentsAndWhiteSpace (text, ref index, endIndex, throwOnError))
						return false;

					// Note: some clients don't quote dots in the name
					if (index >= endIndex || text[index] != (byte) '.')
						break;

					index++;

					length = index - startIndex;
				} while (true);

				words++;

				// Note: some clients don't quote commas in the name
				if (options.AllowUnquotedCommasInAddresses && index < endIndex && text[index] == ',' && !quoted) {
					index++;

					length = index - startIndex;

					if (!ParseUtils.SkipCommentsAndWhiteSpace (text, ref index, endIndex, throwOnError))
						return false;
				}
			}

			if (!ParseUtils.SkipCommentsAndWhiteSpace (text, ref index, endIndex, throwOnError))
				return false;

			// specials    =  "(" / ")" / "<" / ">" / "@"  ; Must be in quoted-
			//             /  "," / ";" / ":" / "\" / <">  ;  string, to use
			//             /  "." / "[" / "]"              ;  within a word.

			if (index >= endIndex || text[index] == (byte) ',' || text[index] == (byte) '>' || text[index] == ';') {
				// we've completely gobbled up an addr-spec w/o a domain
				byte sentinel = index < endIndex ? text[index] : (byte) ',';
				string name;

				if ((flags & AddressParserFlags.AllowMailboxAddress) == 0) {
					if (throwOnError)
						throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Addr-spec token at offset {0}", startIndex), startIndex, index);

					return false;
				}

				if (!options.AllowAddressesWithoutDomain) {
					if (throwOnError)
						throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Incomplete addr-spec token at offset {0}", startIndex), startIndex, index);

					return false;
				}

				// rewind back to the beginning of the local-part
				index = startIndex;

				if (!TryParseLocalPart (text, ref index, endIndex, options.AddressParserComplianceMode, false, throwOnError, out var addrspec))
					return false;

				ParseUtils.SkipWhiteSpace (text, ref index, endIndex);

				if (index < endIndex && text[index] == '(') {
					int comment = index + 1;

					// Note: this can't fail because it has already been skipped in TryParseLocalPart() above.
					ParseUtils.SkipComment (text, ref index, endIndex);

					name = Rfc2047.DecodePhrase (options, text, comment, (index - 1) - comment).Trim ();

					ParseUtils.SkipCommentsAndWhiteSpace (text, ref index, endIndex, throwOnError);
				} else {
					name = string.Empty;
				}

				if (index < endIndex && text[index] == (byte) '>') {
					if (options.AddressParserComplianceMode == RfcComplianceMode.Strict) {
						if (throwOnError)
							throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Unexpected '>' token at offset {0}", index), startIndex, index);

						return false;
					}

					index++;
				}

				if (index < endIndex && text[index] != sentinel) {
					if (throwOnError)
						throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Unexpected '{0}' token at offset {1}", (char) text[index], index), startIndex, index);

					return false;
				}

				address = new MailboxAddress (Encoding.UTF8, name, addrspec, -1);

				return true;
			}

			if (text[index] == (byte) ':') {
				// rfc2822 group address
				int nameIndex = startIndex;
				int codepage = -1;
				string name;

				if ((flags & AddressParserFlags.AllowGroupAddress) == 0) {
					if (throwOnError)
						throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Group address token at offset {0}", startIndex), startIndex, index);

					return false;
				}

				if (groupDepth >= options.MaxAddressGroupDepth) {
					if (throwOnError)
						throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Exceeded maximum rfc822 group depth at offset {0}", startIndex), startIndex, index);

					return false;
				}

				if (trimLeadingQuote) {
					nameIndex++;
					length--;
				}

				if (length > 0) {
					name = Rfc2047.DecodePhrase (options, text, nameIndex, length, out codepage);
				} else {
					name = string.Empty;
				}

				if (codepage == -1)
					codepage = 65001;

				return TryParseGroup (flags, options, text, startIndex, ref index, endIndex, groupDepth + 1, MimeUtils.Unquote (name, true), codepage, out address);
			}

			if ((flags & AddressParserFlags.AllowMailboxAddress) == 0) {
				if (throwOnError)
					throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Mailbox address token at offset {0}", startIndex), startIndex, index);

				return false;
			}

			if (text[index] == (byte) '@') {
				// we're either in the middle of an addr-spec token or we completely gobbled up an addr-spec w/o a domain
				string name;

				// rewind back to the beginning of the local-part
				index = startIndex;

				if (!TryParseAddrspec (text, ref index, endIndex, CommaGreaterThanOrSemiColon, options.AddressParserComplianceMode, throwOnError, out var addrspec, out int at))
					return false;

				ParseUtils.SkipWhiteSpace (text, ref index, endIndex);

				if (index < endIndex && text[index] == '(') {
					int comment = index;

					if (!ParseUtils.SkipComment (text, ref index, endIndex)) {
						if (throwOnError)
							throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Incomplete comment token at offset {0}", comment), comment, index);

						return false;
					}

					comment++;

					name = Rfc2047.DecodePhrase (options, text, comment, (index - 1) - comment).Trim ();
				} else {
					name = string.Empty;
				}

				if (!ParseUtils.SkipCommentsAndWhiteSpace (text, ref index, endIndex, throwOnError))
					return false;

				if (index >= endIndex) {
					address = new MailboxAddress (Encoding.UTF8, name, addrspec, at);
					return true;
				}

				if (text[index] == (byte) '<') {
					// We have an address like "user@example.com <user@example.com>"; i.e. the name is an unquoted string with an '@'.
					if (options.AddressParserComplianceMode == RfcComplianceMode.Strict) {
						if (throwOnError)
							throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Unexpected '<' token at offset {0}", index), startIndex, index);

						return false;
					}

					int nameEndIndex = index;
					while (nameEndIndex > startIndex && text[nameEndIndex - 1].IsWhitespace ())
						nameEndIndex--;

					length = nameEndIndex - startIndex;

					// fall through to the rfc822 angle-addr token case...
				} else {
					// Note: since there was no '<', there should not be a '>'... but we handle it anyway in order to
					// deal with the second Unbalanced Angle Brackets example in section 7.1.3: second@example.org>
					if (text[index] == (byte) '>') {
						if (options.AddressParserComplianceMode == RfcComplianceMode.Strict) {
							if (throwOnError)
								throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Unexpected '>' token at offset {0}", index), startIndex, index);

							return false;
						}

						index++;
					}

					address = new MailboxAddress (Encoding.UTF8, name, addrspec, at);

					return true;
				}
			}

			if (text[index] == (byte) '<') {
				// rfc2822 angle-addr token
				int nameIndex = startIndex;
				int codepage = -1;
				string name;

				if (trimLeadingQuote) {
					nameIndex++;
					length--;
				}

				if (length > 0) {
					var unquoted = MimeUtils.Unquote (text, nameIndex, length, true);

					name = Rfc2047.DecodePhrase (options, unquoted, 0, unquoted.Length, out codepage);
				} else {
					name = string.Empty;
				}

				if (codepage == -1)
					codepage = 65001;

				return TryParseMailbox (options, text, startIndex, ref index, endIndex, name, codepage, throwOnError, out address);
			}

			if (throwOnError)
				throw new ParseException (string.Format (CultureInfo.InvariantCulture, "Invalid address token at offset {0}", startIndex), startIndex, index);

			return false;
		}
	}
}
