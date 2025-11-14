//-----------------------------------------------------------------------
// <copyright file="EmailingServiceCollectionExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.Extensions.DependencyInjection
{
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using PosInformatique.Foundations.Emailing;

    /// <summary>
    /// Contains extension methods to register the e-mailing feature in the <see cref="IServiceCollection"/>.
    /// </summary>
    public static class EmailingServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the e-mailing feature.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> where to register the services.</param>
        /// <param name="options">Options of the <see cref="IEmailManager"/>.</param>
        /// <returns>An instance of <see cref="EmailingBuilder"/> to continue the configuration for the e-mailing feature.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="services"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="options"/> argument is <see langword="null"/>.</exception>
        public static EmailingBuilder AddEmailing(this IServiceCollection services, Action<EmailingOptions> options)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(options);

            services.TryAddScoped<IEmailManager, EmailManager>();

            services.Configure(options);

            return new EmailingBuilder(services);
        }
    }
}