//-----------------------------------------------------------------------
// <copyright file="IRazorTextTemplateRenderer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Text.Templating.Razor
{
    /// <summary>
    /// Used internaly by the <see cref="RazorTextTemplate{TModel}"/> to render a Razor component to a text output.
    /// </summary>
    internal interface IRazorTextTemplateRenderer
    {
        /// <summary>
        /// Generates the text output of a Razor component.
        /// </summary>
        /// <param name="componentType">Type of the Razor component which will be use to generate the text.</param>
        /// <param name="model">Model to inject in the Razor component.</param>
        /// <param name="output">Output where the text will be generated.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> used to cancel the generation process.</param>
        /// <returns>An instance of the <see cref="Task"/> which represents the asynchronous operation.</returns>
        Task RenderAsync(Type componentType, object? model, TextWriter output, CancellationToken cancellationToken = default);
    }
}