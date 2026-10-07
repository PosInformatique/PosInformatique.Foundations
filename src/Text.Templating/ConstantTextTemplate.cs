//-----------------------------------------------------------------------
// <copyright file="ConstantTextTemplate.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Text.Templating
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Implementation of the <see cref="TextTemplate{TModel}"/> which generates a constant text.
    /// </summary>
    /// <typeparam name="TModel">Type of data model to inject to the template to generate the final text. This type is not used in this implementation since the text is constant.</typeparam>
    public class ConstantTextTemplate<TModel> : TextTemplate<TModel>
    {
        private readonly string text;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantTextTemplate{TModel}"/> class
        /// with the specified constant <paramref name="text"/> to generate.
        /// </summary>
        /// <param name="text">The constant text to generate.</param>
        public ConstantTextTemplate(string text)
        {
            ArgumentNullException.ThrowIfNull(text);

            this.text = text;
        }

        /// <inheritdoc />
        [RequiresUnreferencedCode("Model is accessed by text template renderer and may require all members to be preserved.")]
        public override Task RenderAsync(TModel model, TextWriter output, ITextTemplateRenderContext context, CancellationToken cancellationToken = default)
        {
            return output.WriteAsync(this.text);
        }
    }
}