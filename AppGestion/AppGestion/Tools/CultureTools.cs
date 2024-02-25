using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGestion.Tools
{
    public static class CultureTools
    {
        /// <summary>
        /// Get the configured language
        /// </summary>
        /// <returns>The configured language</returns>
        public static string GetConfiguredCulture()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(
                ConfigurationUserLevel.None
            );

            string? culture = ConfigurationManager.AppSettings["Culture"];

            // If the culture is not set, set it to fr-CA
            if (culture is null)
            {
                config.AppSettings.Settings["Culture"].Value = "fr-CA";
                culture = "fr-CA";
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }

            return culture!;
        }
    }
}
