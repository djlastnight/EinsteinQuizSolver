// <copyright file="MultiRuleSet.cs" company="Ivan Yankov">
//     Copyright (c) Ivan Yankov 2018. All rights reserved.
// </copyright>
// <summary>.</summary>
// <author>Ivan Yankov</author>
namespace EinsteinQuiz.Rules
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// MathRuleSet class.
    /// </summary>
    public class MultiRuleSet : IRuleSet
    {
        /// <summary>
        /// Field, which holds the strict rule.
        /// </summary>
        private Rule rule;

        /// <summary>
        /// Field, which holds the math rule.
        /// </summary>
        private MathRule mathRule;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiRuleSet"/> class, using multiple rules.
        /// <remarks>All of the rules are possible, but only one is the right one.</remarks>
        /// </summary>
        /// <param name="rule">The rule, which contains two houses.</param>
        /// <param name="mathRule">The math rule, which implements the arithmetical relations between the houses.</param>
        /// <exception cref="ArgumentException">Thrown, when the rule.Houses.Count != 2.</exception>
        public MultiRuleSet(Rule rule, MathRule mathRule)
        {
            if (rule.Houses.Count != 2)
            {
                throw new ArgumentException("The rule houses count must be 2!");
            }

            this.rule = rule;
            this.mathRule = mathRule;
        }

        /// <summary>
        /// Gets the associated strict <see cref="Rule"/>.
        /// </summary>
        public Rule Rule
        {
            get
            {
                return this.rule;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the IRuleSet is multi rule set.
        /// </summary>
        public bool IsMultiRule
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the associated <see cref="MathRule"/>.
        /// </summary>
        public MathRule MathRule
        {
            get
            {
                return this.mathRule;
            }
        }
    }
}