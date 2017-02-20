using System;
using System.IO;
using System.Text;

namespace Poker
{
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
            return File.ReadAllText(GetPath(), Encoding.UTF8);
        }

        /// <summary>
        /// Replaces the app.config XML of the currently executing process
        /// </summary>
        public static void SaveXml(string xml)
        {
            File.WriteAllText(GetPath(), xml, Encoding.UTF8);
        }
    }
}