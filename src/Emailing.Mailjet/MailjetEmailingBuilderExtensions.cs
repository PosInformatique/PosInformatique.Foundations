//-----------------------------------------------------------------------
// <copyright file="MailjetEmailingBuilderExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.Extensions.DependencyInjection
{
    using Mailjet.Client;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using PosInformatique.Foundations.Emailing;
    using PosInformatique.Foundations.Emailing.Mailjet;

    /// <summary>
    /// Extension methods to configure the <c>Mailjet</c> provider
    /// for the <see cref="IEmailManager"/>.
    /// </summary>
    public static class MailjetEmailingBuilderExtensions
    {
        private static readonly string HttpClientName = typeof(MailjetClient).Namespace!;

        /// <summary>
        /// Configure the provider of <see cref="IEmailManager"/> to use <c>Mailjet</c>.
        /// </summary>
        /// <param name="builder"><see cref="EmailingBuilder"/> which to configure.</param>
        /// <param name="apiKey">API key of the <c>Mailjet</c> account.</param>
        /// <param name="apiSecret">API secret of the <c>Mailjet</c> account.</param>
        /// <returns>The <paramref name="builder"/> instance to continue the configuration of the emailing feature.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="builder"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="apiKey"/> argument is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="apiSecret"/> argument is <see langword="null"/> or empty.</exception>
        public static EmailingBuilder UseMailjet(this EmailingBuilder builder, string apiKey, string apiSecret)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);
            ArgumentException.ThrowIfNullOrWhiteSpace(apiSecret);

            builder.Services.AddHttpClient(HttpClientName, httpClient =>
            {
                httpClient.SetDefaultSettings();
                httpClient.UseBasicAuthentication(apiKey, apiSecret);
            });

            builder.Services.TryAddSingleton<IEmailProvider>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient(HttpClientName);

                var mailjetClient = new MailjetClient(httpClient);

                return new MailjetEmailProvider(mailjetClient);
            });

            return builder;
        }
    }
}