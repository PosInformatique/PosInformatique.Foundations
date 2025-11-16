//-----------------------------------------------------------------------
// <copyright file="RazorTextTemplateTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Text.Templating.Razor.Tests
{
    public class RazorTextTemplateTest
    {
        [Fact]
        public void Constructor_WithComponentTypeArgumentNull()
        {
            var act = () =>
            {
                new RazorTextTemplate<Model>(null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("componentType");
        }

        [Fact]
        public async Task RenderAsync()
        {
            var cancellationToken = new CancellationTokenSource().Token;

            var model = new Model();

            var renderer = new Mock<IRazorTextTemplateRenderer>(MockBehavior.Strict);
            renderer.Setup(r => r.RenderAsync(typeof(string), model, It.IsAny<TextWriter>(), cancellationToken))
                .Callback((Type _, object _, TextWriter writer, CancellationToken _) =>
                {
                    writer.Write("The output");
                })
                .Returns(Task.CompletedTask);

            var serviceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
            serviceProvider.Setup(sp => sp.GetService(typeof(IRazorTextTemplateRenderer)))
                .Returns(renderer.Object);

            var context = new Mock<ITextTemplateRenderContext>(MockBehavior.Strict);
            context.Setup(c => c.ServiceProvider)
                .Returns(serviceProvider.Object);

            var output = new StringWriter();

            var template = new RazorTextTemplate<Model>(typeof(string));

            await template.RenderAsync(model, output, context.Object, cancellationToken);

            output.ToString().Should().Be("The output");

            context.VerifyAll();
            renderer.VerifyAll();
            serviceProvider.VerifyAll();
        }

        [Fact]
        public async Task RenderAsync_WithModelArgumentNull()
        {
            var template = new RazorTextTemplate<Model>(typeof(string));

            await template.Invoking(t => t.RenderAsync(null, default, default, default))
                .Should().ThrowExactlyAsync<ArgumentNullException>()
                .WithParameterName("model");
        }

        [Fact]
        public async Task RenderAsync_WithOutputArgumentNull()
        {
            var model = new Model();

            var template = new RazorTextTemplate<Model>(typeof(string));

            await template.Invoking(t => t.RenderAsync(model, default, default, default))
                .Should().ThrowExactlyAsync<ArgumentNullException>()
                .WithParameterName("output");
        }

        [Fact]
        public async Task RenderAsync_WithContextArgumentNull()
        {
            var model = new Model();
            var output = new StringWriter();

            var template = new RazorTextTemplate<Model>(typeof(string));

            await template.Invoking(t => t.RenderAsync(model, output, null, default))
                .Should().ThrowExactlyAsync<ArgumentNullException>()
                .WithParameterName("context");
        }

        private sealed class Model
        {
        }
    }
}