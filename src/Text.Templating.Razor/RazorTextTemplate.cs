//-----------------------------------------------------------------------
// <copyright file="RazorTextTemplate.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Text.Templating.Razor
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Implementation of the <see cref="TextTemplate{TModel}"/> which generates using a Razor component.
    /// </summary>
    /// <typeparam name="TModel">Type of the data model to inject to the Razor component.</typeparam>
    public class RazorTextTemplate<TModel> : TextTemplate<TModel>
    {
        private readonly Type componentType;

        /// <summary>
        /// Initializes a new instance of the <see cref="RazorTextTemplate{TModel}"/> class.
        /// </summary>
        /// <param name="componentType">Type of the Razor component which will be use to generate the text.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="componentType"/> argument is <see langword="null"/>.</exception>
        public RazorTextTemplate(Type componentType)
        {
            ArgumentNullException.ThrowIfNull(componentType);

            this.componentType = componentType;
        }

        /// <inheritdoc />
        public override async Task RenderAsync(TModel model, TextWriter output, ITextTemplateRenderContext context, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(model);
            ArgumentNullException.ThrowIfNull(output);
            ArgumentNullException.ThrowIfNull(context);

            var razorRenderer = context.ServiceProvider.GetRequiredService<IRazorTextTemplateRenderer>();

            await razorRenderer.RenderAsync(this.componentType, model, output, cancellationToken);
        }
    }
}