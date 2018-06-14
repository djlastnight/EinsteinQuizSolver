// <copyright file="Program.cs" company="Ivan Yankov">
//     Copyright (c) Ivan Yankov 2018. All rights reserved.
// </copyright>
// <summary>.</summary>
// <author>Ivan Yankov</author>
namespace EinsteinQuiz
{
    using System;
    using Microsoft.SolverFoundation.Services;

    /// <summary>
    /// Console Application EntryPoint.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The app main method.
        /// </summary>
        /// <param name="args">The arguments to the application.</param>
        private static void Main(string[] args)
        {
            var context = SolverContext.GetContext();
            var model = context.CreateModel();

            // Defining houses
            var house1 = new Decision(Domain.Set(1), "House_1");
            var house2 = new Decision(Domain.Set(2), "House_2");
            var house3 = new Decision(Domain.Set(3), "House_3");
            var house4 = new Decision(Domain.Set(4), "House_4");
            var house5 = new Decision(Domain.Set(5), "House_5");
            model.AddDecisions(house1, house2, house3, house4, house5);

            // Defining nationalities
            var norwegian = Define("Norwegian");
            var englishman = Define("Englishman");
            var swedish = Define("Swedish");
            var danish = Define("Danish");
            var german = Define("German");
            model.AddDecisions(norwegian, englishman, swedish, danish, german);

            // Defining house colors
            var red = Define("Red");
            var green = Define("Green");
            var white = Define("White");
            var blue = Define("Blue");
            var yellow = Define("Yellow");
            model.AddDecisions(red, green, white, blue, yellow);

            // Defining pets
            var dog = Define("Dog");
            var cat = Define("Cat");
            var bird = Define("Bird");
            var horse = Define("Horse");
            var fish = Define("Fish");
            model.AddDecisions(dog, cat, bird, horse, fish);

            // Defining drinks
            var tea = Define("Tea");
            var coffee = Define("Coffee");
            var milk = Define("Milk");
            var beer = Define("Beer");
            var water = Define("Water");
            model.AddDecisions(tea, coffee, milk, beer, water);

            // Defining cigarettes
            var pallMall = Define("PallMall");
            var winfield = Define("Winfield");
            var marlboro = Define("Marlboro");
            var dunhill = Define("Dunhill");
            var rothmans = Define("Rothmans");
            model.AddDecisions(pallMall, winfield, marlboro, dunhill, rothmans);

            // Defining the rules
            model.AddConstraints(
                "constraints",
                Model.AllDifferent(house1, house2, house3, house4, house5),
                Model.AllDifferent(norwegian, englishman, swedish, danish, german),
                Model.AllDifferent(red, green, white, blue, yellow),
                Model.AllDifferent(dog, cat, bird, horse, fish),
                Model.AllDifferent(tea, coffee, milk, beer, water),
                Model.AllDifferent(pallMall, winfield, marlboro, dunhill, rothmans),
                englishman == red, /* rule #1 */
                swedish == dog, /* rule #2 */
                danish == tea, /* rule #3 */
                white - green == 1, /* rule #4 */
                house1 == norwegian, /* rule #5 */
                green == coffee, /* rule #6 */
                pallMall == bird, /* rule #7 */
                house3 == milk, /* rule #8 */
                dunhill == yellow, /* rule #9 */
                Model.Abs(marlboro - cat) == 1, /* rule #10 */
                Model.Abs(horse - dunhill) == 1, /* rule #11 */
                winfield == beer, /* rule #12 */
                Model.Abs(norwegian - blue) == 1, /* rule #13 */
                german == rothmans, /* rule #14 */
                Model.Abs(marlboro - water) == 1 /* rule #15 */);

            var solution = context.Solve(new ConstraintProgrammingDirective());
            var report = solution.GetReport();
            Console.Write(report);

            foreach (var decision in solution.Decisions)
            {
                if (decision.Name == fish.Name)
                {
                    Console.WriteLine(new string('-', 50));
                    Console.WriteLine("Solution: The 'Fish' lives at house #{0}", decision.GetString());
                    Console.WriteLine(new string('-', 50));
                }
            }

            Console.WriteLine("Press [Enter] To Exit...");
            Console.ReadLine();
        }

        /// <summary>
        /// Creates a decision, using an integer range 1-5.
        /// </summary>
        /// <param name="decisionName">The name of the decision.</param>
        /// <returns>New decision.</returns>
        private static Decision Define(string decisionName)
        {
            return new Decision(Domain.IntegerRange(1, 5), decisionName);
        }
    }
}
