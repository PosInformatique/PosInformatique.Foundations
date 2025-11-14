//-----------------------------------------------------------------------
// <copyright file="RazorTextTemplateRenderer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Text.Templating.Razor
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Web;
    using Microsoft.Extensions.Logging;

    internal sealed class RazorTextTemplateRenderer : IRazorTextTemplateRenderer
    {
        private readonly IServiceProvider serviceProvider;

        private readonly ILoggerFactory loggerFactory;

        public RazorTextTemplateRenderer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            this.serviceProvider = serviceProvider;
            this.loggerFactory = loggerFactory;
        }

        public async Task RenderAsync(Type componentType, object? model, TextWriter output, CancellationToken cancellationToken = default)
        {
            await using var htmlRenderer = new HtmlRenderer(this.serviceProvider, this.loggerFactory);

            await htmlRenderer.Dispatcher.InvokeAsync(async () =>
            {
                var values = new Dictionary<string, object?>
                {
                    { "Model", model },
                };

                var parameters = ParameterView.FromDictionary(values);
                var result = await htmlRenderer.RenderComponentAsync(componentType, parameters);

                result.WriteHtmlTo(output);
            });
        }
    }
}