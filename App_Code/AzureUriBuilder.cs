using System;

namespace CrystalImageHelper
{
    /// <summary>
    /// Pure helper that builds the absolute Azure Blob URI for a given file.
    /// </summary>
    public static class AzureUriBuilder
    {
        public static Uri Build(SourceConfig config, string fileName)
        {
            if (config == null)
                throw new ArgumentNullException("config");
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("fileName is required", "fileName");

            string baseUrl = config.AzureBaseUrl.TrimEnd('/');
            string encodedName = Uri.EscapeDataString(fileName);

            string query = string.Empty;
            if (!string.IsNullOrWhiteSpace(config.AzureSasToken))
            {
                query = "?" + config.AzureSasToken.TrimStart('?');
            }

            return new Uri(baseUrl + "/" + encodedName + query, UriKind.Absolute);
        }
    }
}
