//-----------------------------------------------------------------------
// <copyright file="MimeTypeTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.MediaTypes.Tests
{
    public class MimeTypeTest
    {
        [Theory]
        [InlineData("text/plain", "text", "plain")]
        [InlineData("image/jpeg", "image", "jpeg")]
        public void Parse_Success(string input, string expectedType, string expectedSubtype)
        {
            var mimeType = MimeType.Parse(input);

            mimeType.Type.Should().Be(expectedType);
            mimeType.Subtype.Should().Be(expectedSubtype);
        }

        [Fact]
        public void Parse_WithNullArgument()
        {
            var act = () =>
            {
                MimeType.Parse(null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("s");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("part1")]
        [InlineData("part1/part2/part3")]
        [InlineData("part1/")]
        [InlineData("/part2")]
        public void Parse_Failed(string input)
        {
            var act = () =>
            {
                MimeType.Parse(input);
            };

            act.Should().ThrowExactly<FormatException>()
                .WithMessage("Invalid MIME type format.");
        }

        [Theory]
        [InlineData("text/plain", "text", "plain")]
        [InlineData("image/jpeg", "image", "jpeg")]
        public void Parse_WithFormatProvider_Success(string input, string expectedType, string expectedSubtype)
        {
            var formatProvider = Mock.Of<IFormatProvider>(MockBehavior.Strict);

            var mimeType = MimeType.Parse(input, formatProvider);

            mimeType.Type.Should().Be(expectedType);
            mimeType.Subtype.Should().Be(expectedSubtype);
        }

        [Fact]
        public void Parse_WithFormatProvider_WithNullArgument()
        {
            var act = () =>
            {
                MimeType.Parse(null, default);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("s");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("part1")]
        [InlineData("part1/part2/part3")]
        [InlineData("part1/")]
        [InlineData("/part2")]
        public void Parse_WithFormatProvider_Failed(string input)
        {
            var formatProvider = Mock.Of<IFormatProvider>(MockBehavior.Strict);

            var act = () =>
            {
                MimeType.Parse(input, formatProvider);
            };

            act.Should().ThrowExactly<FormatException>()
                .WithMessage("Invalid MIME type format.");
        }

        [Theory]
        [InlineData("text/plain", "text", "plain")]
        [InlineData("image/jpeg", "image", "jpeg")]
        public void TryParse_Success(string input, string expectedType, string expectedSubtype)
        {
            MimeType.TryParse(input, out var mimeType).Should().BeTrue();

            mimeType.Type.Should().Be(expectedType);
            mimeType.Subtype.Should().Be(expectedSubtype);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("part1")]
        [InlineData("part1/part2/part3")]
        [InlineData("part1/")]
        [InlineData("/part2")]
        public void TryParse_Failed(string input)
        {
            MimeType.TryParse(input, out var mimeType).Should().BeFalse();

            mimeType.Should().BeNull();
        }

        [Theory]
        [InlineData("text/plain", "text", "plain")]
        [InlineData("image/jpeg", "image", "jpeg")]
        public void TryParse_WithFormatProvider_Success(string input, string expectedType, string expectedSubtype)
        {
            var formatProvider = Mock.Of<IFormatProvider>(MockBehavior.Strict);

            MimeType.TryParse(input, formatProvider, out var mimeType).Should().BeTrue();

            mimeType.Type.Should().Be(expectedType);
            mimeType.Subtype.Should().Be(expectedSubtype);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("part1")]
        [InlineData("part1/part2/part3")]
        [InlineData("part1/")]
        [InlineData("/part2")]
        public void TryParse_WithFormatProvider_Failed(string input)
        {
            var formatProvider = Mock.Of<IFormatProvider>(MockBehavior.Strict);

            MimeType.TryParse(input, formatProvider, out var mimeType).Should().BeFalse();

            mimeType.Should().BeNull();
        }

        [Theory]
        [InlineData("text/plain", "text/plain", true)]
        [InlineData("text/plain", "image/jpeg", false)]
        [InlineData("text/plain", null, false)]
        public void Equals_WithObject(string mimeType1String, string mimeType2String, bool expectedResult)
        {
            var mimeType1 = MimeType.Parse(mimeType1String);
            var mimeType2 = mimeType2String != null ? MimeType.Parse(mimeType2String) : null;

            mimeType1.Equals((object)mimeType2).Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("text/plain", "text/plain", true)]
        [InlineData("text/plain", "image/jpeg", false)]
        [InlineData("text/plain", null, false)]
        public void Equals_WithMimeType(string mimeType1String, string mimeType2String, bool expectedResult)
        {
            var mimeType1 = MimeType.Parse(mimeType1String);
            var mimeType2 = mimeType2String != null ? MimeType.Parse(mimeType2String) : null;

            mimeType1.Equals(mimeType2).Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("text/plain", "text/plain", true)]
        [InlineData("text/plain", "image/jpeg", false)]
        [InlineData("text/plain", null, false)]
        [InlineData(null, null, true)]
        public void OperatorEqual(string mimeType1String, string mimeType2String, bool expectedResult)
        {
            var mimeType1 = mimeType1String != null ? MimeType.Parse(mimeType1String) : null;
            var mimeType2 = mimeType2String != null ? MimeType.Parse(mimeType2String) : null;

            (mimeType1 == mimeType2).Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("text/plain", "text/plain", false)]
        [InlineData("text/plain", "image/jpeg", true)]
        [InlineData("text/plain", null, true)]
        [InlineData(null, null, false)]
        public void OperatorDifferent(string mimeType1String, string mimeType2String, bool expectedResult)
        {
            var mimeType1 = mimeType1String != null ? MimeType.Parse(mimeType1String) : null;
            var mimeType2 = mimeType2String != null ? MimeType.Parse(mimeType2String) : null;

            (mimeType1 != mimeType2).Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("text/plain", "text/plain", true)]
        [InlineData("text/plain", "image/jpeg", false)]
        public void GetHashCode_Test(string mimeType1String, string mimeType2String, bool expectedEqual)
        {
            var mimeType1 = MimeType.Parse(mimeType1String);
            var mimeType2 = mimeType2String != null ? MimeType.Parse(mimeType2String) : null;

            (mimeType1.GetHashCode() == mimeType2.GetHashCode()).Should().Be(expectedEqual);
        }

        [Theory]
        [InlineData("pdf", "Application.Pdf")]
        [InlineData("docx", "Application.Docx")]
        [InlineData("bmp", "Image.Bmp")]
        [InlineData("jpg", "Image.Jpeg")]
        [InlineData("jpeg", "Image.Jpeg")]
        [InlineData("png", "Image.Png")]
        [InlineData("tif", "Image.Tiff")]
        [InlineData("tiff", "Image.Tiff")]
        [InlineData("webp", "Image.WebP")]
        [InlineData("unknown", "Application.OctetStream")]
        public void FromExtension(string extension, string path)
        {
            MimeType.FromExtension(extension).Should().BeSameAs(GetMimeTypeFromPath(path));
            MimeType.FromExtension("." + extension).Should().BeSameAs(GetMimeTypeFromPath(path));
            MimeType.FromExtension(extension.ToUpperInvariant()).Should().BeSameAs(GetMimeTypeFromPath(path));
            MimeType.FromExtension("." + extension.ToUpperInvariant()).Should().BeSameAs(GetMimeTypeFromPath(path));
        }

        [Fact]
        public void FromExtension_WithNullArgument()
        {
            var act = () =>
            {
                MimeType.FromExtension(null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("extension");
        }

        [Theory]
        [InlineData("application/pdf", ".pdf")]
        [InlineData("application/vnd.openxmlformats-officedocument.wordprocessingml.document", ".docx")]
        [InlineData("image/bmp", ".bmp")]
        [InlineData("image/jpeg", ".jpg")]
        [InlineData("image/png", ".png")]
        [InlineData("image/tiff", ".tiff")]
        [InlineData("image/webp", ".webp")]
        [InlineData("other/type", "")]
        public void GetExtension(string mimeType, string expectedExtensions)
        {
            MimeType.Parse(mimeType).GetExtension().Should().Be(expectedExtensions);
        }

        [Fact]
        public void ToString_Test()
        {
            var mimeType = MimeType.Parse("text/plain");

            mimeType.ToString().Should().Be("text/plain");
        }

        private static MimeType GetMimeTypeFromPath(string path)
        {
            var properties = path.Split(".");

            var type = typeof(MimeTypes).GetNestedType(properties[0]);
            var propertySubType = type.GetProperty(properties[1]);

            return (MimeType)propertySubType.GetValue(null);
        }
    }
}