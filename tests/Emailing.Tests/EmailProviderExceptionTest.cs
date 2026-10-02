//-----------------------------------------------------------------------
// <copyright file="EmailProviderExceptionTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Tests
{
    public class EmailProviderExceptionTest
    {
        [Fact]
        public void Constructor()
        {
            var exception = new EmailProviderException();

            exception.Message.Should().Be("Exception of type 'PosInformatique.Foundations.Emailing.EmailProviderException' was thrown.");
            exception.InnerException.Should().BeNull();
        }

        [Fact]
        public void Constructor_WithMessage()
        {
            var exception = new EmailProviderException("The message");

            exception.Message.Should().Be("The message");
            exception.InnerException.Should().BeNull();
        }

        [Fact]
        public void Constructor_WithMessageAndInnerException()
        {
            var innerException = new FormatException("The inner exception");
            var exception = new EmailProviderException("The message", innerException);

            exception.Message.Should().Be("The message");
            exception.InnerException.Should().BeSameAs(innerException);
        }
    }
}