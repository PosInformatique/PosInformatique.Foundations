//-----------------------------------------------------------------------
// <copyright file="FirstNameValidator.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.People
{
    using FluentValidation;
    using FluentValidation.Validators;

    internal sealed class FirstNameValidator<T> : PropertyValidator<T, string>
    {
        private static readonly string AllowedSeparators = string.Join(", ", FirstName.AllowedSeparators.Select(s => $"'{s}'"));

        public override string Name
        {
            get => "FirstNameValidator";
        }

        public override bool IsValid(ValidationContext<T> context, string value)
        {
            if (value is not null)
            {
                return FirstName.IsValid(value);
            }

            return false;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
        {
            return $"'{{PropertyName}}' must contain a first name that consists only of alphabetic characters, with the [{AllowedSeparators}] separators, and is less than {FirstName.MaxLength} characters long.";
        }
    }
}
