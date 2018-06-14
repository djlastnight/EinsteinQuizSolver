// <copyright file="StreetRuleManager.cs" company="Ivan Yankov">
//     Copyright (c) Ivan Yankov 2018. All rights reserved.
// </copyright>
// <summary>.</summary>
// <author>Ivan Yankov</author>
namespace EinsteinQuiz.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EinsteinQuiz.Common;
    using EinsteinQuiz.Enums;
    using EinsteinQuiz.Models;

    /// <summary>
    /// RuleManager class intended to create and maintain the quiz rules.
    /// </summary>
    public class StreetRuleManager
    {
        /// <summary>
        /// Field, which holds the rule sets.
        /// </summary>
        private List<IRuleSet> ruleSets;

        /// <summary>
        /// Field, which holds the street.
        /// </summary>
        private Street<House> street;

        /// <summary>
        /// Field, which holds the logger.
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreetRuleManager"/> class.
        /// </summary>
        /// <param name="street">Instance of street object.</param>
        /// <param name="logger">Instance of logger, used to log errors.</param>
        /// <exception cref="ArgumentNullException">Thrown, when logger is null.</exception>
        public StreetRuleManager(Street<House> street, ILogger logger)
        {
            if (street == null)
            {
                throw new ArgumentNullException("street");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            this.street = street;
            this.logger = logger;
            this.ruleSets = this.CreateRules();
        }

        /// <summary>
        /// Gets the <see cref="StreetRuleManager"/>'s street.
        /// </summary>
        public Street<House> Street
        {
            get
            {
                return this.street;
            }
        }

        /// <summary>
        /// Gets the internally created rule sets.
        /// </summary>
        public List<IRuleSet> RuleSets
        {
            get
            {
                return this.ruleSets;
            }
        }

        /// <summary>
        /// Gets List&lt;T&gt;, which contains the currently possible &lt;T&gt; values for a given house index from the specified street.
        /// </summary>
        /// <typeparam name="T">Enum type. Example Color.</typeparam>
        /// <param name="houseIndex">Index of the desired house.</param>
        /// <returns>List of T which contains all the possible values for a given house.</returns>
        public List<T> GetPossibleValues<T>(byte houseIndex) where T : struct, IConvertible
        {
            Type type = typeof(T);
            if (!type.IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            var possibleValues = new List<T>();

            // Assuming that the House class has property, which name equals the enum name. E.g House.Color's type name == Color
            var property = typeof(House).GetProperty(type.Name);
            if (property == null)
            {
                throw new InvalidProgramException("House class does not have property called " + type.Name);
            }

            var value = (T)this.street[houseIndex].GetValue(type.Name);
            if (!value.IsNullOrDefault())
            {
                // The T value for this house is already known
                possibleValues.Add(value);
                return possibleValues;
            }

            foreach (T enumVal in Enum.GetValues(type))
            {
                if (Convert.ToInt32(enumVal) == 0)
                {
                    // Skipping the default enum value
                    continue;
                }

                var houses = this.street[property.Name, enumVal.ToString()];
                if (houses.Count != 0)
                {
                    // This value is already set for another house
                    continue;
                }

                var clonedStreet = this.street.DeepClone();
                property.SetValue(clonedStreet[houseIndex], enumVal, null);
                if (!this.IsThereBrokenRule(clonedStreet))
                {
                    possibleValues.Add(enumVal);
                }
            }

            return possibleValues;
        }

        /// <summary>
        /// Scans the rules and sets the house properties.
        /// </summary>
        /// <returns>Returns true, if the street was changed.</returns>
        public bool TrySetStreetRelations()
        {
            int relationsCount = 0;
            for (byte streetIndex = 0; streetIndex < 5; streetIndex++)
            {
                foreach (Category category in Enum.GetValues(typeof(Category)))
                {
                    switch (category)
                    {
                        case Category.Color:
                            var colors = this.GetPossibleValues<Color>(streetIndex);
                            if (colors.Count == 1 && this.street[streetIndex].Color.IsNullOrDefault())
                            {
                                this.street[streetIndex].Color = colors[0];
                                relationsCount++;
                            }

                            break;
                        case Category.Drink:
                            var drinks = this.GetPossibleValues<Drink>(streetIndex);
                            if (drinks.Count == 1 && this.street[streetIndex].Drink.IsNullOrDefault())
                            {
                                this.street[streetIndex].Drink = drinks[0];
                                relationsCount++;
                            }

                            break;
                        case Category.Nationality:
                            var nationalities = this.GetPossibleValues<Nationality>(streetIndex);
                            if (nationalities.Count == 1 && this.street[streetIndex].Nationality.IsNullOrDefault())
                            {
                                this.street[streetIndex].Nationality = nationalities[0];
                                relationsCount++;
                            }

                            break;
                        case Category.Cigarettes:
                            var brands = this.GetPossibleValues<Cigarettes>(streetIndex);
                            if (brands.Count == 1 && this.street[streetIndex].Cigarettes.IsNullOrDefault())
                            {
                                this.street[streetIndex].Cigarettes = brands[0];
                                relationsCount++;
                            }

                            break;
                        case Category.Pet:
                            var pets = this.GetPossibleValues<Pet>(streetIndex);
                            if (pets.Count == 1 && this.street[streetIndex].Pet.IsNullOrDefault())
                            {
                                this.street[streetIndex].Pet = pets[0];
                                relationsCount++;
                            }

                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }

            return relationsCount > 0;
        }

        /// <summary>
        /// Tries to set each house's category, using a permutations.
        /// </summary>
        /// <param name="category">The category to set.</param>
        /// <returns>Returns true, if the street was changed.</returns>
        public bool TrySetStreetCategory(Category category)
        {
            var range = Enumerable.Range(House.MinHouseNumber, House.MaxHouseNumber);
            var permutations = range.GetPermutations(House.MaxHouseNumber);
            var clone = this.street.DeepClone();
            var property = typeof(House).GetProperty(category.ToString());

            foreach (var permutation in permutations)
            {
                var numbers = permutation.Select(x => x).ToArray();
                for (int streetIndex = 0; streetIndex < this.street.Houses.Count; streetIndex++)
                {
                    var valueSuggestion = numbers[streetIndex];
                    var actualValue = property.GetValue(this.street[streetIndex], null);
                    if (actualValue.IsNullOrDefault())
                    {
                        property.SetValue(clone[streetIndex], valueSuggestion, null);
                    }
                }

                if (!this.IsThereBrokenRule(clone))
                {
                    this.street = clone;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks whether some of the quiz rules is broken.
        /// </summary>
        /// <returns>True if some rule is broken.</returns>
        public bool IsThereBrokenRule()
        {
            return this.IsThereBrokenRule(this.street);
        }

        /// <summary>
        /// Checks whether some of the quiz rules is broken.
        /// </summary>
        /// <param name="street">The street to check (usually a cloning of the original one).</param>
        /// <returns>True if some rule is broken.</returns>
        private bool IsThereBrokenRule(Street<House> street)
        {
            foreach (var ruleSet in this.ruleSets)
            {
                if (ruleSet.IsMultiRule)
                {
                    if (this.IsNeighboursHouseRuleBroken(street, ruleSet.Rule))
                    {
                        return true;
                    }

                    switch (ruleSet.MathRule.Operation)
                    {
                        case MathOperation.SmallerThan:
                            // Value 1 must be smaller than value 2
                            var house1 = ruleSet.Rule.Houses[0];
                            var house2 = ruleSet.Rule.Houses[1];
                            var prop = ruleSet.MathRule.TargetType.GetProperty(ruleSet.MathRule.TargetProperty);
                            var value1 = Convert.ToInt32(prop.GetValue(house1, null));
                            var value2 = Convert.ToInt32(prop.GetValue(house2, null));
                            if (value1 != 0 && value2 != 0)
                            {
                                if (value2 - 1 < ruleSet.MathRule.MinValue)
                                {
                                    return true;
                                }

                                if (value1 + 1 > ruleSet.MathRule.MaxValue)
                                {
                                    return true;
                                }

                                if (!(value1 < value2))
                                {
                                    return true;
                                }
                            }

                            break;
                        default:
                            throw new NotImplementedException(
                                "Not implemented MathOperation: " + ruleSet.MathRule.Operation);
                    }
                }
                else
                {
                    var rule = ruleSet.Rule;
                    if (rule.Houses.Count == 1)
                    {
                        if (this.IsSingleHouseRuleBroken(street, rule))
                        {
                            return true;
                        }
                    }
                    else if (rule.Houses.Count == 2)
                    {
                        if (this.IsNeighboursHouseRuleBroken(street, rule))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Creates internal list of 15 quiz rules.
        /// </summary>
        /// <returns>The quiz rules.</returns>
        private List<IRuleSet> CreateRules()
        {
            // Rule 1: The Englishman lives at the red house
            var ruleSet1 = new SingleRuleSet(new Rule(new House() { Nationality = Nationality.Englishman, Color = Color.Red }));

            // Rule 2: The Swedish has a dog
            var ruleSet2 = new SingleRuleSet(new Rule(new House() { Nationality = Nationality.Swedish, Pet = Pet.Dog }));

            // Rule 3: The Danish drinks tea
            var ruleSet3 = new SingleRuleSet(new Rule(new House() { Nationality = Nationality.Danish, Drink = Drink.Tea }));

            // Rule 4: The green house is at the left side of the white house
            var rule4a = new Rule(
                new House() { Color = Color.Green },
                new House() { Color = Color.White });

            var rule4b = new MathRule(House.MinHouseNumber, House.MaxHouseNumber, MathOperation.SmallerThan, typeof(House), "Number");

            var ruleSet4 = new MultiRuleSet(rule4a, rule4b);

            // Rule 5: The Norwegian lives at the first house
            var ruleSet5 = new SingleRuleSet(new Rule(new House() { Nationality = Nationality.Norwegian, Number = 1 }));

            // Rule 6: The green house owner drinks coffee
            var ruleSet6 = new SingleRuleSet(new Rule(new House() { Color = Color.Green, Drink = Drink.Coffee }));

            // Rule 7: The man who smokes Pall Mall has a bird
            var ruleSet7 = new SingleRuleSet(new Rule(new House() { Cigarettes = Cigarettes.PallMall, Pet = Pet.Bird }));

            // Rule 8: The middle house owner drinks milk
            var ruleSet8 = new SingleRuleSet(new Rule(new House() { Number = 3, Drink = Drink.Milk }));

            // Rule 9: The yellow house owner smokes Dunhill
            var ruleSet9 = new SingleRuleSet(new Rule(new House() { Color = Color.Yellow, Cigarettes = Cigarettes.Dunhill }));

            // Rule 10: The Marlboro smoker has neighbour, which has Cat
            var rule10 = new Rule(
                new House() { Cigarettes = Cigarettes.Marlboro },
                new House() { Pet = Pet.Cat });

            var ruleSet10 = new SingleRuleSet(rule10);

            // Rule 11: The man who has horse has neighbour, which smokes Dunhill
            var rule11 = new Rule(
                new House() { Pet = Pet.Horse },
                new House() { Cigarettes = Cigarettes.Dunhill });

            var ruleSet11 = new SingleRuleSet(rule11);

            // Rule 12: The Winfield smoker drinks beer
            var ruleSet12 = new SingleRuleSet(new Rule(new House() { Cigarettes = Cigarettes.Winfield, Drink = Drink.Beer }));

            // Rule 13: The Norwegian lives next to the blue house
            var rule13 = new Rule(
                new House() { Nationality = Nationality.Norwegian },
                new House() { Color = Color.Blue });

            var ruleSet13 = new SingleRuleSet(rule13);

            // Rule 14: The German smokes Rothmans
            var ruleSet14 = new SingleRuleSet(new Rule(new House() { Nationality = Nationality.German, Cigarettes = Cigarettes.Rothmans }));

            // Rule 15: The Marlboro smoker has neighbour, which drinks water
            var rule15 = new Rule(
                new House() { Cigarettes = Cigarettes.Marlboro },
                new House() { Drink = Drink.Water });

            var ruleSet15 = new SingleRuleSet(rule15);

            var ruleSets = new List<IRuleSet>()
            {
                ruleSet1,
                ruleSet2,
                ruleSet3,
                ruleSet4,
                ruleSet5,
                ruleSet6,
                ruleSet7,
                ruleSet8,
                ruleSet9,
                ruleSet10,
                ruleSet11,
                ruleSet12,
                ruleSet13,
                ruleSet14,
                ruleSet15
            };

            return ruleSets;
        }

        /// <summary>
        /// Checks whether a single house rule is broken.
        /// </summary>
        /// <param name="street">The street to check (usually a cloned one).</param>
        /// <param name="rule">The rule to check.</param>
        /// <returns>True if the rule is broken.</returns>
        private bool IsSingleHouseRuleBroken(Street<House> street, Rule rule)
        {
            var propertyInfos = rule.GetRelatedProperties();
            string[] properties = propertyInfos.Select(x => x.PropertyName).ToArray();
            string[] values = propertyInfos.Select(x => x.PropertyValue).ToArray();

            // Search by property #1
            var houses1 = street[properties[0], values[0]];
            if (houses1.Count > 1)
            {
                // There are at least two houses, which have the same property, value pair set.
                return true;
            }

            if (houses1.Count == 1)
            {
                var value = houses1[0].GetValue(properties[1]);
                if (!value.IsNullOrDefault() && value.ToString() != values[1])
                {
                    var message = string.Format(
                        "House #{0} [{1} <-> {2}] error: {3} != {4}",
                        houses1[0].Number,
                        properties[1].ToLower(),
                        properties[0].ToLower(),
                        value,
                        values[1]);

                    this.logger.LogWrite(message);
                    return true;
                }
            }

            // Search by property #2
            var houses2 = street[properties[1], values[1]];
            if (houses2.Count > 1)
            {
                // There are at least two houses, which have the same property, value pair set.
                return true;
            }

            if (houses2.Count == 1)
            {
                var value = houses2[0].GetValue(properties[0]);
                if (!value.IsNullOrDefault() && value.ToString() != values[0])
                {
                    var message = string.Format(
                        "House #{0} [{1} <-> {2}] error: {3} != {4}",
                        houses2[0].Number,
                        properties[0].ToLower(),
                        properties[1].ToLower(),
                        value,
                        values[0]);

                    this.logger.LogWrite(message);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks whether two house rule (neighbor rule) is broken.
        /// </summary>
        /// <param name="street">The street to check (usually a cloned one).</param>
        /// <param name="rule">The rule to check.</param>
        /// <returns>True if the rule is broken.</returns>
        private bool IsNeighboursHouseRuleBroken(Street<House> street, Rule rule)
        {
            var propertyInfos = rule.GetRelatedProperties();
            string[] properties = propertyInfos.Select(x => x.PropertyName).ToArray();
            string[] values = propertyInfos.Select(x => x.PropertyValue).ToArray();

            // Search by property #1
            var houses1 = street[properties[0], values[0]];
            if (houses1.Count > 1)
            {
                // There are at least two houses, which have the same property, value pair set.
                return true;
            }

            if (houses1.Count == 1)
            {
                var value = houses1[0].GetValue(properties[1]);
                if (!value.IsNullOrDefault() && value.ToString() == values[1])
                {
                    // Current house owns property #2's value
                    return true;
                }
                
                // Checking the neighbours
                var leftNeighbour = this.GetNeighbour(street, houses1[0].Number - 1, true);
                var rightNeighbour = this.GetNeighbour(street, houses1[0].Number - 1, false);
                bool leftValueIsBroken = false;
                bool rightValueIsBroken = false;

                if (leftNeighbour != null)
                {
                    var leftNeighbourValue = leftNeighbour.GetValue(properties[1]);
                    if (!leftNeighbourValue.IsNullOrDefault() && leftNeighbourValue.ToString() != values[1])
                    {
                        leftValueIsBroken = true;
                    }
                }

                if (rightNeighbour != null)
                {
                    var rightNegighbourValue = rightNeighbour.GetValue(properties[1]);
                    if (!rightNegighbourValue.IsNullOrDefault() && rightNegighbourValue.ToString() != values[1])
                    {
                        rightValueIsBroken = true;
                    }
                }

                if (leftValueIsBroken && rightValueIsBroken)
                {
                    return true;
                }
            }

            // Search by property #2
            var houses2 = street[properties[1], values[1]];
            if (houses2.Count > 1)
            {
                // There are at least two houses, which have the same property, value pair set.
                return true;
            }

            if (houses2.Count == 1)
            {
                var value = houses2[0].GetValue(properties[0]);
                if (!value.IsNullOrDefault() && value.ToString() == values[0])
                {
                    // Current house owns property #1's value
                    return true;
                }

                // Checking the neighbours
                var leftNeighbour = this.GetNeighbour(street, houses2[0].Number - 1, true);
                var rightNeighbour = this.GetNeighbour(street, houses2[0].Number - 1, false);
                var leftValueIsBroken = false;
                var rightValueIsBroken = false;

                if (leftNeighbour != null)
                {
                    var leftNeighbourValue = leftNeighbour.GetValue(properties[0]);
                    if (!leftNeighbourValue.IsNullOrDefault() && leftNeighbourValue.ToString() != values[0])
                    {
                        leftValueIsBroken = true;
                    }
                }

                if (rightNeighbour != null)
                {
                    var rightNegighbourValue = rightNeighbour.GetValue(properties[0]);
                    if (!rightNegighbourValue.IsNullOrDefault() && rightNegighbourValue.ToString() != values[0])
                    {
                        rightValueIsBroken = true;
                    }
                }

                if (leftValueIsBroken && rightValueIsBroken)
                {
                    return true;
                }
            }

            if (houses1.Count == 1 &&
                houses2.Count == 1 &&
                houses1[0].Number != 0 &&
                houses2[0].Number != 0)
            {
                if (Math.Abs(houses1[0].Number - houses2[0].Number) != 1)
                {
                    // The #1 and #2 owners are not neighbours
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Searches for neighbor's house.
        /// </summary>
        /// <param name="street">The street, where the neighbors live.</param>
        /// <param name="houseIndex">The pivot house index.</param>
        /// <param name="isLeft">Set to true if you are looking for the left neighbor, otherwise pass false.</param>
        /// <returns>Returns null if no neighbor is found. Otherwise returns the left or the right neighbor, depending on the passed isLeft argument value.</returns>
        private House GetNeighbour(Street<House> street, int houseIndex, bool isLeft)
        {
            var allowedHouseIndicies = Enumerable.Range(0, 5);
            if (!allowedHouseIndicies.Contains(houseIndex))
            {
                throw new ArgumentOutOfRangeException();
            }

            var house1LeftNeightbourIndex = street[houseIndex].Number - 2;
            var house1RightNeighbourIndex = street[houseIndex].Number;
            if (isLeft && allowedHouseIndicies.Contains(house1LeftNeightbourIndex))
            {
                var leftNeighbour = street[house1LeftNeightbourIndex];

                return leftNeighbour;
            }

            if (!isLeft && allowedHouseIndicies.Contains(house1RightNeighbourIndex))
            {
                var rightNeighbour = street[house1RightNeighbourIndex];
                return rightNeighbour;
            }

            return null;
        }
    }
}