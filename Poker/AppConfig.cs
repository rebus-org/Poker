using System;

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
    }
}