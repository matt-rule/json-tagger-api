using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace JsonTaggerApi.Model.BusinessLogic
{
    public static class Thumbnails
    {
        public const string THUMBNAIL_EXTERNAL_PATH = "file/thumbnails/";

        public static Func<string, string?> GetThumbnailFilePath = (string basis) =>
            THUMBNAIL_EXTERNAL_PATH + GetThumbnailFilename(basis);

        /// <summary>
        /// Construct a suitable filename for a thumbnail, including the extension.
        /// </summary>
        /// <param name="basis">The string to build the filename around, not including an extension nor the . character.</param>
        /// <returns>null if any characters were encountered in the base string, or the thumbnail filename if valid.</returns>
        public static Func<string, string?> GetThumbnailFilename = (string basis) =>
            (basis.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                ? null
                : "thumb_" + basis + ".jpg";

        /// <summary>
        /// Saves a file to a given path.static (pass in '/data' for non-testing use).
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filenameWithoutExtension"></param>
        /// <param name="extension">From the way it is used this looks as though it should have a . at the beginning.</param>
        /// <returns></returns>
        public static Action<string, string, string> SaveThumbnailToFile = (string path, string filenameWithoutExtension, string extension) => {
            
            string localFilename = filenameWithoutExtension + extension;
            string localFilePath = Path.Combine(path, localFilename);
            string thumbnailFolderPath = Path.Combine(path, "thumbnails");
            string? thumbnailFilename = GetThumbnailFilename(filenameWithoutExtension);

            if (thumbnailFilename == null)
                return;

            string thumbnailPath = Path.Combine(thumbnailFolderPath, thumbnailFilename);

            if (!System.IO.File.Exists(thumbnailPath))
            {
                try {
                    Directory.CreateDirectory(thumbnailFolderPath);

                    using (Stream openFileStream = System.IO.File.Open(localFilePath, FileMode.Open)) {
                        CreateFromStream(openFileStream, 64, 64)
                        ?.Save(thumbnailPath, ImageFormat.Jpeg);
                    }
                }
                catch {
                    return;
                }
            }
        };

        /// <summary>
        /// Takes an image from a memory stream and downscales it to a thumbnail of the specified width and height.
        /// Aspect ratio is lost.
        /// </summary>
        /// <param name="source">The Stream to get the image from.</param>
        /// <param name="width">The target width.</param>
        /// <param name="height">The target height.</param>
        /// <returns>A thumbnail image of the specified dimensions.</returns>
        public static Func<Stream, int, int, Image?> CreateFromStream = (Stream source, int width, int height) => {

            var memoryStream = new MemoryStream();
            source.CopyTo(memoryStream);

            try {
                var image = Image.FromStream(memoryStream);
                return image.GetThumbnailImage(width, height, () => false, IntPtr.Zero);
            }
            catch {
                return null;
            }
        };
    }
}
