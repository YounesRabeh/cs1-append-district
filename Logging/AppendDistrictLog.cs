using UnityEngine;

namespace AppendDistrict
{
    internal static class AppendDistrictLog
    {
        private const string Prefix = "[AppendDistrict] ";

        /// <summary>
        /// Writes an informational log message.
        /// </summary>
        /// <param name="category">Log category label.</param>
        /// <param name="message">Message content.</param>
        public static void Info(string category, string message)
        {
            UnityEngine.Debug.Log(Prefix + "[" + category + "] " + message);
        }

        /// <summary>
        /// Writes a warning log message.
        /// </summary>
        /// <param name="category">Log category label.</param>
        /// <param name="message">Message content.</param>
        public static void Warn(string category, string message)
        {
            UnityEngine.Debug.LogWarning(Prefix + "[" + category + "] " + message);
        }

        /// <summary>
        /// Writes an error log message.
        /// </summary>
        /// <param name="category">Log category label.</param>
        /// <param name="message">Message content.</param>
        public static void Error(string category, string message)
        {
            UnityEngine.Debug.LogError(Prefix + "[" + category + "] " + message);
        }
    }
}
