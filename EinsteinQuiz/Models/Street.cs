// <copyright file="Street.cs" company="Ivan Yankov">
//     Copyright (c) Ivan Yankov 2018. All rights reserved.
// </copyright>
// <summary>.</summary>
// <author>Ivan Yankov</author>
namespace EinsteinQuiz.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EinsteinQuiz.Enums;

    /// <summary>
    /// Represents neighborhood as described in the quiz.
    /// </summary>
    /// <typeparam name="T">T must be a House type.</typeparam>
    public class Street<T> where T : House
    {
        /// <summary>
        /// Field, which holds the street.
        /// </summary>
        private readonly List<House> street;

        /// <summary>
        /// Initializes a new instance of the <see cref="Street{T}"/> class.
        /// <para>Five houses with numbers 1-5 will be created.</para>
        /// </summary>
        public Street()
        {
            this.street = new List<House>()
                {
                    new House() { Number = 1 },
                    new House() { Number = 2 },
                    new House() { Number = 3 },
                    new House() { Number = 4 },
                    new House() { Number = 5 }
                };
        }

        /// <summary>
        /// Gets the street houses.
        /// </summary>
        public List<House> Houses
        {
            get
            {
                return this.street;
            }
        }

        /// <summary>
        /// Gets or sets house by a given index.
        /// </summary>
        /// <param name="index">Index of the house.</param>
        /// <returns>The searched house.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when no such index is found.</exception>
        public House this[int index]
        {
            get
            {
                return this.street[index];
            }

            set
            {
                this.street[index] = value;
            }
        }

        /// <summary>
        /// Gets or sets house by a given nationality.
        /// </summary>
        /// <param name="nationality">A Nationality enumeration value.</param>
        /// <returns>The first found house or null if not found.</returns>
        public House this[Nationality nationality]
        {
            get
            {
                return this.street.FirstOrDefault(h => h.Nationality == nationality);
            }

            set
            {
                var house = this.street.FirstOrDefault(h => h.Nationality == nationality);
                if (house != null)
                {
                    house.Nationality = nationality;
                }
            }
        }

        /// <summary>
        /// Gets or sets house by a given color.
        /// </summary>
        /// <param name="color">A Color enumeration value.</param>
        /// <returns>The first found house or null if not found.</returns>
        public House this[Color color]
        {
            get
            {
                return this.street.FirstOrDefault(h => h.Color == color);
            }

            set
            {
                var house = this.street.FirstOrDefault(h => h.Color == color);
                if (house != null)
                {
                    house.Color = color;
                }
            }
        }

        /// <summary>
        /// Gets or sets house by a given cigarettes brand.
        /// </summary>
        /// <param name="cigarettes">A Cigarettes enumeration value.</param>
        /// <returns>The first found house or null if not found.</returns>
        public House this[Cigarettes cigarettes]
        {
            get
            {
                return this.street.FirstOrDefault(h => h.Cigarettes == cigarettes);
            }

            set
            {
                var house = this.street.FirstOrDefault(h => h.Cigarettes == cigarettes);
                if (house != null)
                {
                    house.Cigarettes = cigarettes;
                }
            }
        }

        /// <summary>
        /// Gets or sets house by a given drink.
        /// </summary>
        /// <param name="drink">A Drink enumeration value.</param>
        /// <returns>The first found house or null if not found.</returns>
        public House this[Drink drink]
        {
            get
            {
                return this.street.FirstOrDefault(h => h.Drink == drink);
            }

            set
            {
                var house = this.street.FirstOrDefault(h => h.Drink == drink);
                if (house != null)
                {
                    house.Drink = drink;
                }
            }
        }

        /// <summary>
        /// Gets or sets house by a given pet.
        /// </summary>
        /// <param name="pet">A Pet enumeration value.</param>
        /// <returns>The first found house or null if not found.</returns>
        public House this[Pet pet]
        {
            get
            {
                return this.street.FirstOrDefault(h => h.Pet == pet);
            }

            set
            {
                var house = this.street.FirstOrDefault(h => h.Pet == pet);
                if (house != null)
                {
                    house.Pet = pet;
                }
            }
        }

        /// <summary>
        /// Gets the houses by a given House property and a target value.
        /// </summary>
        /// <param name="propertyName">House class property name, which must equal its type name.</param>
        /// <param name="targetValue">The target value.</param>
        /// <returns>Returns empty list, if nothing is found. Otherwise the searched house(s).</returns>
        public List<House> this[string propertyName, string targetValue]
        {
            get
            {
                List<House> targetHouses = new List<House>();
                foreach (var house in this.Houses)
                {
                    foreach (var property in house.GetType().GetProperties())
                    {
                        if (property.Name == propertyName)
                        {
                            var value = property.GetValue(house, null);
                            if (value.ToString() == targetValue)
                            {
                                targetHouses.Add(house);
                            }
                        }
                    }
                }

                return targetHouses;
            }
        }

        /// <summary>
        /// Creates a street deep clone.
        /// </summary>
        /// <returns>Street cloning with copied values.</returns>
        public Street<House> DeepClone()
        {
            var cloning = new Street<House>();
            var type = typeof(House);

            for (int i = 0; i < this.street.Count; i++)
            {
                foreach (var property in type.GetProperties())
                {
                    var value = property.GetValue(this.street[i], null);
                    if (!value.IsNullOrDefault())
                    {
                        var prop = type.GetProperty(property.Name);
                        prop.SetValue(cloning[i], value, null);
                    }
                }
            }

            return cloning;
        }

        /// <summary>
        /// Converts the <see cref="Street"/> to string with a custom formatting.
        /// </summary>
        /// <returns>Formatted string.</returns>
        public override string ToString()
        {
            string output = string.Empty;
            foreach (var house in this.Houses)
            {
                output += "{ " + house + " } " + Environment.NewLine;
            }

            return output;
        }
    }
}
