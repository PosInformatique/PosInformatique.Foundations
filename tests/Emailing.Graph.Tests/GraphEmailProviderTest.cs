//-----------------------------------------------------------------------
// <copyright file="GraphEmailProviderTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Graph.Tests
{
    using Microsoft.Graph;
    using Microsoft.Graph.Models;
    using Microsoft.Graph.Users.Item.SendMail;
    using Microsoft.Kiota.Abstractions;
    using Microsoft.Kiota.Abstractions.Serialization;
    using Microsoft.Kiota.Serialization.Json;

    public class GraphEmailProviderTest
    {
        [Fact]
        public void Constructor_WithServiceClientArgumentNull()
        {
            var act = () =>
            {
                new GraphEmailProvider(null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("serviceClient");
        }

        [Fact]
        public async Task SendAsync()
        {
            var cancellationToken = new CancellationTokenSource().Token;

            var serializationWriterFactory = new Mock<ISerializationWriterFactory>(MockBehavior.Strict);
            serializationWriterFactory.Setup(f => f.GetSerializationWriter("application/json"))
                .Returns(new JsonSerializationWriter());

            var requestAdapter = new Mock<IRequestAdapter>(MockBehavior.Strict);
            requestAdapter.Setup(r => r.BaseUrl)
                .Returns("http://base/url");
            requestAdapter.Setup(r => r.EnableBackingStore(null));
            requestAdapter.Setup(r => r.SerializationWriterFactory)
                .Returns(serializationWriterFactory.Object);
            requestAdapter.Setup(r => r.SendNoContentAsync(It.IsAny<RequestInformation>(), It.IsNotNull<Dictionary<string, ParsableFactory<IParsable>>>(), cancellationToken))
                .Callback((RequestInformation requestInfo, Dictionary<string, ParsableFactory<IParsable>> _, CancellationToken _) =>
                {
                    requestInfo.HttpMethod.Should().Be(Method.POST);
                    requestInfo.URI.Should().Be("http://base/url/users/sender%40domain.com/sendMail");

                    var jsonMessage = KiotaJsonSerializer.DeserializeAsync<SendMailPostRequestBody>(requestInfo.Content).GetAwaiter().GetResult();

                    jsonMessage.Message.Attachments.Should().BeNull();
                    jsonMessage.Message.Body.Content.Should().Be("The HTML content");
                    jsonMessage.Message.Body.ContentType.Should().Be(BodyType.Html);
                    jsonMessage.Message.BccRecipients.Should().BeNull();
                    jsonMessage.Message.CcRecipients.Should().BeNull();
                    jsonMessage.Message.ToRecipients.Should().HaveCount(1);
                    jsonMessage.Message.ToRecipients[0].EmailAddress.Address.Should().Be("recipient@domain.com");
                    jsonMessage.Message.ToRecipients[0].EmailAddress.Name.Should().Be("The recipient");
                    jsonMessage.SaveToSentItems.Should().BeFalse();
                })
                .Returns(Task.CompletedTask);

            var graphServiceClient = new Mock<GraphServiceClient>(MockBehavior.Strict, requestAdapter.Object, null);

            var client = new GraphEmailProvider(graphServiceClient.Object);

            var from = new EmailContact(EmailAddresses.EmailAddress.Parse("sender@domain.com"), "The sender");
            var to = new EmailContact(EmailAddresses.EmailAddress.Parse("recipient@domain.com"), "The recipient");

            var message = new EmailMessage(from, to, "The subject", "The HTML content");

            await client.SendAsync(message, cancellationToken);

            graphServiceClient.VerifyAll();
            requestAdapter.VerifyAll();
            serializationWriterFactory.VerifyAll();
        }

        [Fact]
        public async Task SendSync_WithMessageArgumentNull()
        {
            var requestAdapter = new Mock<IRequestAdapter>(MockBehavior.Strict);
            requestAdapter.Setup(r => r.BaseUrl)
                .Returns("http://base/url");
            requestAdapter.Setup(r => r.EnableBackingStore(null));

            var serviceClient = new Mock<GraphServiceClient>(MockBehavior.Strict, requestAdapter.Object, null);

            var provider = new GraphEmailProvider(serviceClient.Object);

            await provider.Invoking(p => p.SendAsync(null, default))
                .Should().ThrowExactlyAsync<ArgumentNullException>()
                .WithParameterName("message");

            requestAdapter.VerifyAll();
            serviceClient.VerifyAll();
        }
    }
}