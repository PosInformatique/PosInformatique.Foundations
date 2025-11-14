//-----------------------------------------------------------------------
// <copyright file="IRazorTextTemplateRenderer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Text.Templating.Razor
{
    internal interface IRazorTextTemplateRenderer
    {
        Task RenderAsync(Type componentType, object? model, TextWriter output, CancellationToken cancellationToken = default);
    }
}