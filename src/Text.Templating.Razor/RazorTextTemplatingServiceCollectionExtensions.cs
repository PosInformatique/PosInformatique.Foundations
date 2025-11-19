//-----------------------------------------------------------------------
// <copyright file="RazorTextTemplatingServiceCollectionExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.Extensions.DependencyInjection
{
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using PosInformatique.Foundations.Text.Templating.Razor;

    /// <summary>
    /// Contains extension methods to register the Razor text templating feature in the <see cref="IServiceCollection"/>.
    /// </summary>
    public static class RazorTextTemplatingServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the Razor text templating engine in the specified <paramref name="services"/>.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> where the text templating engine will be registered.</param>
        /// <returns>The <paramref name="services"/> instance ton continue the configuration.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="services"/> argument is <see langword="null"/>.</exception>
        public static IServiceCollection AddRazorTextTemplating(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddLogging();
            services.TryAddScoped<IRazorTextTemplateRenderer, RazorTextTemplateRenderer>();

            return services;
        }
    }
}