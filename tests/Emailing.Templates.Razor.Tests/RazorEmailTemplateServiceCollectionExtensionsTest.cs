//-----------------------------------------------------------------------
// <copyright file="RazorEmailTemplateServiceCollectionExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.Extensions.DependencyInjection.Tests
{
    using Microsoft.Extensions.Logging;

    public class RazorEmailTemplateServiceCollectionExtensionsTest
    {
        private static readonly Type IRazorTextTemplateRendererInterface = Type.GetType("PosInformatique.Foundations.Text.Templating.Razor.IRazorTextTemplateRenderer, PosInformatique.Foundations.Text.Templating.Razor");
        private static readonly Type RazorTextTemplateRendererClass = Type.GetType("PosInformatique.Foundations.Text.Templating.Razor.RazorTextTemplateRenderer, PosInformatique.Foundations.Text.Templating.Razor");

        [Fact]
        public void UseRazorEmailTemplates()
        {
            var serviceCollection = new ServiceCollection();
            var emailingBuilder = new EmailingBuilder(serviceCollection);

            emailingBuilder.UseRazorEmailTemplates().Should().BeSameAs(emailingBuilder);

            var sp = serviceCollection.BuildServiceProvider();

            sp.GetRequiredService(IRazorTextTemplateRendererInterface).Should().BeOfType(RazorTextTemplateRendererClass);
            sp.GetRequiredService<ILogger<string>>().Should().NotBeNull();
        }

        [Fact]
        public void UseRazorEmailTemplates_WithBuilderArgumentNull()
        {
            var act = () =>
            {
                RazorEmailTemplateServiceCollectionExtensions.UseRazorEmailTemplates(null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("builder");
        }
    }
}