//-----------------------------------------------------------------------
// <copyright file="NameValidatorExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace FluentValidation
{
    using PosInformatique.Foundations.People;

    /// <summary>
    /// Contains extension methods for <c>FluentValidation</c> to validate first name and last name
    /// to check the business rules of the <see cref="FirstName"/> and <see cref="LastName"/>.
    /// </summary>
    public static class NameValidatorExtensions
    {
        /// <summary>
        /// Defines a validator that checks if a <see cref="string"/> property is a valid first name
        /// (parsable by the <see cref="FirstName"/> value object).
        /// Validation fails if the value is not a valid first name according to the business rules:
        /// letters only with separators (' ' or '-'), proper casing, no consecutive/trailing separators, and max length of 50.
        /// If the <see cref="string"/> value is <see langword="null"/>, validation succeeds.
        /// Use the <see cref="DefaultValidatorExtensions.NotNull{T, TProperty}(IRuleBuilder{T, TProperty})"/> validator
        /// to disallow <see langword="null"/> values.
        /// </summary>
        /// <typeparam name="T">The type of the object being validated.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator is defined.</param>
        /// <returns>The <paramref name="ruleBuilder"/> instance to continue configuring the property validator.</returns>
        /// <exception cref="ArgumentNullException">If the specified <paramref name="ruleBuilder"/> argument is <see langword="null"/>.</exception>
        public static IRuleBuilderOptions<T, string> MustBeFirstName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder, nameof(ruleBuilder));

            return ruleBuilder.SetValidator(new FirstNameValidator<T>());
        }

        /// <summary>
        /// Defines a validator that checks if a <see cref="string"/> property is a valid last name
        /// (parsable by the <see cref="LastName"/> value object).
        /// Validation fails if the value is not a valid last name according to the business rules:
        /// letters only with separators (' ' or '-'), fully uppercased normalization, no consecutive/trailing separators, and max length of 50.
        /// If the <see cref="string"/> value is <see langword="null"/>, validation succeeds.
        /// Use the <see cref="DefaultValidatorExtensions.NotNull{T, TProperty}(IRuleBuilder{T, TProperty})"/> validator
        /// to disallow <see langword="null"/> values.
        /// </summary>
        /// <typeparam name="T">The type of the object being validated.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator is defined.</param>
        /// <returns>The <paramref name="ruleBuilder"/> instance to continue configuring the property validator.</returns>
        /// <exception cref="ArgumentNullException">If the specified <paramref name="ruleBuilder"/> argument is <see langword="null"/>.</exception>
        public static IRuleBuilderOptions<T, string> MustBeLastName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder, nameof(ruleBuilder));

            return ruleBuilder.SetValidator(new LastNameValidator<T>());
        }
    }
}
