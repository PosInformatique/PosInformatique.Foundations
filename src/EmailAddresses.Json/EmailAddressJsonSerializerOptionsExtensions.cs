//-----------------------------------------------------------------------
// <copyright file="EmailAddressJsonSerializerOptionsExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace System.Text.Json
{
    using PosInformatique.Foundations.EmailAddresses.Json;

    /// <summary>
    /// Contains extension methods to configure <see cref="JsonSerializerOptions"/>.
    /// </summary>
    public static class EmailAddressJsonSerializerOptionsExtensions
    {
        /// <summary>
        /// Registers the <see cref="EmailAddressJsonConverter"/> to the <paramref name="options"/>.
        /// </summary>
        /// <param name="options"><see cref="JsonSerializerOptions"/> which the <see cref="EmailAddressJsonConverter"/>
        /// converter will be added in the <see cref="JsonSerializerOptions.Converters"/> collection.</param>
        /// <returns>The <paramref name="options"/> instance to continue the configuration.</returns>
        /// <exception cref="ArgumentNullException">If the specified <paramref name="options"/> argument is <see langword="null"/>.</exception>
        public static JsonSerializerOptions AddEmailAddressesConverters(this JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(options, nameof(options));

            if (!options.Converters.Any(c => c.GetType() == typeof(EmailAddressJsonConverter)))
            {
                options.Converters.Add(new EmailAddressJsonConverter());
            }

            return options;
        }
    }
}
