//-----------------------------------------------------------------------
// <copyright file="CollectionExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace System.Collections.Generic
{
    /// <summary>
    /// Provides extension methods for collections and sequences.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>Adds all items to the collection in enumeration order.</summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="collection">The collection to modify.</param>
        /// <param name="items">The items to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            ArgumentNullException.ThrowIfNull(collection);
            ArgumentNullException.ThrowIfNull(items);

            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        /// <summary>Returns a sequence containing the specified value as its only element.</summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value to include.</param>
        /// <returns>A sequence containing exactly one element.</returns>
        /// <remarks>If the value is itself a collection, it is returned as one element rather than enumerated.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        public static IEnumerable<T> AsSequence<T>(this T value)
        {
            ArgumentNullException.ThrowIfNull(value);

            return Enumerable.Repeat(value, 1);
        }

        /// <summary>Finds the index of the first item matching the predicate.</summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="items">The list to search.</param>
        /// <param name="predicate">The predicate used to test each item.</param>
        /// <returns>The index of the first matching item, or <see langword="null"/> if no item matches.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="items"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
        public static int? IndexOf<T>(this IReadOnlyList<T> items, Func<T, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(predicate);

            for (var i = 0; i < items.Count; i++)
            {
                if (predicate(items[i]))
                {
                    return i;
                }
            }

            return null;
        }

        /// <summary>Removes all items matching the predicate from the collection.</summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="collection">The collection to modify.</param>
        /// <param name="predicate">The predicate used to test each item.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">The collection is read-only.</exception>
        public static void RemoveAll<T>(this ICollection<T> collection, Func<T, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(collection);
            ArgumentNullException.ThrowIfNull(predicate);

            if (collection.IsReadOnly)
            {
                throw new ArgumentException("The collection is read-only.", nameof(collection));
            }

            var copy = collection.ToArray();

            foreach (var item in copy.Where(predicate))
            {
                collection.Remove(item);
            }
        }

        /// <summary>Swaps two items in a list.</summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="list">The list to modify.</param>
        /// <param name="index1">The index of the first item.</param>
        /// <param name="index2">The index of the second item.</param>
        /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Either index is outside the list.</exception>
        public static void Swap<T>(this IList<T> list, int index1, int index2)
        {
            ArgumentNullException.ThrowIfNull(list);

            if (index1 < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index1), index1, "The first index must be greater than zero.");
            }

            if (index1 >= list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index1), index1, "The first index must less than the number of items of the list.");
            }

            if (index2 < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index2), index2, "The second index must be greater than zero.");
            }

            if (index2 >= list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index2), index2, "The second index must less than the number of items of the list.");
            }

            (list[index1], list[index2]) = (list[index2], list[index1]);
        }

        /// <summary>Creates a read-only snapshot of a sequence.</summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="collection">The sequence to copy.</param>
        /// <returns>A read-only collection containing the items at the time of enumeration.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
        public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> collection)
        {
            ArgumentNullException.ThrowIfNull(collection);

            return new ReadOnlyCollection<T>(collection.ToArray());
        }
    }
}