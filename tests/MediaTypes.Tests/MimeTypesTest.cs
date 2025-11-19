//-----------------------------------------------------------------------
// <copyright file="MimeTypesTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.MediaTypes.Tests
{
    public class MimeTypesTest
    {
        [Fact]
        public void Application_Docx()
        {
            MimeTypes.Application.Docx.Should().BeSameAs(MimeTypes.Application.Docx);

            MimeTypes.Application.Docx.Type.Should().Be("application");
            MimeTypes.Application.Docx.Subtype.Should().Be("vnd.openxmlformats-officedocument.wordprocessingml.document");
        }

        [Fact]
        public void Application_OctetStream()
        {
            MimeTypes.Application.OctetStream.Should().BeSameAs(MimeTypes.Application.OctetStream);

            MimeTypes.Application.OctetStream.Type.Should().Be("application");
            MimeTypes.Application.OctetStream.Subtype.Should().Be("octet-stream");
        }

        [Fact]
        public void Application_Pdf()
        {
            MimeTypes.Application.Pdf.Should().BeSameAs(MimeTypes.Application.Pdf);

            MimeTypes.Application.Pdf.Type.Should().Be("application");
            MimeTypes.Application.Pdf.Subtype.Should().Be("pdf");
        }

        [Fact]
        public void Image_Bmp()
        {
            MimeTypes.Image.Bmp.Should().BeSameAs(MimeTypes.Image.Bmp);

            MimeTypes.Image.Bmp.Type.Should().Be("image");
            MimeTypes.Image.Bmp.Subtype.Should().Be("bmp");
        }

        [Fact]
        public void Image_Jpeg()
        {
            MimeTypes.Image.Jpeg.Should().BeSameAs(MimeTypes.Image.Jpeg);

            MimeTypes.Image.Jpeg.Type.Should().Be("image");
            MimeTypes.Image.Jpeg.Subtype.Should().Be("jpeg");
        }

        [Fact]
        public void Image_Dxf()
        {
            MimeTypes.Image.Dxf.Should().BeSameAs(MimeTypes.Image.Dxf);

            MimeTypes.Image.Dxf.Type.Should().Be("image");
            MimeTypes.Image.Dxf.Subtype.Should().Be("x-dxf");
        }

        [Fact]
        public void Image_Dwg()
        {
            MimeTypes.Image.Dwg.Should().BeSameAs(MimeTypes.Image.Dwg);

            MimeTypes.Image.Dwg.Type.Should().Be("image");
            MimeTypes.Image.Dwg.Subtype.Should().Be("x-dwg");
        }

        [Fact]
        public void Image_Png()
        {
            MimeTypes.Image.Png.Should().BeSameAs(MimeTypes.Image.Png);

            MimeTypes.Image.Png.Type.Should().Be("image");
            MimeTypes.Image.Png.Subtype.Should().Be("png");
        }

        [Fact]
        public void Image_Tiff()
        {
            MimeTypes.Image.Tiff.Should().BeSameAs(MimeTypes.Image.Tiff);

            MimeTypes.Image.Tiff.Type.Should().Be("image");
            MimeTypes.Image.Tiff.Subtype.Should().Be("tiff");
        }

        [Fact]
        public void Image_Webp()
        {
            MimeTypes.Image.WebP.Should().BeSameAs(MimeTypes.Image.WebP);

            MimeTypes.Image.WebP.Type.Should().Be("image");
            MimeTypes.Image.WebP.Subtype.Should().Be("webp");
        }
    }
}