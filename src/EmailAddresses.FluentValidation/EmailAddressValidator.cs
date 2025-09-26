//-----------------------------------------------------------------------
// <copyright file="EmailAddressValidator.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace FluentValidation
{
    using FluentValidation.Validators;
    using PosInformatique.Foundations.EmailAddresses;

    internal sealed class EmailAddressValidator<T> : PropertyValidator<T, string>
    {
        public override string Name
        {
            get => "EmailAddressValidator";
        }

        public override bool IsValid(ValidationContext<T> context, string value)
        {
            if (value is not null)
            {
                return EmailAddress.IsValid(value);
            }

            return true;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
        {
            return $"'{{PropertyName}}' must be a valid email address.";
        }
    }
}
