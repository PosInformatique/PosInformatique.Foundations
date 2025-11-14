//-----------------------------------------------------------------------
// <copyright file="IEmailManager.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing
{
    /// <summary>
    /// Manager which allows to send e-mail using a <see cref="IEmailProvider"/>.
    /// </summary>
    public interface IEmailManager
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Email{TModel}"/>
        /// with the specified <paramref name="identifier"/>.
        /// The <see cref="EmailTemplate{TModel}"/> is retrieved from the <see cref="EmailingOptions"/>
        /// when calling the <see cref="EmailingOptions.RegisterTemplate{TModel}(EmailTemplateIdentifier{TModel}, EmailTemplate{TModel})"/>
        /// method.
        /// </summary>
        /// <typeparam name="TModel">Type of the data model to inject in the <see cref="Email{TModel}.Template"/>.</typeparam>
        /// <param name="identifier">Unique identifier of the <see cref="EmailTemplate{TModel}"/> which will be use
        /// to create the <see cref="Email{TModel}"/>.</param>
        /// <returns>A new instance of <see cref="Email{TModel}"/> which represents an e-mail based to the <see cref="EmailTemplate{TModel}"/>
        /// associated to the <paramref name="identifier"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="identifier"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if no <see cref="EmailTemplate{TModel}"/> has been registered with the specified <paramref name="identifier"/>.</exception>
        Email<TModel> Create<TModel>(EmailTemplateIdentifier<TModel> identifier)
            where TModel : EmailModel;

        /// <summary>
        /// Sends the specified <paramref name="email"/>.
        /// </summary>
        /// <typeparam name="TModel">Type of the data model to inject in the <see cref="Email{TModel}.Template"/>.</typeparam>
        /// <param name="email">The e-mail template with the recipients to send.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> which allows to cancel the send process.</param>
        /// <returns>An instance of the <see cref="Task"/> class which represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="email"/> argument is <see langword="null"/>.</exception>
        Task SendAsync<TModel>(Email<TModel> email, CancellationToken cancellationToken = default)
            where TModel : EmailModel;
    }
}