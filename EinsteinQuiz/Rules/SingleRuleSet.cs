// <copyright file="SingleRuleSet.cs" company="Ivan Yankov">
//     Copyright (c) Ivan Yankov 2018. All rights reserved.
// </copyright>
// <summary>.</summary>
// <author>Ivan Yankov</author>
namespace EinsteinQuiz.Rules
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a single rule set. Contains a single strict rule.
    /// </summary>
    public class SingleRuleSet : IRuleSet
    {
        /// <summary>
        /// The rule filed.
        /// </summary>
        private readonly Rule rule;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleRuleSet"/> class, using a single strict rule.
        /// </summary>
        /// <param name="rule">The strict rule.</param>
        public SingleRuleSet(Rule rule)
        {
            this.rule = rule;
        }

        /// <summary>
        /// Gets the <see cref="Rule"/>.
        /// </summary>
        public Rule Rule
        {
            get
            {
                return this.rule;
            }
        }

        /// <summary>
        /// Gets the <see cref="MathRule"/>. In this case returns null.
        /// </summary>
        public MathRule MathRule
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the IRuleSet is multi.
        /// </summary>
        public bool IsMultiRule
        {
            get
            {
                return false;
            }
        }
    }
}
