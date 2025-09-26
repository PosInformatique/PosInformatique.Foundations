//-----------------------------------------------------------------------
// <copyright file="EmailAddressValidatorExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace FluentValidation
{
    using PosInformatique.Foundations.EmailAddresses;

    /// <summary>
    /// Contains extension methods for <c>FluentValidation</c> to validate e-mail addresses.
    /// </summary>
    public static class EmailAddressValidatorExtensions
    {
        /// <summary>
        /// Defines a validator that checks if a <see cref="string"/> property is a valid e-mail address
        /// (parsable by the <see cref="EmailAddress"/> class).
        /// Validation fails if the value is not a valid e-mail address.
        /// If the <see cref="string"/> value is <see langword="null"/>, validation succeeds.
        /// Use the <see cref="DefaultValidatorExtensions.NotNull{T, TProperty}(IRuleBuilder{T, TProperty})"/> validator
        /// to disallow <see langword="null"/> values.
        /// </summary>
        /// <typeparam name="T">The type of the object being validated.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator is defined.</param>
        /// <returns>The <paramref name="ruleBuilder"/> instance to continue configuring the property validator.</returns>
        /// <exception cref="ArgumentNullException">If the specified <paramref name="ruleBuilder"/> argument is <see langword="null"/>.</exception>
        public static IRuleBuilderOptions<T, string> MustBeEmailAddress<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            ArgumentNullException.ThrowIfNull(ruleBuilder, nameof(ruleBuilder));

            return ruleBuilder.SetValidator(new EmailAddressValidator<T>());
        }
    }
}
