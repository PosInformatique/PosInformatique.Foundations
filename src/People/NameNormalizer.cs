//-----------------------------------------------------------------------
// <copyright file="NameNormalizer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.People
{
    /// <summary>
    /// Provides helper methods to standardize how the name of a person is presented and referenced.
    /// </summary>
    public static class NameNormalizer
    {
        /// <summary>
        /// Gets the display full name in the format <c>"First-Name LASTNAME"</c> (For example <c>John DOE</c>).
        /// This full name convention allows to display the name of a person in UI.
        /// </summary>
        /// <param name="firstName">The <see cref="FirstName"/> of the person.</param>
        /// <param name="lastName">The <see cref="LastName"/> of the person.</param>
        /// <returns>The normalized display full name.</returns>
        /// <exception cref="ArgumentNullException">If the specified <paramref name="firstName"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If the specified <paramref name="lastName"/> argument is <see langword="null"/>.</exception>
        public static string GetFullNameForDisplay(FirstName firstName, LastName lastName)
        {
            ArgumentNullException.ThrowIfNull(firstName, nameof(firstName));
            ArgumentNullException.ThrowIfNull(lastName, nameof(lastName));

            return $"{firstName} {lastName}";
        }

        /// <summary>
        /// Gets the ordering full name in the format <c>"LASTNAME First-Name"</c> (For example <c>DOE John</c>).
        /// This full name convention allows to order a set of person by there last name first and the first name next.
        /// </summary>
        /// <param name="firstName">The person's first name.</param>
        /// <param name="lastName">The person's last name.</param>
        /// <returns>The normalized ordering full name.</returns>
        /// <exception cref="ArgumentNullException">If the specified <paramref name="firstName"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If the specified <paramref name="lastName"/> argument is <see langword="null"/>.</exception>
        public static string GetFullNameForOrder(FirstName firstName, LastName lastName)
        {
            ArgumentNullException.ThrowIfNull(firstName, nameof(firstName));
            ArgumentNullException.ThrowIfNull(lastName, nameof(lastName));

            return $"{lastName} {firstName}";
        }
    }
}
