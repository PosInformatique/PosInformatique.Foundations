//-----------------------------------------------------------------------
// <copyright file="IPerson.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.People
{
    /// <summary>
    /// Represents a nominative person entity identified by a <see cref="FirstName"/> and a <see cref="LastName"/>.
    /// Use it for domain concepts like user, customer, employee, or contact. Can be implemented
    /// by any business entity to generalize persons.
    /// </summary>
    public interface IPerson
    {
        /// <summary>
        /// Gets the normalized first name of the person.
        /// </summary>
        FirstName FirstName { get; }

        /// <summary>
        /// Gets the normalized last name of the person.
        /// </summary>
        LastName LastName { get; }
    }
}
