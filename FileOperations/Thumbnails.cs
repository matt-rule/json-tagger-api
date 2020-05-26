using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using JsonTaggerApi.BusinessLogic;

namespace JsonTaggerApi.FileOperations
{
    public static class Thumbnails
    {
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
            string? thumbnailFilename = ImageProcessing.GetThumbFileName(filenameWithoutExtension);

            if (thumbnailFilename == null)
                return;

            string thumbnailPath = Path.Combine(thumbnailFolderPath, thumbnailFilename);

            if (!System.IO.File.Exists(thumbnailPath))
            {
                try {
                    Directory.CreateDirectory(thumbnailFolderPath);

                    using (Stream openFileStream = System.IO.File.Open(localFilePath, FileMode.Open)) {
                        ImageProcessing
                            .CreateThumbnail(openFileStream, 64, 64)
                            ?.Save(thumbnailPath, ImageFormat.Jpeg);
                    }
                }
                catch {
                    return;
                }
            }
        };
    }
}
