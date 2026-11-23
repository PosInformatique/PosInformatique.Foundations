//-----------------------------------------------------------------------
// <copyright file="UriExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Bcl.Tests
{
    public class UriExtensionsTest
    {
        [Theory]
        [InlineData("http://base/uri", "http://base/uri/path/1/2", "path/1/2")]
        [InlineData("http://base/uri/", "http://base/uri/path/1/2", "path/1/2")]
        [InlineData("http://base/uri", "http://base/uri/path/1/2", "/path/1/2")]
        [InlineData("http://base/uri/", "http://base/uri/path/1/2", "/path/1/2")]
        [InlineData("http://base/uri", "http://base/uri/path/1/2/", "path/1/2/")]
        [InlineData("http://base/uri/", "http://base/uri/path/1/2/", "path/1/2/")]
        [InlineData("http://base/uri", "http://base/uri/path/1/2/", "/path/1/2/")]
        [InlineData("http://base/uri/", "http://base/uri/path/1/2/", "/path/1/2/")]
        [InlineData("http://base/uri", "http://base/uri/path/1/2", "path", "1", "2")]
        [InlineData("http://base/uri/", "http://base/uri/path/1/2", "path", "1", "2")]
        [InlineData("http://base/uri", "http://base/uri/path/1/2", "/path", "/1", "/2")]
        [InlineData("http://base/uri/", "http://base/uri/path/1/2", "/path", "/1", "/2")]
        [InlineData("http://base/uri", "http://base/uri/path/1/2/", "/path/", "/1/", "/2/")]
        [InlineData("http://base/uri/", "http://base/uri/path/1/2/", "/path/", "/1", "/2/")]
        [InlineData("http://base/uri", "http://base/uri/path/1/2/", "path/", "1/", "2/")]
        [InlineData("http://base/uri/", "http://base/uri/path/1/2/", "path/", "1", "2/")]
        [InlineData("http://base/uri/", "http://base/uri/path/1/2", "path", null, "/", "1", "", "/", "2")]
        [InlineData("http://base/uri/", "http://base/uri/path/1/2/", "path/", null, "/", "1/", "", "/", "2/")]
        [InlineData("http://base/uri", "http://base/uri/path/1/2", "path", null, "/", "1", "", "/", "2")]
        [InlineData("http://base/uri", "http://base/uri/path/1/2/", "path/", null, "/", "1/", "", "/", "2/")]
        [InlineData("http://base/uri/", "http://base/uri/", "", "")]
        [InlineData("http://base/uri/", "http://base/uri/", null, null)]
        [InlineData("http://base/uri", "http://base/uri", "", "")]
        [InlineData("http://base/uri", "http://base/uri", null, null)]
        public void AppendPath(string baseUri, string expectedUri, params string[] paths)
        {
            var uri = new Uri(baseUri);

            var result = uri.AppendPath(paths);

            result.AbsoluteUri.Should().Be(expectedUri);
        }

        [Theory]
        [InlineData("https://example.com/base?value=1", "https://example.com/base/next?value=1")]
        [InlineData("https://example.com/base#section", "https://example.com/base/next#section")]
        [InlineData("https://example.com/a%20b?value=1#section", "https://example.com/a%20b/next?value=1#section")]
        public void AppendPath_PreservesQueryAndFragment(string baseUri, string expectedUri)
        {
            var result = new Uri(baseUri).AppendPath("next");

            result.AbsoluteUri.Should().Be(expectedUri);
        }

        [Fact]
        public void AppendPath_NullArguments()
        {
            Action nullUri = () => UriExtensions.AppendPath(null, "path");
            Action nullPaths = () => new Uri("https://example.com").AppendPath(null);

            nullUri.Should().ThrowExactly<ArgumentNullException>().WithParameterName("uri");
            nullPaths.Should().ThrowExactly<ArgumentNullException>().WithParameterName("paths");
        }

        [Fact]
        public void AppendPath_RelativeUri()
        {
            Action act = () => new Uri("relative", UriKind.Relative).AppendPath("path");

            act.Should().ThrowExactly<InvalidOperationException>();
        }
    }
}