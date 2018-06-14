// <copyright file="House.cs" company="Ivan Yankov">
//     Copyright (c) Ivan Yankov 2018. All rights reserved.
// </copyright>
// <summary>.</summary>
// <author>Ivan Yankov</author>
namespace EinsteinQuiz.Models
{
    using EinsteinQuiz.Enums;

    /// <summary>
    /// House class, which represents a single house object.
    /// <para></para>
    /// <para>Remarks: Properties, which type is enum must be named as the type name.</para>
    /// <para>Example 1: public Color Color { get; set; }</para>
    /// <para>Example 2: public Drink Drink { get; set; }</para>
    /// </summary>
    public class House
    {
        /// <summary>
        /// Represents the min allowed house number.
        /// </summary>
        public static readonly byte MinHouseNumber = 1;

        /// <summary>
        /// Represents the max allowed house number.
        /// </summary>
        public static readonly byte MaxHouseNumber = 5;

        /// <summary>
        /// Gets or sets the house color.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets the house number.
        /// <remarks>The property name is hardcoded. In case you change it, please do change it at <see cref="RuleManager"/> class too.</remarks>
        /// </summary>
        public byte Number { get; set; }
        
        /// <summary>
        /// Gets or sets the house nationality.
        /// </summary>
        public Nationality Nationality { get; set; }

        /// <summary>
        /// Gets or sets the house drink.
        /// </summary>
        public Drink Drink { get; set; }

        /// <summary>
        /// Gets or sets the house cigarettes brand.
        /// </summary>
        public Cigarettes Cigarettes { get; set; }

        /// <summary>
        /// Gets or sets the house pet.
        /// </summary>
        public Pet Pet { get; set; }

        /// <summary>
        /// Gets value for a given house property.
        /// </summary>
        /// <param name="propertyName">The property name, which must equals the property type name (for enumerated types only).</param>
        /// <returns>Null if there is not such property.</returns>
        public object GetValue(string propertyName)
        {
            foreach (var property in this.GetType().GetProperties())
            {
                if (property.Name == propertyName)
                {
                    return property.GetValue(this, null);
                }
            }

            return null;
        }

        /// <summary>
        /// Converts the current house to a string.
        /// </summary>
        /// <returns>A formatted string.</returns>
        public override string ToString()
        {
            return string.Format(
                "{0} | {1} | {2} | {3} | {4} | {5}",
                this.Number.ToString().PadLeft(1),
                this.Color.ToString().PadLeft(6),
                this.Nationality.ToString().PadLeft(10),
                this.Cigarettes.ToString().PadLeft(8),
                this.Drink.ToString().PadLeft(6),
                this.Pet.ToString().PadLeft(5));
        }
    }
}