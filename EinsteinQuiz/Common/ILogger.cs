// <copyright file="ILogger.cs" company="Ivan Yankov">
//     Copyright (c) Ivan Yankov 2018. All rights reserved.
// </copyright>
// <summary>.</summary>
// <author>Ivan Yankov</author>
namespace EinsteinQuiz.Common
{
    /// <summary>
    /// The <see cref="ILogger"/> interface.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Gets or sets a value indicating whether the logging is enabled or not.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Logs the passed message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void LogWrite(string message);
    }
}
