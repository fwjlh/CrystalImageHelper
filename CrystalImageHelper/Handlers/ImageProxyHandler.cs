using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace CrystalImageHelper
{
    /// <summary>
    /// HTTP handler that serves images from a local cache, downloading from Azure when needed.
    /// URL contract: last two path segments are treated as {key}/{filename}.
    /// </summary>
    public sealed class ImageProxyHandler : HttpTaskAsyncHandler
    {
        private readonly LocalImageCache _cache = new LocalImageCache();

        public override bool IsReusable
        {
            get { return true; }
        }

        public override async Task ProcessRequestAsync(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            RouteSegments route = RouteParser.TryParse(context.Request);
            if (route == null)
            {
                WriteError(context.Response, 400, "Expected URL format ending with /{key}/{filename}");
                return;
            }

            SourceConfig sourceConfig = SourceConfigLoader.TryLoad(route.Key);
            if (sourceConfig == null)
            {
                WriteError(context.Response, 404, "Unknown source key: " + route.Key);
                return;
            }

            string localPath = Path.Combine(sourceConfig.LocalFolder, route.FileName);
            bool resolved = await _cache.EnsureLocalFileAsync(sourceConfig, route.FileName, localPath)
                                        .ConfigureAwait(false);

            if (!resolved)
            {
                WriteError(context.Response, 404, "Image not found locally or in Azure.");
                return;
            }

            ServeFile(context.Response, localPath);
        }

        private static void ServeFile(HttpResponse response, string localPath)
        {
            FileInfo fileInfo = new FileInfo(localPath);

            response.Clear();
            response.StatusCode = 200;
            response.ContentType = MimeMapping.GetMimeMapping(localPath);
            response.AddHeader("Content-Length", fileInfo.Length.ToString());
            response.TransmitFile(localPath);
        }

        private static void WriteError(HttpResponse response, int statusCode, string message)
        {
            response.Clear();
            response.StatusCode = statusCode;
            response.ContentType = "text/plain";
            response.Write(message);
        }
    }
}
