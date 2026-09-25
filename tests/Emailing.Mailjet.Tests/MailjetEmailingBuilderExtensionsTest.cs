//-----------------------------------------------------------------------
// <copyright file="MailjetEmailingBuilderExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Mailjet.Tests
{
    using System.Net.Http.Headers;
    using System.Text;
    using Microsoft.Extensions.DependencyInjection;

    public class MailjetEmailingBuilderExtensionsTest
    {
        [Fact]
        public void UseMailjet()
        {
            var serviceCollection = new ServiceCollection();
            var builder = new EmailingBuilder(serviceCollection);

            builder.UseMailjet("The API key", "The API secret")
                .Should().BeSameAs(builder);

            var sp = builder.Services.BuildServiceProvider();

            var provider = sp.GetRequiredService<IEmailProvider>();
            provider.Should().BeOfType<MailjetEmailProvider>();

            sp.GetRequiredService<IEmailProvider>().Should().BeSameAs(provider);

            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient(typeof(global::Mailjet.Client.MailjetClient).Namespace!);

            httpClient.DefaultRequestHeaders.Authorization.Should().BeEquivalentTo(
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("The API key:The API secret"))));
        }

        [Fact]
        public void UseMailjet_WithNullBuilder()
        {
            var act = () =>
            {
                MailjetEmailingBuilderExtensions.UseMailjet(null, "The API key", "The API secret");
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("builder");
        }

        [Fact]
        public void UseMailjet_WithNullApiKey()
        {
            var builder = new EmailingBuilder(Mock.Of<IServiceCollection>(MockBehavior.Strict));

            var act = () =>
            {
                MailjetEmailingBuilderExtensions.UseMailjet(builder, null, "The API secret");
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("apiKey");
        }

        [Fact]
        public void UseMailjet_WithEmptyApiKey()
        {
            var builder = new EmailingBuilder(Mock.Of<IServiceCollection>(MockBehavior.Strict));

            var act = () =>
            {
                MailjetEmailingBuilderExtensions.UseMailjet(builder, string.Empty, "The API secret");
            };

            act.Should().ThrowExactly<ArgumentException>()
                .WithParameterName("apiKey");
        }

        [Fact]
        public void UseMailjet_WithNullApiSecret()
        {
            var builder = new EmailingBuilder(Mock.Of<IServiceCollection>(MockBehavior.Strict));

            var act = () =>
            {
                MailjetEmailingBuilderExtensions.UseMailjet(builder, "The API key", null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("apiSecret");
        }

        [Fact]
        public void UseMailjet_WithEmptyApiSecret()
        {
            var builder = new EmailingBuilder(Mock.Of<IServiceCollection>(MockBehavior.Strict));

            var act = () =>
            {
                MailjetEmailingBuilderExtensions.UseMailjet(builder, "The API key", string.Empty);
            };

            act.Should().ThrowExactly<ArgumentException>()
                .WithParameterName("apiSecret");
        }
    }
}