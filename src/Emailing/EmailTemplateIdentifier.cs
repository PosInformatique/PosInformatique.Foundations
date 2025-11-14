//-----------------------------------------------------------------------
// <copyright file="EmailTemplateIdentifier.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing
{
    /// <summary>
    /// Represents an unique <see cref="EmailTemplate{TModel}"/> identifier.
    /// </summary>
    /// <typeparam name="TModel">Data model injected to the <see cref="EmailTemplate{TModel}"/> to generate the e-mail.</typeparam>
    public sealed class EmailTemplateIdentifier<TModel>
        where TModel : EmailModel
    {
        private EmailTemplateIdentifier()
        {
        }

        /// <summary>
        /// Creates a new <see cref="EmailTemplate{TModel}"/> identifier.
        /// </summary>
        /// <returns>A new <see cref="EmailTemplate{TModel}"/> identifier.</returns>
        public static EmailTemplateIdentifier<TModel> Create()
        {
            return new EmailTemplateIdentifier<TModel>();
        }
    }
}