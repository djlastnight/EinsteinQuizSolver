// <copyright file="Extensions.cs" company="Ivan Yankov">
//     Copyright (c) Ivan Yankov 2018. All rights reserved.
// </copyright>
// <summary>.</summary>
// <author>Ivan Yankov</author>
namespace EinsteinQuiz
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extension methods class.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets the permutations of a given collection&lt;T&gt;.
        /// </summary>
        /// <typeparam name="T">The permutation value type.</typeparam>
        /// <param name="list">The collection&lt;T&gt;.</param>
        /// <param name="length">The max length of each permutation.</param>
        /// <returns>Collection, which contains the permutations.</returns>
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> list, int length)
        {
            if (length == 1)
            {
                return list.Select(t => new T[] { t });
            }

            return GetPermutations(list, length - 1).SelectMany(
                t => list.Where(e => !t.Contains(e)),
                (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        /// <summary>
        /// Checks whether the object has a default or null value.
        /// </summary>
        /// <param name="obj">The input object.</param>
        /// <returns>True if object is null or not set.</returns>
        public static bool IsNullOrDefault(this object obj)
        {
            if (obj == null)
            {
                return true;
            }

            if (obj is Enum)
            {
                return (int)obj == 0;
            }

            if (obj is int || obj is byte)
            {
                return Convert.ToInt32(obj) == 0;
            }

            return false;
        }
    }
}
