//-----------------------------------------------------------------------
// <copyright file="ScribanTextTemplate.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Text.Templating.Scriban
{
    using System.Dynamic;
    using System.IO;
    using global::Scriban;
    using global::Scriban.Runtime;

    /// <summary>
    /// Implementation of the <see cref="TextTemplate{TModel}"/> which generates text using a <see href="https://github.com/scriban/scriban">Scriban</see> as text template.
    /// </summary>
    /// <typeparam name="TModel">Type of the data model to inject to the <see href="https://github.com/scriban/scriban">Scriban</see> text template.</typeparam>
    public sealed class ScribanTextTemplate<TModel> : TextTemplate<TModel>
    {
        private readonly string content;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScribanTextTemplate{TModel}"/> class
        /// with the specified <see href="https://github.com/scriban/scriban">Scriban</see> text template <paramref name="content"/>.
        /// </summary>
        /// <param name="content"><see href="https://github.com/scriban/scriban">Scriban</see> text template to use.</param>
        public ScribanTextTemplate(string content)
        {
            ArgumentNullException.ThrowIfNull(content);

            this.content = content;
        }

        /// <inheritdoc />
        public override async Task RenderAsync(TModel model, TextWriter output, ITextTemplateRenderContext context, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(model);
            ArgumentNullException.ThrowIfNull(output);
            ArgumentNullException.ThrowIfNull(context);

            var scriptObject = new ScriptObject();

            if (model is ExpandoObject expandoData)
            {
                foreach (var property in (IDictionary<string, object?>)expandoData)
                {
                    scriptObject.Add(property.Key, property.Value);
                }
            }
            else
            {
                foreach (var property in model.GetType().GetProperties())
                {
                    scriptObject.Add(property.Name, property.GetValue(model));
                }
            }

            var scribanContext = new TemplateContext()
            {
                MemberRenamer = r => r.Name,
                MemberFilter = null,
            };

            scribanContext.PushGlobal(scriptObject);

            var scribanTemplate = Template.Parse(this.content);

            var text = await scribanTemplate.RenderAsync(scribanContext);

            await output.WriteAsync(text);
        }
    }
}