// <copyright file="IRuleSet.cs" company="Ivan Yankov">
//     Copyright (c) Ivan Yankov 2018. All rights reserved.
// </copyright>
// <summary>.</summary>
// <author>Ivan Yankov</author>
namespace EinsteinQuiz.Rules
{
    using System.Collections.Generic;

    /// <summary>
    /// The <see cref="IRuleSet"/> interface.
    /// </summary>
    public interface IRuleSet
    {
        /// <summary>
        /// Gets the associated <see cref="Rule"/>.
        /// </summary>
        Rule Rule { get; }

        /// <summary>
        /// Gets the associated <see cref="MathRule"/>.
        /// </summary>
        MathRule MathRule { get; }

        /// <summary>
        /// Gets a value indicating whether the IRuleSet is multi rule set.
        /// </summary>
        bool IsMultiRule { get; }
    }
}