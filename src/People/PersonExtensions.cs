//-----------------------------------------------------------------------
// <copyright file="PersonExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.People
{
    /// <summary>
    /// Contains extensions methods for the <see cref="IPerson"/>.
    /// </summary>
    public static class PersonExtensions
    {
        /// <summary>
        /// Gets the display full name in the format <c>"First-Name LASTNAME"</c> (For example <c>John DOE</c>).
        /// This full name convention allows to display the name of a person in UI.
        /// </summary>
        /// <param name="person"><see cref="IPerson"/> to retrieve the full name for display.</param>
        /// <returns>The normalized display full name.</returns>
        /// <exception cref="ArgumentNullException">If the specified <paramref name="person"/> argument is <see langword="null"/>.</exception>
        public static string GetFullNameForDisplay(this IPerson person)
        {
            ArgumentNullException.ThrowIfNull(person, nameof(person));

            return NameNormalizer.GetFullNameForDisplay(person.FirstName, person.LastName);
        }

        /// <summary>
        /// Gets the ordering full name in the format <c>"LASTNAME First-Name"</c> (For example <c>DOE John</c>)
        /// of the specified <paramref name="person"/>.
        /// This full name convention allows to order a set of person by there last name first and the first name next.
        /// </summary>
        /// <param name="person"><see cref="IPerson"/> to retrieve the full name for order.</param>
        /// <returns>The normalized ordering full name.</returns>
        /// <exception cref="ArgumentNullException">If the specified <paramref name="person"/> argument is <see langword="null"/>.</exception>
        public static string GetFullNameForOrder(this IPerson person)
        {
            ArgumentNullException.ThrowIfNull(person, nameof(person));

            return NameNormalizer.GetFullNameForOrder(person.FirstName, person.LastName);
        }

        /// <summary>
        /// Gets the initials of the specified <paramref name="person"/>.
        /// The initials are the first letter of the <see cref="IPerson.FirstName"/> and the first letter
        /// of the <see cref="IPerson.LastName"/>.
        /// </summary>
        /// <param name="person">The <see cref="IPerson"/> to retrieve the initials.</param>
        /// <returns>The initials of the <paramref name="person"/>.</returns>
        /// <exception cref="ArgumentNullException">If the specified <paramref name="person"/> argument is <see langword="null"/>.</exception>
        public static string GetInitials(this IPerson person)
        {
            ArgumentNullException.ThrowIfNull(person, nameof(person));

            return $"{person.FirstName[0]}{person.LastName[0]}";
        }
    }
}
