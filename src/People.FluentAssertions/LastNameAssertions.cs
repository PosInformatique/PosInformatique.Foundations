//-----------------------------------------------------------------------
// <copyright file="LastNameAssertions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.People
{
    using FluentAssertions;
    using FluentAssertions.Primitives;

    /// <summary>
    /// Contains assert methods to check the <see cref="LastName"/> instances.
    /// </summary>
    public sealed class LastNameAssertions : ObjectAssertions<LastName, LastNameAssertions>
    {
        internal LastNameAssertions(LastName value)
            : base(value)
        {
        }

        /// <summary>
        /// Asserts that <see cref="LastName"/> is exactly the same as another string.
        /// </summary>
        /// <param name="lastName">The expected first name in <see cref="string"/>.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because" />.</param>
        /// <returns>An instance of <see cref="AndConstraint{T}"/> with a <see cref="LastNameAssertions"/> to continue
        /// assertion on the string value.</returns>
        public AndConstraint<LastNameAssertions> Be(string lastName, string? because = null, params object[] becauseArgs)
        {
            var assertion = new StringAssertions(this.Subject.ToString());

            assertion.Be(lastName, because, becauseArgs);

            return new AndConstraint<LastNameAssertions>(this);
        }
    }
}
