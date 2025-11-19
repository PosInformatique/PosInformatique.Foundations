//-----------------------------------------------------------------------
// <copyright file="EmailingServiceCollectionExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.Extensions.DependencyInjection.Tests
{
    using Microsoft.Extensions.Options;
    using PosInformatique.Foundations.EmailAddresses;
    using PosInformatique.Foundations.Emailing;

    public class EmailingServiceCollectionExtensionsTest
    {
        [Fact]
        public void AddEmailing()
        {
            var provider = Mock.Of<IEmailProvider>(MockBehavior.Strict);

            var services = new ServiceCollection();
            services.AddSingleton(provider);

            EmailingServiceCollectionExtensions.AddEmailing(
                services,
                opt =>
                {
                    opt.SenderEmailAddress = EmailAddress.Parse("sender@domain.com");
                })
                .Services.Should().BeSameAs(services);

            var sp = services.BuildServiceProvider();

            var manager = sp.GetRequiredService<IEmailManager>();

            manager.Should().BeOfType<EmailManager>();
            sp.GetRequiredService<IEmailManager>().Should().BeSameAs(manager);

            var options = sp.GetRequiredService<IOptions<EmailingOptions>>();
            options.Value.SenderEmailAddress.Should().Be(EmailAddress.Parse("sender@domain.com"));
        }

        [Fact]
        public void AddEmailing_WithNullServices()
        {
            var act = () =>
            {
                EmailingServiceCollectionExtensions.AddEmailing(null, default);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("services");
        }

        [Fact]
        public void AddEmailing_WithOptions()
        {
            var act = () =>
            {
                EmailingServiceCollectionExtensions.AddEmailing(Mock.Of<IServiceCollection>(MockBehavior.Strict), default);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("options");
        }
    }
}