//-----------------------------------------------------------------------
// <copyright file="LastNameAttribute.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.People.DataAnnotations
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Validates that a string is a valid last name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class LastNameAttribute : ValidationAttribute
    {
        private static readonly string AllowedSeparators = string.Join(", ", LastName.AllowedSeparators.Select(s => $"'{s}'"));

        /// <summary>
        /// Initializes a new instance of the <see cref="LastNameAttribute"/> class.
        /// </summary>
        public LastNameAttribute()
            : base(() => string.Format(PeopleDataAnnotationsResources.InvalidLastName, AllowedSeparators))
        {
        }

        /// <inheritdoc />
        public override bool IsValid(object? value)
        {
            if (value is null)
            {
                return true;
            }

            if (value is not string lastName)
            {
                return true;
            }

            return LastName.IsValid(lastName);
        }
    }
}
