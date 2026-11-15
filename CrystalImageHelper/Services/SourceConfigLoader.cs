using System;
using System.Configuration;

namespace CrystalImageHelper
{
    /// <summary>
    /// Loads and parses Source:{key} entries from appSettings.
    /// </summary>
    public static class SourceConfigLoader
    {
        /// <summary>
        /// Returns the config for the given key, or null when the key is missing or malformed.
        /// Format: LocalFolder|AzureBaseUrl|AzureSasToken|NormalizeForCrystal
        /// </summary>
        public static SourceConfig TryLoad(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return null;

            string raw = ConfigurationManager.AppSettings["Source:" + key];
            if (string.IsNullOrWhiteSpace(raw))
                return null;

            return ParseRaw(raw);
        }

        /// <summary>
        /// Pure parser for the pipe-delimited configuration string.
        /// </summary>
        public static SourceConfig ParseRaw(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return null;

            string[] parts = raw.Split('|');
            if (parts.Length < 2)
                return null;

            string localFolder = parts[0].Trim();
            string azureBaseUrl = parts[1].Trim();
            string sasToken = parts.Length > 2 ? parts[2].Trim() : string.Empty;

            // Default to true (normalize) unless the fourth segment is explicitly "false".
            bool normalize = true;
            if (parts.Length > 3)
            {
                normalize = !string.Equals(parts[3].Trim(), "false", StringComparison.OrdinalIgnoreCase);
            }

            if (string.IsNullOrWhiteSpace(localFolder) || string.IsNullOrWhiteSpace(azureBaseUrl))
                return null;

            return new SourceConfig(localFolder, azureBaseUrl, sasToken, normalize);
        }
    }
}
