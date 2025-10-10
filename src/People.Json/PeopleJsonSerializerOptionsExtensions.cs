//-----------------------------------------------------------------------
// <copyright file="PeopleJsonSerializerOptionsExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace System.Text.Json
{
    using PosInformatique.Foundations.People.Json;

    /// <summary>
    /// Contains extension methods to configure <see cref="JsonSerializerOptions"/>.
    /// </summary>
    public static class PeopleJsonSerializerOptionsExtensions
    {
        /// <summary>
        /// Registers the <see cref="FirstNameJsonConverter"/> and <see cref="LastNameJsonConverter"/> to the <paramref name="options"/>.
        /// </summary>
        /// <param name="options"><see cref="JsonSerializerOptions"/> which the <see cref="FirstNameJsonConverter"/> and <see cref="LastNameJsonConverter"/>
        /// converter will be added in the <see cref="JsonSerializerOptions.Converters"/> collection.</param>
        /// <returns>The <paramref name="options"/> instance to continue the configuration.</returns>
        /// <exception cref="ArgumentNullException">If the specified <paramref name="options"/> argument is <see langword="null"/>.</exception>
        public static JsonSerializerOptions AddPeopleConverters(this JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(options, nameof(options));

            if (!options.Converters.Any(c => c.GetType() == typeof(FirstNameJsonConverter)))
            {
                options.Converters.Add(new FirstNameJsonConverter());
            }

            if (!options.Converters.Any(c => c.GetType() == typeof(LastNameJsonConverter)))
            {
                options.Converters.Add(new LastNameJsonConverter());
            }

            return options;
        }
    }
}
