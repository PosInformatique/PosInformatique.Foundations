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
        public void IsAutoCad()
        {
            MimeTypes.Application.Docx.IsAutoCad().Should().BeFalse();
            MimeTypes.Application.OctetStream.IsAutoCad().Should().BeFalse();
            MimeTypes.Application.Pdf.IsAutoCad().Should().BeFalse();
            MimeTypes.Image.Bmp.IsAutoCad().Should().BeFalse();
            MimeTypes.Image.Dwg.IsAutoCad().Should().BeTrue();
            MimeTypes.Image.Dxf.IsAutoCad().Should().BeTrue();
            MimeTypes.Image.Jpeg.IsAutoCad().Should().BeFalse();
            MimeTypes.Image.Png.IsAutoCad().Should().BeFalse();
            MimeTypes.Image.Svg.IsAutoCad().Should().BeFalse();
            MimeTypes.Image.Tiff.IsAutoCad().Should().BeFalse();
            MimeTypes.Image.WebP.IsAutoCad().Should().BeFalse();
            MimeTypes.Text.Csv.IsAutoCad().Should().BeFalse();
            MimeTypes.Text.Html.IsAutoCad().Should().BeFalse();
            MimeTypes.Text.Markdown.IsAutoCad().Should().BeFalse();
            MimeTypes.Text.Plain.IsAutoCad().Should().BeFalse();
            MimeTypes.Text.Rtf.IsAutoCad().Should().BeFalse();
            MimeTypes.Text.TabSeparatedValues.IsAutoCad().Should().BeFalse();
        }

        [Fact]
        public void IsAutoCad_WithNullArgument()
        {
            var act = () =>
            {
                MimeTypeExtensions.IsAutoCad(null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("mimeType");
        }

        [Fact]
        public void IsImage()
        {
            MimeTypes.Application.Docx.IsImage().Should().BeFalse();
            MimeTypes.Application.OctetStream.IsImage().Should().BeFalse();
            MimeTypes.Application.Pdf.IsImage().Should().BeFalse();
            MimeTypes.Image.Bmp.IsImage().Should().BeTrue();
            MimeTypes.Image.Dwg.IsImage().Should().BeFalse();
            MimeTypes.Image.Dxf.IsImage().Should().BeFalse();
            MimeTypes.Image.Jpeg.IsImage().Should().BeTrue();
            MimeTypes.Image.Png.IsImage().Should().BeTrue();
            MimeTypes.Image.Svg.IsImage().Should().BeTrue();
            MimeTypes.Image.Tiff.IsImage().Should().BeTrue();
            MimeTypes.Image.WebP.IsImage().Should().BeTrue();
            MimeTypes.Text.Csv.IsImage().Should().BeFalse();
            MimeTypes.Text.Html.IsImage().Should().BeFalse();
            MimeTypes.Text.Markdown.IsImage().Should().BeFalse();
            MimeTypes.Text.Plain.IsImage().Should().BeFalse();
            MimeTypes.Text.Rtf.IsImage().Should().BeFalse();
            MimeTypes.Text.TabSeparatedValues.IsImage().Should().BeFalse();
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

        [Fact]
        public void IsPdf()
        {
            MimeTypes.Application.Docx.IsPdf().Should().BeFalse();
            MimeTypes.Application.OctetStream.IsPdf().Should().BeFalse();
            MimeTypes.Application.Pdf.IsPdf().Should().BeTrue();
            MimeTypes.Image.Bmp.IsPdf().Should().BeFalse();
            MimeTypes.Image.Dwg.IsPdf().Should().BeFalse();
            MimeTypes.Image.Dxf.IsPdf().Should().BeFalse();
            MimeTypes.Image.Jpeg.IsPdf().Should().BeFalse();
            MimeTypes.Image.Png.IsPdf().Should().BeFalse();
            MimeTypes.Image.Svg.IsPdf().Should().BeFalse();
            MimeTypes.Image.Tiff.IsPdf().Should().BeFalse();
            MimeTypes.Image.WebP.IsPdf().Should().BeFalse();
            MimeTypes.Text.Csv.IsPdf().Should().BeFalse();
            MimeTypes.Text.Html.IsPdf().Should().BeFalse();
            MimeTypes.Text.Markdown.IsPdf().Should().BeFalse();
            MimeTypes.Text.Plain.IsPdf().Should().BeFalse();
            MimeTypes.Text.Rtf.IsPdf().Should().BeFalse();
            MimeTypes.Text.TabSeparatedValues.IsPdf().Should().BeFalse();
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
        public void IsText()
        {
            MimeTypes.Application.Docx.IsText().Should().BeFalse();
            MimeTypes.Application.OctetStream.IsText().Should().BeFalse();
            MimeTypes.Application.Pdf.IsText().Should().BeFalse();
            MimeTypes.Image.Bmp.IsText().Should().BeFalse();
            MimeTypes.Image.Dwg.IsText().Should().BeFalse();
            MimeTypes.Image.Dxf.IsText().Should().BeFalse();
            MimeTypes.Image.Jpeg.IsText().Should().BeFalse();
            MimeTypes.Image.Png.IsText().Should().BeFalse();
            MimeTypes.Image.Svg.IsText().Should().BeFalse();
            MimeTypes.Image.Tiff.IsText().Should().BeFalse();
            MimeTypes.Image.WebP.IsText().Should().BeFalse();
            MimeTypes.Text.Csv.IsText().Should().BeTrue();
            MimeTypes.Text.Html.IsText().Should().BeTrue();
            MimeTypes.Text.Markdown.IsText().Should().BeTrue();
            MimeTypes.Text.Plain.IsText().Should().BeTrue();
            MimeTypes.Text.Rtf.IsText().Should().BeTrue();
            MimeTypes.Text.TabSeparatedValues.IsText().Should().BeTrue();
        }

        [Fact]
        public void IsText_WithNullArgument()
        {
            var act = () =>
            {
                MimeTypeExtensions.IsText(null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("mimeType");
        }
    }
}