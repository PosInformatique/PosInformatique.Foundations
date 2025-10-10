//-----------------------------------------------------------------------
// <copyright file="PeopleAssertionsExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace FluentAssertions
{
    using PosInformatique.Foundations.People;

    /// <summary>
    /// Contains extension methods for custom assertions in unit tests.
    /// </summary>
    public static class PeopleAssertionsExtensions
    {
        /// <summary>
        /// Returns an <see cref="FirstNameAssertions"/> object that can be used to assert the
        /// current <see cref="FirstName"/>.
        /// </summary>
        /// <param name="subject">The <see cref="FirstName"/> to assert.</param>
        /// <returns>An instance of the <see cref="FirstNameAssertions"/> which allows to assert the <see cref="FirstName"/>.</returns>
        /// <exception cref="ArgumentNullException">If the specified <paramref name="subject"/> argument is <see langword="null"/>.</exception>
        public static FirstNameAssertions Should(this FirstName subject)
        {
            ArgumentNullException.ThrowIfNull(subject, nameof(subject));

            return new FirstNameAssertions(subject);
        }

        /// <summary>
        /// Returns an <see cref="LastNameAssertions"/> object that can be used to assert the
        /// current <see cref="LastName"/>.
        /// </summary>
        /// <param name="subject">The <see cref="LastName"/> to assert.</param>
        /// <returns>An instance of the <see cref="LastNameAssertions"/> which allows to assert the <see cref="LastName"/>.</returns>
        /// <exception cref="ArgumentNullException">If the specified <paramref name="subject"/> argument is <see langword="null"/>.</exception>
        public static LastNameAssertions Should(this LastName subject)
        {
            ArgumentNullException.ThrowIfNull(subject, nameof(subject));

            return new LastNameAssertions(subject);
        }
    }
}
