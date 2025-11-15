//-----------------------------------------------------------------------
// <copyright file="RazorEmailTemplateSubject.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Templates.Razor
{
    using Microsoft.AspNetCore.Components;

    /// <summary>
    /// Base class of a Razor component which is used to generate the subject of an email.
    /// </summary>
    /// <typeparam name="TModel">Type of the <see cref="Model"/> used to generate the subject of the e-mail.</typeparam>
    public abstract class RazorEmailTemplateSubject<TModel> : ComponentBase
        where TModel : EmailModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RazorEmailTemplateSubject{TModel}"/> class.
        /// </summary>
        protected RazorEmailTemplateSubject()
        {
        }

        /// <summary>
        /// Gets or sets the model used to generate the body of the e-mail.
        /// </summary>
        [Parameter]
        public TModel Model { get; set; } = default!;
    }
}