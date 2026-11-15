using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CrystalImageHelper
{
    /// <summary>
    /// Ensures a local file exists by downloading from Azure when necessary.
    /// Uses per-file semaphores to avoid concurrent downloads of the same image.
    /// </summary>
    public sealed class LocalImageCache
    {
        private static readonly HttpClient SharedHttpClient = CreateHttpClient();
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> FileLocks =
            new ConcurrentDictionary<string, SemaphoreSlim>(StringComparer.OrdinalIgnoreCase);

        private static HttpClient CreateHttpClient()
        {
            // Azure Blob requires TLS 1.2; older Windows Server defaults may omit it.
            System.Net.ServicePointManager.SecurityProtocol |=
                System.Net.SecurityProtocolType.Tls12;

            return new HttpClient { Timeout = TimeSpan.FromSeconds(15) };
        }

        /// <summary>
        /// Returns true when the file is present locally after the call (either already cached or freshly downloaded).
        /// </summary>
        public async Task<bool> EnsureLocalFileAsync(SourceConfig config, string fileName, string localPath)
        {
            if (config == null)
                throw new ArgumentNullException("config");
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("fileName is required", "fileName");
            if (string.IsNullOrWhiteSpace(localPath))
                throw new ArgumentException("localPath is required", "localPath");

            if (File.Exists(localPath))
                return true;

            Directory.CreateDirectory(config.LocalFolder);

            SemaphoreSlim semaphore = FileLocks.GetOrAdd(localPath, _ => new SemaphoreSlim(1, 1));
            await semaphore.WaitAsync().ConfigureAwait(false);

            try
            {
                // Double-check after acquiring the lock.
                if (File.Exists(localPath))
                    return true;

                return await DownloadFromAzureAsync(config, fileName, localPath).ConfigureAwait(false);
            }
            finally
            {
                semaphore.Release();
                SemaphoreSlim removed;
                FileLocks.TryRemove(localPath, out removed);
            }
        }

        private static async Task<bool> DownloadFromAzureAsync(SourceConfig config, string fileName, string localPath)
        {
            Uri azureUri = AzureUriBuilder.Build(config, fileName);

            using (HttpResponseMessage response = await SharedHttpClient.GetAsync(azureUri).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                    return false;

                using (Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                {
                    await PersistStreamAsync(stream, localPath, config.NormalizeForCrystal).ConfigureAwait(false);
                    return true;
                }
            }
        }

        private static async Task PersistStreamAsync(Stream sourceStream, string localPath, bool normalize)
        {
            if (normalize)
            {
                ImageNormalizer.SaveAs24BitRgb(sourceStream, localPath);
                return;
            }

            string tempPath = localPath + "." + Guid.NewGuid().ToString("N") + ".tmp";
            try
            {
                using (FileStream fileStream = File.Create(tempPath))
                {
                    await sourceStream.CopyToAsync(fileStream).ConfigureAwait(false);
                }

                if (File.Exists(localPath))
                    File.Delete(localPath);

                File.Move(tempPath, localPath);
            }
            finally
            {
                if (File.Exists(tempPath))
                    File.Delete(tempPath);
            }
        }
    }
}
