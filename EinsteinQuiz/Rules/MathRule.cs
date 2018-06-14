// <copyright file="MathRule.cs" company="Ivan Yankov">
//     Copyright (c) Ivan Yankov 2018. All rights reserved.
// </copyright>
// <summary>.</summary>
// <author>Ivan Yankov</author>
namespace EinsteinQuiz.Rules
{
    using System;
    using EinsteinQuiz.Enums;

    /// <summary>
    /// MathRule class.
    /// </summary>
    public class MathRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MathRule"/> class.
        /// </summary>
        /// <param name="minValue">The min allowed value.</param>
        /// <param name="maxValue">The max allowed value.</param>
        /// <param name="operation">The math operation to check for.</param>
        /// <param name="targetType">The type of the target, which we will check.</param>
        /// <param name="targetProperty">The name of target type's property.</param>
        public MathRule(int minValue, int maxValue, MathOperation operation, Type targetType, string targetProperty)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.Operation = operation;
            this.TargetType = targetType;
            this.TargetProperty = targetProperty;
        }

        /// <summary>
        /// Gets the <see cref="MathRule"/>'s min value.
        /// </summary>
        public double MinValue { get; private set; }

        /// <summary>
        /// Gets the <see cref="MathRule"/>'s max value.
        /// </summary>
        public double MaxValue { get; private set; }

        /// <summary>
        /// Gets the <see cref="MathRule"/>'s <see cref="MathOperation"/>.
        /// </summary>
        public MathOperation Operation { get; private set; }

        /// <summary>
        /// Gets the <see cref="MathRule"/>'s target type.
        /// </summary>
        public Type TargetType { get; private set; }

        /// <summary>
        /// Gets the <see cref="MathRule"/>'s target property.
        /// </summary>
        public string TargetProperty { get; private set; }
    }
}