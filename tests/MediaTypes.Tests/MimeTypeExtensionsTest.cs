//-----------------------------------------------------------------------
// <copyright file="MimeTypeExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.MediaTypes.Tests
{
    public class MimeTypeExtensionsTest
    {
        [Fact]
        public void IsPdf()
        {
            MimeTypes.Application.Docx.IsPdf().Should().BeFalse();
            MimeTypes.Application.Pdf.IsPdf().Should().BeTrue();
            MimeTypes.Application.OctetStream.IsPdf().Should().BeFalse();
            MimeTypes.Image.Bmp.IsPdf().Should().BeFalse();
            MimeTypes.Image.Jpeg.IsPdf().Should().BeFalse();
            MimeTypes.Image.Png.IsPdf().Should().BeFalse();
            MimeTypes.Image.Tiff.IsPdf().Should().BeFalse();
            MimeTypes.Image.WebP.IsPdf().Should().BeFalse();
        }

        [Fact]
        public void IsPdf_WithNullArgument()
        {
            var act = () =>
            {
                MimeTypeExtensions.IsPdf(null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("mimeType");
        }

        [Fact]
        public void IsImage()
        {
            MimeTypes.Application.Docx.IsImage().Should().BeFalse();
            MimeTypes.Application.Pdf.IsImage().Should().BeFalse();
            MimeTypes.Application.OctetStream.IsPdf().Should().BeFalse();
            MimeTypes.Image.Bmp.IsImage().Should().BeTrue();
            MimeTypes.Image.Jpeg.IsImage().Should().BeTrue();
            MimeTypes.Image.Png.IsImage().Should().BeTrue();
            MimeTypes.Image.Tiff.IsImage().Should().BeTrue();
            MimeTypes.Image.WebP.IsImage().Should().BeTrue();
        }

        [Fact]
        public void IsImage_WithNullArgument()
        {
            var act = () =>
            {
                MimeTypeExtensions.IsImage(null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("mimeType");
        }
    }
}