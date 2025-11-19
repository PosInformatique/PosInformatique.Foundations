//-----------------------------------------------------------------------
// <copyright file="GraphBuilderExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Graph.Tests
{
    using System.Reflection;
    using Azure.Core;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Graph;
    using Microsoft.Graph.Authentication;

    public class GraphBuilderExtensionsTest
    {
        [Theory]
        [InlineData(null, "https://graph.microsoft.com/v1.0")]
        [InlineData("https://the/url", "https://the/url")]
        public void UseGraph(string baseUrl, string expectedBaseUrl)
        {
            var serviceCollection = new ServiceCollection();
            var builder = new EmailingBuilder(serviceCollection);

            var credential = Mock.Of<TokenCredential>(MockBehavior.Strict);

            builder.UseGraph(credential, baseUrl)
                .Should().BeSameAs(builder);

            var sp = builder.Services.BuildServiceProvider();

            var provider = sp.GetRequiredService<IEmailProvider>();
            provider.Should().BeOfType<GraphEmailProvider>();

            var provider2 = sp.GetRequiredService<IEmailProvider>();
            provider2.Should().BeSameAs(provider);

            var graphServiceClient = GetFieldValue<GraphServiceClient>(provider, "serviceClient");
            graphServiceClient.RequestAdapter.As<BaseGraphRequestAdapter>().BaseUrl.Should().Be(expectedBaseUrl);

            GetFieldValue<GraphServiceClient>(provider2, "serviceClient").Should().BeSameAs(graphServiceClient);

            var graphServiceClientCredential = GetCredential(graphServiceClient);

            graphServiceClientCredential.Should().BeSameAs(credential);
        }

        [Fact]
        public void UseGraph_WithBuilderArgumentNull()
        {
            var act = () =>
            {
                GraphEmailingBuilderExtensions.UseGraph(null, default, default);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("builder");
        }

        [Fact]
        public void UseGraph_WithTokenCredentialArgumentNull()
        {
            var serviceCollection = new ServiceCollection();
            var builder = new EmailingBuilder(serviceCollection);

            var act = () =>
            {
                GraphEmailingBuilderExtensions.UseGraph(builder, null, default);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("tokenCredential");
        }

        private static TokenCredential GetCredential(GraphServiceClient serviceClient)
        {
            var requestAdapter = serviceClient.RequestAdapter.As<BaseGraphRequestAdapter>();
            var authenticationProvider = GetFieldValue<AzureIdentityAuthenticationProvider>(requestAdapter, "authProvider");

            return GetFieldValue<TokenCredential>(authenticationProvider.AccessTokenProvider, "_credential");
        }

        private static T GetFieldValue<T>(object obj, string name)
        {
            var currentType = obj.GetType();

            FieldInfo field;

            do
            {
                field = currentType
                    .GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);
                currentType = currentType.BaseType;
            }
            while (field is null);

            return (T)field.GetValue(obj)!;
        }
    }
}