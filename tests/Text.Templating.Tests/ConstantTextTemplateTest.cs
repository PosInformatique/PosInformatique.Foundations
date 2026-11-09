//-----------------------------------------------------------------------
// <copyright file="ConstantTextTemplateTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Text.Templating.Tests
{
    public class ConstantTextTemplateTest
    {
        [Fact]
        public void Constructor_WithTextArgumentNull()
        {
            var act = () =>
            {
                new ConstantTextTemplate<Model>(null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("text");
        }

        [Fact]
        public async Task RenderAsync()
        {
            var cancellationToken = new CancellationTokenSource().Token;

            var model = new Model();

            var context = Mock.Of<ITextTemplateRenderContext>(MockBehavior.Strict);

            using var output = new StringWriter();

            var template = new ConstantTextTemplate<Model>("The constant text");

            await template.RenderAsync(model, output, context, cancellationToken);

            output.ToString().Should().Be("The constant text");
        }

        [Fact]
        public async Task RenderAsync_WithEmptyText()
        {
            var cancellationToken = new CancellationTokenSource().Token;

            var model = new Model();

            var context = Mock.Of<ITextTemplateRenderContext>(MockBehavior.Strict);

            using var output = new StringWriter();

            var template = new ConstantTextTemplate<Model>(string.Empty);

            await template.RenderAsync(model, output, context, cancellationToken);

            output.ToString().Should().Be(string.Empty);
        }

        [Fact]
        public async Task RenderAsync_WithModelNull()
        {
            var cancellationToken = new CancellationTokenSource().Token;

            var context = Mock.Of<ITextTemplateRenderContext>(MockBehavior.Strict);

            using var output = new StringWriter();

            var template = new ConstantTextTemplate<Model>("The constant text");

            await template.RenderAsync(null, output, context, cancellationToken);

            output.ToString().Should().Be("The constant text");
        }

        private sealed class Model
        {
        }
    }
}