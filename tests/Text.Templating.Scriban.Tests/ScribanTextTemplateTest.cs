//-----------------------------------------------------------------------
// <copyright file="ScribanTextTemplateTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Text.Templating.Scriban.Tests
{
    using System.Dynamic;
    using PosInformatique.Foundations.People;

    public class ScribanTextTemplateTest
    {
        [Fact]
        public async Task RenderAsync()
        {
            var cancellationToken = new CancellationTokenSource().Token;

            var data = new
            {
                FirstName = FirstName.Create("Gilles"),
                LastName = LastName.Create("TOURREAU"),
                Subject = "The subject",
                InnerObject = new
                {
                    Age = 1234,
                },
            };

            var context = Mock.Of<ITextTemplateRenderContext>(MockBehavior.Strict);

            using var output = new StringWriter();

            var textTemplating = new ScribanTextTemplate<object>("FirstName='{{FirstName}}', LastName='{{LastName}}', Age={{InnerObject.Age}}, Subject={{Subject}}");

            await textTemplating.RenderAsync(data, output, context, cancellationToken);

            output.ToString().Should().Be("FirstName='Gilles', LastName='TOURREAU', Age=1234, Subject=The subject");
        }

        [Fact]
        public async Task RenderAsync_UsingExpando()
        {
            var cancellationToken = new CancellationTokenSource().Token;

            var data = new ExpandoObject();

            data.As<IDictionary<string, object>>()["FirstName"] = FirstName.Create("Gilles");
            data.As<IDictionary<string, object>>()["LastName"] = LastName.Create("TOURREAU");
            data.As<IDictionary<string, object>>()["InnerObject"] = new { Age = 1234 };
            data.As<IDictionary<string, object>>()["Subject"] = "The subject";

            var context = Mock.Of<ITextTemplateRenderContext>(MockBehavior.Strict);

            using var output = new StringWriter();

            var textTemplating = new ScribanTextTemplate<object>("FirstName='{{FirstName}}', LastName='{{LastName}}', Age={{InnerObject.Age}}, Subject={{Subject}}");

            await textTemplating.RenderAsync(data, output, context, cancellationToken);

            output.ToString().Should().Be("FirstName='Gilles', LastName='TOURREAU', Age=1234, Subject=The subject");
        }

        [Fact]
        public async Task RenderAsync_WithModelNullArgument()
        {
            var textTemplating = new ScribanTextTemplate<object>("FirstName='{{FirstName}}', LastName='{{LastName}}', Age={{InnerObject.Age}}, Subject={{Subject}}");

            await textTemplating.Invoking(r => r.RenderAsync(null, default, default, default))
                .Should().ThrowExactlyAsync<ArgumentNullException>()
                .WithParameterName("model");
        }

        [Fact]
        public async Task RenderAsync_WithOutputNullArgument()
        {
            var textTemplating = new ScribanTextTemplate<object>("FirstName='{{FirstName}}', LastName='{{LastName}}', Age={{InnerObject.Age}}, Subject={{Subject}}");

            await textTemplating.Invoking(r => r.RenderAsync(new object(), null, default, default))
                .Should().ThrowExactlyAsync<ArgumentNullException>()
                .WithParameterName("output");
        }

        [Fact]
        public async Task RenderAsync_WithContextNullArgument()
        {
            var textTemplating = new ScribanTextTemplate<object>("FirstName='{{FirstName}}', LastName='{{LastName}}', Age={{InnerObject.Age}}, Subject={{Subject}}");

            await textTemplating.Invoking(r => r.RenderAsync(new object(), new StringWriter(), null, default))
                .Should().ThrowExactlyAsync<ArgumentNullException>()
                .WithParameterName("context");
        }
    }
}