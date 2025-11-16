//-----------------------------------------------------------------------
// <copyright file="AzureEmailingBuilderExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Azure.Tests
{
    using System.Runtime.CompilerServices;
    using global::Azure.Communication.Email;
    using Microsoft.Extensions.DependencyInjection;

    public class AzureEmailingBuilderExtensionsTest
    {
        [Fact]
        public void UseAzureCommunicationService_WithConnectionString()
        {
            var serviceCollection = new ServiceCollection();
            var builder = new EmailingBuilder(serviceCollection);

            builder.UseAzureCommunicationService("endpoint=https://my-acs-resource.communication.azure.com/;accesskey=2x3Yz==")
                .Should().BeSameAs(builder);

            var sp = builder.Services.BuildServiceProvider();

            var provider = sp.GetRequiredService<IEmailProvider>();
            provider.Should().BeOfType<AzureEmailProvider>();

            sp.GetRequiredService<IEmailProvider>().Should().BeSameAs(provider);

            var azureClient = sp.GetRequiredService<EmailClient>();
            var client = AzureEmailProviderAccessor.GetClientField((AzureEmailProvider)provider);

            client.Should().BeSameAs(azureClient);
        }

        [Fact]
        public void UseAzureCommunicationService_WithConnectionString_WithClientBuilder()
        {
            var serviceCollection = new ServiceCollection();
            var builder = new EmailingBuilder(serviceCollection);

            var clientBuilderCalled = false;

            builder.UseAzureCommunicationService("endpoint=https://my-acs-resource.communication.azure.com/;accesskey=2x3Yz==", clientBuilder =>
            {
                clientBuilderCalled = true;
            })
            .Should().BeSameAs(builder);

            var sp = builder.Services.BuildServiceProvider();

            var provider = sp.GetRequiredService<IEmailProvider>();
            provider.Should().BeOfType<AzureEmailProvider>();

            sp.GetRequiredService<IEmailProvider>().Should().BeSameAs(provider);

            clientBuilderCalled.Should().BeTrue();

            var azureClient = sp.GetRequiredService<EmailClient>();
            var client = AzureEmailProviderAccessor.GetClientField((AzureEmailProvider)provider);

            client.Should().BeSameAs(azureClient);
        }

        [Fact]
        public void UseAzureCommunicationService_WithConnectionString_WithNullBuilder()
        {
            var act = () =>
            {
                AzureEmailingBuilderExtensions.UseAzureCommunicationService(null, (string)default);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("builder");
        }

        [Fact]
        public void UseAzureCommunicationService_WithConnectionString_WithNullConnectionString()
        {
            var builder = new EmailingBuilder(Mock.Of<IServiceCollection>(MockBehavior.Strict));

            var act = () =>
            {
                AzureEmailingBuilderExtensions.UseAzureCommunicationService(builder, (string)null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("connectionString");
        }

        [Fact]
        public void UseAzureCommunicationService_WithUri()
        {
            var serviceCollection = new ServiceCollection();
            var builder = new EmailingBuilder(serviceCollection);

            builder.UseAzureCommunicationService(new Uri("https://my-acs-resource.communication.azure.com/"))
                .Should().BeSameAs(builder);

            var sp = builder.Services.BuildServiceProvider();

            var provider = sp.GetRequiredService<IEmailProvider>();
            provider.Should().BeOfType<AzureEmailProvider>();

            sp.GetRequiredService<IEmailProvider>().Should().BeSameAs(provider);

            var azureClient = sp.GetRequiredService<EmailClient>();
            var client = AzureEmailProviderAccessor.GetClientField((AzureEmailProvider)provider);

            client.Should().BeSameAs(azureClient);
        }

        [Fact]
        public void UseAzureCommunicationService_WithUri_WithClientBuilder()
        {
            var serviceCollection = new ServiceCollection();
            var builder = new EmailingBuilder(serviceCollection);

            var clientBuilderCalled = false;

            builder.UseAzureCommunicationService(new Uri("https://my-acs-resource.communication.azure.com/"), clientBuilder =>
            {
                clientBuilderCalled = true;
            })
            .Should().BeSameAs(builder);

            var sp = builder.Services.BuildServiceProvider();

            var provider = sp.GetRequiredService<IEmailProvider>();
            provider.Should().BeOfType<AzureEmailProvider>();

            sp.GetRequiredService<IEmailProvider>().Should().BeSameAs(provider);

            clientBuilderCalled.Should().BeTrue();

            var azureClient = sp.GetRequiredService<EmailClient>();
            var client = AzureEmailProviderAccessor.GetClientField((AzureEmailProvider)provider);

            client.Should().BeSameAs(azureClient);
        }

        [Fact]
        public void UseAzureCommunicationService_WithUri_WithNullBuilder()
        {
            var act = () =>
            {
                AzureEmailingBuilderExtensions.UseAzureCommunicationService(null, (Uri)default);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("builder");
        }

        [Fact]
        public void UseAzureCommunicationService_WithUri_WithNullUri()
        {
            var builder = new EmailingBuilder(Mock.Of<IServiceCollection>(MockBehavior.Strict));

            var act = () =>
            {
                AzureEmailingBuilderExtensions.UseAzureCommunicationService(builder, (Uri)null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("uri");
        }

        public static class AzureEmailProviderAccessor
        {
            [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "client")]
            public static extern ref EmailClient GetClientField(AzureEmailProvider instance);
        }
    }
}