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

            return LoadXml(path);
        }

        /// <summary>
        /// Loads the app.config XML of the file specified by <paramref name="applicationConfigurationFilePath"/>
        /// </summary>
        public static string LoadXml(string applicationConfigurationFilePath)
        {
            try
            {
                return File.ReadAllText(applicationConfigurationFilePath, Encoding.UTF8);
            }
            catch (Exception exception)
            {
                throw new IOException($"Reading app.config XML from {applicationConfigurationFilePath} failed", exception);
            }
        }

        /// <summary>
        /// Replaces the app.config XML of the currently executing process
        /// </summary>
        public static void SaveXml(string xml)
        {
            var path = GetPath();
            SaveXml(path, xml);
        }

        /// <summary>
        /// Replaces the app.config XML of the file specified by <paramref name="applicationConfigurationFilePath"/>
        /// </summary>
        public static void SaveXml(string applicationConfigurationFilePath, string xml)
        {
            try
            {
                File.WriteAllText(applicationConfigurationFilePath, xml, Encoding.UTF8);
            }
            catch (Exception exception)
            {
                throw new IOException($"Writing app.config XML to {applicationConfigurationFilePath} failed", exception);
            }
        }
    }
}