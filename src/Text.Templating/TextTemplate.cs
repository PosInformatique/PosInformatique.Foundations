//-----------------------------------------------------------------------
// <copyright file="TextTemplate.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Text.Templating
{
    /// <summary>
    /// Base classe which represents a text template.
    /// </summary>
    /// <typeparam name="TModel">Type of data model to inject to the template to generate the final text.</typeparam>
    public abstract class TextTemplate<TModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextTemplate{TModel}"/> class.
        /// </summary>
        protected TextTemplate()
        {
        }

        /// <summary>
        /// Generates the text using the <paramref name="model"/> to the current template. The result
        /// of the generated text is obtained in the <paramref name="output"/> writer.
        /// </summary>
        /// <param name="model">Data model to inject to the template to generate the final text.</param>
        /// <param name="output"><see cref="TextWriter"/> which contains the generated text.</param>
        /// <param name="context"><see cref="ITextTemplateRenderContext"/> which allows to retrieve additional services for text generation.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> which allows to cancel the generation of text.</param>
        /// <returns>A <see cref="Task"/> instance which represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="model"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="output"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="context"/> argument is <see langword="null"/>.</exception>
        public abstract Task RenderAsync(TModel model, TextWriter output, ITextTemplateRenderContext context, CancellationToken cancellationToken = default);
    }
}