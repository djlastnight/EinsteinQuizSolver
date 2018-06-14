// <copyright file="PropertyInfo.cs" company="Ivan Yankov">
//     Copyright (c) Ivan Yankov 2018. All rights reserved.
// </copyright>
// <summary>.</summary>
// <author>Ivan Yankov</author>
namespace EinsteinQuiz.Common
{
    /// <summary>
    /// PropertyInfo helper class.
    /// </summary>
    public class PropertyInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyInfo"/> class.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <param name="propertyValue">The property value.</param>
        public PropertyInfo(string propertyName, string propertyValue)
        {
            this.PropertyName = propertyName;
            this.PropertyValue = propertyValue;
        }

        /// <summary>
        /// Gets the property name as string.
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// Gets the property value as string.
        /// </summary>
        public string PropertyValue { get; private set; }
    }
}