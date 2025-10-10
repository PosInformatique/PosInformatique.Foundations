//-----------------------------------------------------------------------
// <copyright file="FirstNameAttribute.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.People.DataAnnotations
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Validates that a string is a valid first name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class FirstNameAttribute : ValidationAttribute
    {
        private static readonly string AllowedSeparators = string.Join(", ", FirstName.AllowedSeparators.Select(s => $"'{s}'"));

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstNameAttribute"/> class.
        /// </summary>
        public FirstNameAttribute()
            : base(() => string.Format(PeopleDataAnnotationsResources.InvalidFirstName, AllowedSeparators))
        {
        }

        /// <inheritdoc />
        public override bool IsValid(object? value)
        {
            if (value is null)
            {
                return true;
            }

            if (value is not string firstName)
            {
                return true;
            }

            return FirstName.IsValid(firstName);
        }
    }
}
