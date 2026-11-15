using System;
using System.IO;
using System.Web;

namespace CrystalImageHelper
{
    /// <summary>
    /// Pure helpers for extracting and validating the last two URL segments.
    /// </summary>
    public static class RouteParser
    {
        /// <summary>
        /// Returns the last two path segments as key/filename, or null when the path is invalid.
        /// Expects exactly two non-empty segments after the application root.
        /// </summary>
        public static RouteSegments TryParse(HttpRequest request)
        {
            if (request == null)
                return null;

            // Prefer PathInfo when the handler is mapped with path="*/*".
            // Fall back to AbsolutePath so clean URLs still work.
            string path = (request.PathInfo ?? string.Empty).Trim('/');
            if (string.IsNullOrEmpty(path))
            {
                path = (request.AbsolutePath ?? string.Empty).Trim('/');
            }

            string[] parts = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
                return null;

            // Take the last two segments so deeper paths still resolve correctly.
            string key = parts[parts.Length - 2];
            string fileName = parts[parts.Length - 1];

            if (!IsSafeFileName(fileName))
                return null;

            return new RouteSegments(key, fileName);
        }

        /// <summary>
        /// Rejects path traversal and empty names.
        /// </summary>
        public static bool IsSafeFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            // Path.GetFileName strips directory components; if the result differs, the name was unsafe.
            return string.Equals(Path.GetFileName(fileName), fileName, StringComparison.Ordinal);
        }
    }
}
