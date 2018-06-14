// <copyright file="ConsoleLogger.cs" company="Ivan Yankov">
//     Copyright (c) Ivan Yankov 2018. All rights reserved.
// </copyright>
// <summary>.</summary>
// <author>Ivan Yankov</author>
namespace EinsteinQuiz.Common
{
    using System;

    /// <summary>
    /// RuleLogger class.
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleLogger"/> class.
        /// </summary>
        public ConsoleLogger()
        {
            this.IsEnabled = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the logger is enabled or not.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Writes the passed message to the Console, if currently enabled.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogWrite(string message)
        {
            if (this.IsEnabled)
            {
                Console.WriteLine("Console logger: {0}", message);
            }
        }
    }
}
