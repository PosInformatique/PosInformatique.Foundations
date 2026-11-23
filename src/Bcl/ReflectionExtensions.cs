//-----------------------------------------------------------------------
// <copyright file="ReflectionExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace System
{
    /// <summary>
    /// Provides extension methods for inspecting types.
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Determines whether a type is a generic type with the specified generic type definition.
        /// </summary>
        /// <param name="type">The type to inspect.</param>
        /// <param name="genericType">The generic type definition to match.</param>
        /// <returns><see langword="true"/> if the type has the specified definition; otherwise, <see langword="false"/>.</returns>
        /// <remarks>Base types and implemented interfaces are not inspected.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> or <paramref name="genericType"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="genericType"/> is not a generic type definition.</exception>
        public static bool IsGeneric(this Type type, Type genericType)
        {
            ArgumentNullException.ThrowIfNull(type);
            ArgumentNullException.ThrowIfNull(genericType);

            if (!genericType.IsGenericTypeDefinition)
            {
                throw new ArgumentException($"The '{genericType.FullName}' must be a generic type definition.", nameof(genericType));
            }

            if (!type.IsGenericType)
            {
                return false;
            }

            if (type.GetGenericTypeDefinition() != genericType)
            {
                return false;
            }

            return true;
        }
    }
}