//-----------------------------------------------------------------------
// <copyright file="LastNameValidator.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.People
{
    using FluentValidation;
    using FluentValidation.Validators;

    internal sealed class LastNameValidator<T> : PropertyValidator<T, string>
    {
        private static readonly string AllowedSeparators = string.Join(", ", LastName.AllowedSeparators.Select(s => $"'{s}'"));

        public override string Name
        {
            get => "LastNameValidator";
        }

        public override bool IsValid(ValidationContext<T> context, string value)
        {
            if (value is not null)
            {
                return LastName.IsValid(value);
            }

            return false;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
        {
            return $"'{{PropertyName}}' must contain a last name that consists only of alphabetic characters, with the [{AllowedSeparators}] separators, and is less than {LastName.MaxLength} characters long.";
        }
    }
}
