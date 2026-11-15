using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace CrystalImageHelper
{
    /// <summary>
    /// Converts images to 24-bit RGB (no alpha) so Crystal Reports can consume them reliably.
    /// </summary>
    public static class ImageNormalizer
    {
        /// <summary>
        /// Reads the source stream, draws onto a white 24-bit canvas and writes to targetPath.
        /// Uses a temporary file and then renames to avoid partial writes.
        /// </summary>
        public static void SaveAs24BitRgb(Stream sourceStream, string targetPath)
        {
            if (sourceStream == null)
                throw new ArgumentNullException("sourceStream");
            if (string.IsNullOrWhiteSpace(targetPath))
                throw new ArgumentException("targetPath is required", "targetPath");

            string tempPath = targetPath + "." + Guid.NewGuid().ToString("N") + ".tmp";

            try
            {
                using (Image source = Image.FromStream(sourceStream))
                using (Bitmap rgbCanvas = new Bitmap(source.Width, source.Height, PixelFormat.Format24bppRgb))
                using (Graphics graphics = Graphics.FromImage(rgbCanvas))
                {
                    graphics.Clear(Color.White);
                    graphics.DrawImage(source, 0, 0, source.Width, source.Height);

                    ImageFormat format = ChooseFormat(targetPath);
                    rgbCanvas.Save(tempPath, format);
                }

                ReplaceFile(tempPath, targetPath);
            }
            finally
            {
                if (File.Exists(tempPath))
                    File.Delete(tempPath);
            }
        }

        private static ImageFormat ChooseFormat(string targetPath)
        {
            string extension = Path.GetExtension(targetPath);
            if (string.Equals(extension, ".png", StringComparison.OrdinalIgnoreCase))
                return ImageFormat.Png;

            return ImageFormat.Jpeg;
        }

        private static void ReplaceFile(string tempPath, string targetPath)
        {
            if (File.Exists(targetPath))
                File.Delete(targetPath);

            File.Move(tempPath, targetPath);
        }
    }
}
