//-----------------------------------------------------------------------
// <copyright file="RazorEmailTemplateServiceCollectionExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to configure the Email templates based on Razor components.
    /// </summary>
    public static class RazorEmailTemplateServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Razor text templating support to the emailing services.
        /// </summary>
        /// <param name="builder"><see cref="EmailingBuilder"/> to configure.</param>
        /// <returns>The <paramref name="builder"/> instance to continue the configuration of the emailing service.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="builder"/> argument is <see langword="null"/>.</exception>
        public static EmailingBuilder UseRazorEmailTemplates(this EmailingBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.Services.AddRazorTextTemplating();

            return builder;
        }
    }
}