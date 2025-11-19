//-----------------------------------------------------------------------
// <copyright file="RazorEmailTemplate.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing.Templates.Razor
{
    using PosInformatique.Foundations.Text.Templating.Razor;

    /// <summary>
    /// Used to create an <see cref="EmailTemplate{TModel}"/> using Razor text templating for the subject and body.
    /// </summary>
    /// <typeparam name="TModel">Type of the model used to generate the subject and body of the e-mail.</typeparam>
    public static class RazorEmailTemplate<TModel>
    {
        /// <summary>
        /// Creates an <see cref="EmailTemplate{TModel}"/> using the specified Razor components as text templating for the subject and body.
        /// </summary>
        /// <typeparam name="TSubjectComponent">Type of the Razor component used to generate the content of the e-mail subject.</typeparam>
        /// <typeparam name="TBodyComponent">Type of the Razor component used to generate the content of the e-mail body.</typeparam>
        /// <returns>An instance <see cref="EmailTemplate{TModel}"/> using the specified Razor components as text templating for the subject and body.</returns>
        public static EmailTemplate<TModel> Create<TSubjectComponent, TBodyComponent>()
            where TSubjectComponent : RazorEmailTemplateSubject<TModel>
            where TBodyComponent : RazorEmailTemplateBody<TModel>
        {
            return new EmailTemplate<TModel>(
                new RazorTextTemplate<TModel>(typeof(TSubjectComponent)),
                new RazorTextTemplate<TModel>(typeof(TBodyComponent)));
        }
    }
}