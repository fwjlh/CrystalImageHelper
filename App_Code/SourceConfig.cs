namespace CrystalImageHelper
{
    /// <summary>
    /// Immutable configuration for one image source key.
    /// </summary>
    public sealed class SourceConfig
    {
        public string LocalFolder { get; private set; }
        public string AzureBaseUrl { get; private set; }
        public string AzureSasToken { get; private set; }
        public bool NormalizeForCrystal { get; private set; }

        public SourceConfig(
            string localFolder,
            string azureBaseUrl,
            string azureSasToken,
            bool normalizeForCrystal)
        {
            LocalFolder = localFolder;
            AzureBaseUrl = azureBaseUrl;
            AzureSasToken = azureSasToken;
            NormalizeForCrystal = normalizeForCrystal;
        }
    }
}
