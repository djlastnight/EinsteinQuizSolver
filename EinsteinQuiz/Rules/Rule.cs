// <copyright file="Rule.cs" company="Ivan Yankov">
//     Copyright (c) Ivan Yankov 2018. All rights reserved.
// </copyright>
// <summary>.</summary>
// <author>Ivan Yankov</author>
namespace EinsteinQuiz.Rules
{
    using System.Collections.Generic;
    using EinsteinQuiz.Common;
    using EinsteinQuiz.Models;

    /// <summary>
    /// Rule class.
    /// </summary>
    public class Rule
    {
        /// <summary>
        /// The first mandatory house.
        /// </summary>
        private House houseOne;

        /// <summary>
        /// The other houses array.
        /// </summary>
        private House[] otherHouses;

        /// <summary>
        /// All houses together.
        /// </summary>
        private List<House> allHouses;

        /// <summary>
        /// Initializes a new instance of the <see cref="Rule"/> class.
        /// </summary>
        /// <param name="houseOne">The mandatory house.</param>
        /// <param name="otherHouses">Optional list of additional houses.</param>
        public Rule(House houseOne, params House[] otherHouses)
        {
            this.houseOne = houseOne;
            this.otherHouses = otherHouses;

            var allHouses = new List<House>() { this.houseOne };
            if (this.otherHouses != null)
            {
                allHouses.AddRange(this.otherHouses);
            }

            this.allHouses = allHouses;
        }

        /// <summary>
        /// Gets the associated houses.
        /// </summary>
        public List<House> Houses
        {
            get
            {
                return this.allHouses;
            }
        }

        /// <summary>
        /// Gets list of property->value relations, for the current <see cref="Rule"/>.
        /// </summary>
        /// <returns>Pairs of property->value relations.</returns>
        public List<PropertyInfo> GetRelatedProperties()
        {
            var list = new List<PropertyInfo>();
            foreach (var house in this.Houses)
            {
                foreach (var property in house.GetType().GetProperties())
                {
                    var value = property.GetValue(house, null);

                    if (!value.IsNullOrDefault())
                    {
                        list.Add(new PropertyInfo(property.Name, value.ToString()));
                    }
                }
            }

            return list;
        }
    }
}