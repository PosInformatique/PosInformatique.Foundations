//-----------------------------------------------------------------------
// <copyright file="RazorTextTemplatingServiceCollectionExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.Extensions.DependencyInjection.Tests
{
    using Microsoft.Extensions.Logging;
    using PosInformatique.Foundations.Text.Templating.Razor;

    public class RazorTextTemplatingServiceCollectionExtensionsTest
    {
        [Fact]
        public void AddRazorTextTemplating()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddRazorTextTemplating().Should().BeSameAs(serviceCollection);

            var sp = serviceCollection.BuildServiceProvider();

            sp.GetRequiredService<IRazorTextTemplateRenderer>().Should().BeOfType<RazorTextTemplateRenderer>();
            sp.GetRequiredService<ILogger<string>>().Should().NotBeNull();
        }

        [Fact]
        public void AddRazorTextTemplating_WithServicesArgumentNull()
        {
            var act = () =>
            {
                RazorTextTemplatingServiceCollectionExtensions.AddRazorTextTemplating(null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("services");
        }
    }
}