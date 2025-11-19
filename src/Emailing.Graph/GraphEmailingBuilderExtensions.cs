//-----------------------------------------------------------------------
// <copyright file="GraphEmailingBuilderExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.Extensions.DependencyInjection
{
    using Azure.Core;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Graph;
    using PosInformatique.Foundations.Emailing;
    using PosInformatique.Foundations.Emailing.Graph;

    /// <summary>
    /// Extension methods to configure the <c>Azure Communication Service</c> provider
    /// for the <see cref="IEmailManager"/>.
    /// </summary>
    public static class GraphEmailingBuilderExtensions
    {
        /// <summary>
        /// Configure the provider of <see cref="IEmailManager"/> to use <c>Azure Communication Service</c>.
        /// </summary>
        /// <param name="builder"><see cref="EmailingBuilder"/> which to configure.</param>
        /// <param name="tokenCredential">The <see cref="TokenCredential"/> for authenticating to Microsoft Graph API.</param>
        /// <param name="baseUrl">The base service URL of the API Graph. If not specified the <c>https://graph.microsoft.com/v1.0</c> will be use.</param>
        /// <returns>The <paramref name="builder"/> instance to continue the configuration of the emailing feature.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="builder"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="tokenCredential"/> argument is <see langword="null"/>.</exception>
        public static EmailingBuilder UseGraph(this EmailingBuilder builder, TokenCredential tokenCredential, string? baseUrl = null)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(tokenCredential);

            builder.Services.TryAddSingleton<IEmailProvider>(sp =>
            {
                var serviceClient = new GraphServiceClient(tokenCredential, null, baseUrl);

                return new GraphEmailProvider(serviceClient);
            });

            return builder;
        }
    }
}