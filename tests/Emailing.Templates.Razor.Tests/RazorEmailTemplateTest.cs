//-----------------------------------------------------------------------
// <copyright file="RazorEmailTemplateTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Templates.Razor.Tests
{
    using PosInformatique.Foundations.Text.Templating.Razor;

    public class RazorEmailTemplateTest
    {
        [Fact]
        public void Create()
        {
            var template = RazorEmailTemplate<Model>.Create<SubjectComponent, BodyComponent>();

            template.HtmlBody.Should().BeOfType<RazorTextTemplate<Model>>();
            template.Subject.Should().BeOfType<RazorTextTemplate<Model>>();
        }

        private sealed class Model
        {
        }

        private sealed class SubjectComponent : RazorEmailTemplateSubject<Model>
        {
        }

        private sealed class BodyComponent : RazorEmailTemplateBody<Model>
        {
        }
    }
}