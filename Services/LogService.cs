namespace Countries.Services
{
    using System;


    internal static class LogService
    {
        /// <summary>
        /// information seen on the console, which allows me to understand what is going on
        /// </summary>
        /// <param name="message">message for me</param>
        /// <param name="exception">exception for me</param>
        internal static void Log(string message, Exception exception) 
        {
            Console.WriteLine($"{DateTime.Now} | {message} | {exception.Message}");
        }

        internal static void Log(string message)
        {
            Console.WriteLine($"{DateTime.Now} | {message}");
        }
    }
}
