//-----------------------------------------------------------------------
// <copyright file="EmailingBuilder.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Used to configure e-mailing feature.
    /// </summary>
    public sealed class EmailingBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailingBuilder"/> class
        /// to configure the e-mailing feature.
        /// </summary>
        /// <param name="services">The services being configured.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="services"/> argument is <see langword="null"/>.</exception>
        public EmailingBuilder(IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            this.Services = services;
        }

        /// <summary>
        /// Gets the services being configured.
        /// </summary>
        public IServiceCollection Services { get; }
    }
}