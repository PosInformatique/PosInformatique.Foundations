//
// ParserOptions.cs
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
using System.Reflection;
using System.Collections.Generic;

#if ENABLE_CRYPTO
using MimeKit.Cryptography;
#endif

using MimeKit.Utils;
using System.Diagnostics.CodeAnalysis;

namespace MimeKit {
	/// <summary>
	/// Parser options as used by <see cref="MimeParser"/> as well as various Parse and TryParse methods in MimeKit.
	/// </summary>
	/// <remarks>
	/// <see cref="ParserOptions"/> allows you to change and/or override default parsing options used by methods such
	/// as <see cref="MimeMessage.Load(ParserOptions,System.IO.Stream,System.Threading.CancellationToken)"/> and others.
	/// </remarks>
	internal class ParserOptions
	{
		/// <summary>
		/// The default parser options.
		/// </summary>
		/// <remarks>
		/// If a <see cref="ParserOptions"/> is not supplied to <see cref="MimeParser"/> or other Parse and TryParse
		/// methods throughout MimeKit, <see cref="ParserOptions.Default"/> will be used.
		/// </remarks>
		public static readonly ParserOptions Default = new ParserOptions ();

		/// <summary>
		/// Get or set the compliance mode that should be used when parsing rfc822 addresses.
		/// </summary>
		/// <remarks>
		/// <para>In general, you'll probably want this value to be <see cref="RfcComplianceMode.Loose"/>
		/// (the default) as it allows maximum interoperability with existing (broken) mail clients
		/// and other mail software such as sloppily written perl scripts (aka spambots).</para>
		/// <note type="tip">Even in <see cref="RfcComplianceMode.Strict"/> mode, the address parser
		/// is fairly liberal in what it accepts. Setting it to <see cref="RfcComplianceMode.Loose"/>
		/// just makes it try harder to deal with garbage input.</note>
		/// </remarks>
		/// <value>The RFC compliance mode.</value>
		public RfcComplianceMode AddressParserComplianceMode { get; set; }

		/// <summary>
		/// Get or set whether the rfc822 address parser should ignore unquoted commas in address names.
		/// </summary>
		/// <remarks>
		/// <para>In general, you'll probably want this value to be <see langword="true" /> (the default) as it allows
		/// maximum interoperability with existing (broken) mail clients and other mail software such as
		/// sloppily written perl scripts (aka spambots) that do not properly quote the name when it
		/// contains a comma.</para>
		/// </remarks>
		/// <value><see langword="true" /> if the address parser should ignore unquoted commas in address names; otherwise, <see langword="false" />.</value>
		public bool AllowUnquotedCommasInAddresses { get; set; }

		/// <summary>
		/// Get or set whether the rfc822 address parser should allow addresses without a domain.
		/// </summary>
		/// <remarks>
		/// <para>In general, you'll probably want this value to be <see langword="true" /> (the default) as it allows
		/// maximum interoperability with older email messages that may contain local UNIX addresses.</para>
		/// <para>This option exists in order to allow parsing of mailbox addresses that do not have an
		/// @domain component. These types of addresses are rare and were typically only used when sending
		/// mail to other users on the same UNIX system.</para>
		/// </remarks>
		/// <value><see langword="true" /> if the address parser should allow mailbox addresses without a domain; otherwise, <see langword="false" />.</value>
		public bool AllowAddressesWithoutDomain { get; set; }

		/// <summary>
		/// Get or set the maximum address group depth the parser should accept.
		/// </summary>
		/// <remarks>
		/// <para>This option exists in order to define the maximum recursive depth of an rfc822 group address
		/// that the parser should accept before bailing out with the assumption that the address is maliciously
		/// formed. If the value is set too large, then it is possible that a maliciously formed set of
		/// recursive group addresses could cause a stack overflow.</para>
		/// </remarks>
		/// <value>The maximum address group depth.</value>
		public int MaxAddressGroupDepth { get; set; }

