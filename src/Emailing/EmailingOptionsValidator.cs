//-----------------------------------------------------------------------
// <copyright file="EmailingOptionsValidator.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Emailing
{
    using Microsoft.Extensions.Options;

    internal sealed class EmailingOptionsValidator : IValidateOptions<EmailingOptions>
    {
        public ValidateOptionsResult Validate(string? name, EmailingOptions options)
        {
            if (options.SenderEmailAddress is null)
            {
                return ValidateOptionsResult.Fail($"The {nameof(EmailingOptions.SenderEmailAddress)} property must be provided when configuring the emailing feature with {nameof(EmailingOptions)}.");
            }

            return ValidateOptionsResult.Success;
        }
    }
}