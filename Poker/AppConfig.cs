using System;
using System.IO;
using System.Text;

namespace Poker
{
    /// <summary>
    /// Gives access to some useful things when working with .NET application configuration files
    /// </summary>
    public static class AppConfig
    {
        const string AppConfigFileKey = "APP_CONFIG_FILE";

        /// <summary>
        /// Gets the path of the app.config of the currently executing process
        /// </summary>
        public static string GetPath()
        {
            var currentDomain = AppDomain.CurrentDomain;
            var data = currentDomain.GetData(AppConfigFileKey);

            if (data == null)
            {
                throw new ApplicationException($"Could not find '{AppConfigFileKey}' data element in the current appdomain {currentDomain.FriendlyName}");
            }

            return data.ToString();
        }

        /// <summary>
        /// Loads the app.config XML of the currently executing process
        /// </summary>
        public static string LoadXml()
        {
            var path = GetPath();
            try
            {
                return File.ReadAllText(path, Encoding.UTF8);
            }
            catch (Exception exception)
            {
                throw new IOException($"Reading app.config XML from {path} failed", exception);
            }
        }

        /// <summary>
        /// Replaces the app.config XML of the currently executing process
        /// </summary>
        public static void SaveXml(string xml)
        {
            var path = GetPath();
            try
            {
                File.WriteAllText(path, xml, Encoding.UTF8);
            }
            catch (Exception exception)
            {
                throw new IOException($"Writing app.config XML to {path} failed", exception);
            }
        }
    }
}