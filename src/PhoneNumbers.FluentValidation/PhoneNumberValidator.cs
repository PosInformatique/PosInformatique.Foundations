//-----------------------------------------------------------------------
// <copyright file="PhoneNumberValidator.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace FluentValidation
{
    using FluentValidation.Validators;
    using PosInformatique.Foundations.PhoneNumbers;

    internal sealed class PhoneNumberValidator<T> : PropertyValidator<T, string>
    {
        public override string Name
        {
            get => "PhoneNumberValidator";
        }

        public override bool IsValid(ValidationContext<T> context, string value)
        {
            if (value is not null)
            {
                return PhoneNumber.IsValid(value);
            }

            return true;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
        {
            return $"'{{PropertyName}}' must be a valid phone number in E.164 format.";
        }
    }
}