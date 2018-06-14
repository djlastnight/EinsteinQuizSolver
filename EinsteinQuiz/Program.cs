// <copyright file="Program.cs" company="Ivan Yankov">
//     Copyright (c) Ivan Yankov 2018. All rights reserved.
// </copyright>
// <summary>.</summary>
// <author>Ivan Yankov</author>
namespace EinsteinQuiz
{
    using System;
    using EinsteinQuiz.Common;
    using EinsteinQuiz.Enums;
    using EinsteinQuiz.Models;
    using EinsteinQuiz.Rules;

    /// <summary>
    /// App's main class.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The app entry point.
        /// </summary>
        /// <param name="args">The arguments to the main method.</param>
        private static void Main(string[] args)
        {
            Console.WriteLine("Trying to solve the Einstein Quiz:");
            Console.WriteLine();
            PrintTheQuiz();

            var startTime = DateTime.Now;
            var maxAllowedTime = TimeSpan.FromSeconds(1);

            var manager = new StreetRuleManager(
                new Street<House>(),
                new ConsoleLogger() { IsEnabled = false });

            // Implement rule #5 (The Norwegian lives at first house)
            manager.Street[0].Nationality = Nationality.Norwegian;

            // Implement rule #8 (The middle house owner drinks milk)
            manager.Street[2].Drink = Drink.Milk;

            // Trying to set the color for every house, using permutations
            if (!manager.TrySetStreetCategory(Category.Color))
            {
                throw new InvalidProgramException(
                    "Application was unable to find the house colors!");
            }

            // Setting the relations
            while (manager.TrySetStreetRelations())
            {
                if ((DateTime.Now - startTime) > maxAllowedTime)
                {
                    throw new InvalidProgramException(
                        "The requested operation took too much time!");
                }
            }

            Console.WriteLine(
                "Executed for {0} milliseconds",
                Math.Round((DateTime.Now - startTime).TotalMilliseconds, 0));

            if (manager.IsThereBrokenRule())
            {
                Console.WriteLine("Unable to find the result!");
            }
            else
            {
                // Scanning for unknown values
                bool hasUnknownValue = false;
                foreach (var house in manager.Street.Houses)
                {
                    foreach (var property in typeof(House).GetProperties())
                    {
                        var value = property.GetValue(house, null);
                        if (value.IsNullOrDefault())
                        {
                            hasUnknownValue = true;
                            break;
                        }
                    }

                    if (hasUnknownValue)
                    {
                        break;
                    }
                }

                if (hasUnknownValue)
                {
                    Console.WriteLine("Unable to find the result!");
                }
                else
                {
                    Console.WriteLine(manager.Street);
                    Console.WriteLine("Solution: The {0} has a fish", manager.Street[Pet.Fish].Nationality);
                }
            }

            Console.WriteLine("Press [Enter] To Exit...");
            Console.ReadLine();
        }

        /// <summary>
        /// Simply prints the quiz rules and hints.
        /// </summary>
        private static void PrintTheQuiz()
        {
            string hints = @"There are five houses in unique colors: blue, green, red, white and yellow.
In each house lives a person of unique nationality: British, Danish, German, Norwegian and Swedish.
Each person drinks a unique beverage: beer, coffee, milk, tea and water.
Each person smokes a unique cigar brand: Rothmans, Dunhill, Pall Mall, Marlboro and Winfield.
Each person keeps a unique pet: cats, birds, dogs, fish and horses.
The following facts are given:

The Brit lives in a red house.
The Swede keeps dogs as pets.
The Dane drinks tea.
The green house is on the left of the white, next to it.
The green house owner drinks coffee.
The person who smokes Pall Mall rears birds.
The owner of the yellow house smokes Dunhill.
The man living in the house right in the center drinks milk.
The Norwegian lives in the first house.
The man who smokes Marlboro lives next to the one who keeps cats.
The man who keeps horses lives next to the man who smokes Dunhill.
The owner who smokes Winfield drinks beer.
The German smokes Rothmans.
The Norwegian lives next to the blue house.
The man who smokes Marlboro has a neighour who drinks water.
The question you need to answer is: Who keeps fish?";

            Console.WriteLine(hints);
            Console.WriteLine();
        }
    }
}
