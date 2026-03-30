//-----------------------------------------------------------------------
// <copyright file="EmailAttachmentTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Tests
{
    using PosInformatique.Foundations.MediaTypes;

    public class EmailAttachmentTest
    {
        [Fact]
        public void Constructor()
        {
            var content = new Mock<Stream>(MockBehavior.Strict);
            content.Setup(s => s.CanRead)
                .Returns(true);

            var attachment = new EmailAttachment(
                "File.pdf",
                MimeTypes.Application.Pdf,
                content.Object);

            attachment.Content.Should().BeSameAs(content.Object);
            attachment.ContentType.Should().Be(MimeTypes.Application.Pdf);
            attachment.FileName.Should().Be("File.pdf");

            content.VerifyAll();
        }

        [Fact]
        public void Constructor_WithContentNoReadable()
        {
            var content = new Mock<Stream>(MockBehavior.Strict);
            content.Setup(s => s.CanRead)
                .Returns(false);

            var act = () =>
            {
                new EmailAttachment(
                    "File.pdf",
                    MimeTypes.Application.Pdf,
                    content.Object);
            };

            act.Should().ThrowExactly<ArgumentException>()
                .WithMessage("The content stream must be readable. (Parameter 'content')")
                .WithParameterName("content");

            content.VerifyAll();
        }

        [Fact]
        public void Constructor_WithContentArgumentNull()
        {
            var act = () =>
            {
                new EmailAttachment(
                    "File.pdf",
                    MimeTypes.Application.Pdf,
                    null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("content");
        }

        [Fact]
        public void Constructor_WithContentTypeArgumentNull()
        {
            var act = () =>
            {
                new EmailAttachment(
                    "File.pdf",
                    null,
                    default);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("contentType");
        }

        [Fact]
        public void Constructor_WithFileNameArgumentNull()
        {
            var act = () =>
            {
                new EmailAttachment(
                    null,
                    default,
                    default);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("fileName");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_WithFileNameWithWhitespaces(string fileName)
        {
            var act = () =>
            {
                new EmailAttachment(
                    fileName,
                    default,
                    default);
            };

            act.Should().ThrowExactly<ArgumentException>()
                .WithMessage("The value cannot be an empty string or composed entirely of whitespace. (Parameter 'fileName')")
                .WithParameterName("fileName");
        }
    }
}