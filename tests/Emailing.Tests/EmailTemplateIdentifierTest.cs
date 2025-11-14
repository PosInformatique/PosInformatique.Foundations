//-----------------------------------------------------------------------
// <copyright file="EmailTemplateIdentifierTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Tests
{
    public class EmailTemplateIdentifierTest
    {
        [Fact]
        public void Constructor()
        {
            var identifier = EmailTemplateIdentifier<Model>.Create();
            var otherIdentifier = EmailTemplateIdentifier<Model>.Create();

            identifier.Should().NotBeSameAs(otherIdentifier);
        }

        private sealed class Model : EmailModel
        {
        }
    }
}