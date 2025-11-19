//-----------------------------------------------------------------------
// <copyright file="ITextTemplateRenderContext.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Text.Templating
{
    /// <summary>
    /// Represents a context used during the generation of text
    /// with a <see cref="TextTemplate{TModel}"/> when the <see cref="TextTemplate{TModel}.RenderAsync(TModel, TextWriter, ITextTemplateRenderContext, CancellationToken)"/>
    /// is called.
    /// </summary>
    public interface ITextTemplateRenderContext
    {
        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> which allows to retrieve additional services
        /// during the text generation.
        /// </summary>
        IServiceProvider ServiceProvider { get; }
    }
}