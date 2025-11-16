//-----------------------------------------------------------------------
// <copyright file="RazorEmailTemplateSubjectTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Templates.Razor.Tests
{
    public class RazorEmailTemplateSubjectTest
    {
        [Fact]
        public void Constructor()
        {
            var template = Mock.Of<RazorEmailTemplateSubject<Model>>(MockBehavior.Strict);

            var model = new Model();

            template.Model = model;

            template.Model.Should().BeSameAs(model);
        }

        internal sealed class Model : EmailModel
        {
        }
    }
}