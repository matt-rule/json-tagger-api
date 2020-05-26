using JsonTaggerApi.Model.EntityFrameworkModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace JsonTaggerApi.Model.BusinessLogic
{
    public static class ImageProcessing
    {
        public static ImageMatch AddGuidToMatchIfMissing(ImageMatch original) =>
            new ImageMatch(
                original.Filename1,
                original.Filename2,
                original.Difference,
                original.Decision,
                original.Guid ?? System.Guid.NewGuid().ToString()
            );

        public static double MapRegions (RectangularRegion region, Func<int, int, byte> f) =>
            Enumerable.Range(region.X1, region.X2 + 1 - region.X1)
            .SelectMany(x =>
                Enumerable.Range(region.Y1, region.Y2 + 1 - region.Y1)
                .Select(y => (double)(f (x, y)))
            )
            .Average();

        public static double[] AveragePixelColour(Bitmap image, RectangularRegion region) =>
            new double[] {
                MapRegions (region, (x, y) => image.GetPixel(x, y).R),
                MapRegions (region, (x, y) => image.GetPixel(x, y).G),
                MapRegions (region, (x, y) => image.GetPixel(x, y).B)
            };

        /// <summary>
        /// Call with new Bitmap(imageFilename)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<double[]> PixelColourAverages(Bitmap bmp, int divisionsPerAxis) =>
            ImageRegion
            .Divide(divisionsPerAxis, bmp.Width, bmp.Height)
            .Select(x => AveragePixelColour(bmp, x));

        /// <summary>
        /// Takes an image from a memory stream and downscales it to a thumbnail of the specified width and height.
        /// Aspect ratio is lost.
        /// </summary>
        /// <param name="source">The Stream to get the image from.</param>
        /// <param name="width">The target width.</param>
        /// <param name="height">The target height.</param>
        /// <returns>A thumbnail image of the specified dimensions.</returns>
        public static Func<Stream, int, int, Image?> CreateThumbnail = (Stream source, int width, int height) => {

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

        /// <summary>
        /// Construct a suitable filename for a thumbnail, including the extension.
        /// </summary>
        /// <param name="filenameWithoutExtension">The base string to use, not including an extension nor the . character.</param>
        /// <returns>null if any characters were encountered in the base string, or the thumbnail filename if valid.</returns>
        public static Func<string, string?> GetThumbFileName = (string filenameWithoutExtension) =>
            (filenameWithoutExtension.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                ? null
                : "thumb_" + filenameWithoutExtension + ".jpg";
    }
}