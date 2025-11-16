//-----------------------------------------------------------------------
// <copyright file="AzureEmailingBuilderExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.Extensions.DependencyInjection
{
    using global::Azure.Communication.Email;
    using global::Azure.Core.Extensions;
    using Microsoft.Extensions.Azure;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using PosInformatique.Foundations.Emailing;
    using PosInformatique.Foundations.Emailing.Azure;

    /// <summary>
    /// Extension methods to configure the <c>Azure Communication Service</c> provider
    /// for the <see cref="IEmailManager"/>.
    /// </summary>
    public static class AzureEmailingBuilderExtensions
    {
        /// <summary>
        /// Configure the provider of <see cref="IEmailManager"/> to use <c>Azure Communication Service</c>.
        /// </summary>
        /// <param name="builder"><see cref="EmailingBuilder"/> which to configure.</param>
        /// <param name="uri">Uri to the the <c>Azure Communication Service</c> instance.</param>
        /// <param name="clientBuilder">Allows to configure the <see cref="EmailClient"/> used by the provider.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="builder"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="uri"/> argument is <see langword="null"/>.</exception>
        public static void UseAzureCommunicationService(this EmailingBuilder builder, Uri uri, Action<IAzureClientBuilder<EmailClient, EmailClientOptions>>? clientBuilder = null)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(uri);

            builder.Services.TryAddSingleton<IEmailProvider, AzureEmailProvider>();

            builder.Services.AddAzureClients(builder =>
            {
                var emailClientBuilder = builder.AddEmailClient(uri);

                if (clientBuilder is not null)
                {
                    clientBuilder(emailClientBuilder);
                }
            });
        }

        /// <summary>
        /// Configure the provider of <see cref="IEmailManager"/> to use <c>Azure Communication Service</c>.
        /// </summary>
        /// <param name="builder"><see cref="EmailingBuilder"/> which to configure.</param>
        /// <param name="connectionString">Connection string to the <c>Azure Communication Service</c> instance.</param>
        /// <param name="clientBuilder">Allows to configure the <see cref="EmailClient"/> used by the provider.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="builder"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="connectionString"/> argument is <see langword="null"/>.</exception>
        public static void UseAzureCommunicationService(this EmailingBuilder builder, string connectionString, Action<IAzureClientBuilder<EmailClient, EmailClientOptions>>? clientBuilder = null)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(connectionString);

            builder.Services.TryAddSingleton<IEmailProvider, AzureEmailProvider>();

            builder.Services.AddAzureClients(builder =>
            {
                var emailClientBuilder = builder.AddEmailClient(connectionString);

                if (clientBuilder is not null)
                {
                    clientBuilder(emailClientBuilder);
                }
            });
        }
    }
}