		/// <summary>
		/// Get or set the maximum MIME nesting depth the parser should accept.
		/// </summary>
		/// <remarks>
		/// <para>This option exists in order to define the maximum recursive depth of MIME parts that the parser
		/// should accept before treating further nesting as a leaf-node MIME part and not recursing any further.
		/// If the value is set too large, then it is possible that a maliciously formed set of deeply nested
		/// multipart MIME parts could cause a stack overflow.</para>
		/// </remarks>
		/// <value>The maximum MIME nesting depth.</value>
		public int MaxMimeDepth { get; set; }

		/// <summary>
		/// Get or set the compliance mode that should be used when parsing Content-Type and Content-Disposition parameters.
		/// </summary>
		/// <remarks>
		/// <para>In general, you'll probably want this value to be <see cref="RfcComplianceMode.Loose"/>
		/// (the default) as it allows maximum interoperability with existing (broken) mail clients
		/// and other mail software such as sloppily written perl scripts (aka spambots).</para>
		/// <note type="tip">Even in <see cref="RfcComplianceMode.Strict"/> mode, the parameter parser
		/// is fairly liberal in what it accepts. Setting it to <see cref="RfcComplianceMode.Loose"/>
		/// just makes it try harder to deal with garbage input.</note>
		/// </remarks>
		/// <value>The RFC compliance mode.</value>
		public RfcComplianceMode ParameterComplianceMode { get; set; }

		/// <summary>
		/// Get or set the compliance mode that should be used when decoding rfc2047 encoded words.
		/// </summary>
		/// <remarks>
		/// In general, you'll probably want this value to be <see cref="RfcComplianceMode.Loose"/>
		/// (the default) as it allows maximum interoperability with existing (broken) mail clients
		/// and other mail software such as sloppily written perl scripts (aka spambots).
		/// </remarks>
		/// <value>The RFC compliance mode.</value>
		public RfcComplianceMode Rfc2047ComplianceMode { get; set; }

		/// <summary>
		/// Get or set a value indicating whether the Content-Length value should be
		/// respected when parsing mbox streams.
		/// </summary>
		/// <remarks>
		/// For more details about why this may be useful, you can find more information
		/// at <a href="http://www.jwz.org/doc/content-length.html">
		/// http://www.jwz.org/doc/content-length.html</a>.
		/// </remarks>
		/// <value><see langword="true" /> if the Content-Length value should be respected;
		/// otherwise, <see langword="false" />.</value>
		public bool RespectContentLength { get; set; }

		/// <summary>
		/// Get or set the charset encoding to use as a fallback for 8bit headers.
		/// </summary>
		/// <remarks>
		/// <see cref="Rfc2047.DecodeText(ParserOptions, byte[])"/> and
		/// <see cref="Rfc2047.DecodePhrase(ParserOptions, byte[])"/>
		/// use this charset encoding as a fallback when decoding 8bit text into unicode. The first
		/// charset encoding attempted is UTF-8, followed by this charset encoding, before finally
		/// falling back to iso-8859-1.
		/// </remarks>
		/// <value>The charset encoding.</value>
		public Encoding CharsetEncoding { get; set; }

		/// <summary>
		/// Initialize a new instance of the <see cref="ParserOptions"/> class.
		/// </summary>
		/// <remarks>
		/// By default, new instances of <see cref="ParserOptions"/> enable rfc2047 work-arounds
		/// (which are needed for maximum interoperability with mail software used in the wild)
		/// and do not respect the Content-Length header value.
		/// </remarks>
		public ParserOptions ()
		{
			AddressParserComplianceMode = RfcComplianceMode.Loose;
			ParameterComplianceMode = RfcComplianceMode.Loose;
			Rfc2047ComplianceMode = RfcComplianceMode.Loose;
			CharsetEncoding = CharsetUtils.UTF8;
			AllowUnquotedCommasInAddresses = true;
			AllowAddressesWithoutDomain = true;
			RespectContentLength = false;
			MaxAddressGroupDepth = 3;
			MaxMimeDepth = 1024;
		}
	}
}
