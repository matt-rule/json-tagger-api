using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using JsonTaggerApi.ImageComparison.BasicTypes;

namespace JsonTaggerApi.ImageComparison.BusinessLogic
{
    public static class ImageProcessing
    {
        /// <summary>
        /// Average a function for each pixel in a region.
        /// </summary>
        /// <param name="region">A region to process.</param>
        /// <param name="f">A function which returns a byte given an X and Y coordinate.</param>
        /// <returns>The double value which is the result of normalising each byte then averaging them all.</returns>
        private static Func<RectangularRegion, Func<int, int, byte>, double> MapRegions
            = (RectangularRegion region, Func<int, int, byte> f) =>
        {
            return
                Enumerable.Range(region.X1, region.X2 + 1 - region.X1)
                .SelectMany(x =>
                    Enumerable.Range(region.Y1, region.Y2 + 1 - region.Y1)
                    .Select(y => (double)(f (x, y)))
                )
                .Average();
        };

        /// <summary>
        /// Produce the average red, green and blue values for a region in a Bitmap.
        /// </summary>
        /// <param name="image">The Bitmap to analyse.</param>
        /// <param name="region">The region within the Bitmap to consider.</param>
        /// <returns>An array containing three elements  (0=red, 1=green, 2=blue).</returns>
        private static double[] AveragePixelColour(Bitmap image, RectangularRegion region) =>
            new double[] {
                MapRegions (region, (x, y) => image.GetPixel(x, y).R),
                MapRegions (region, (x, y) => image.GetPixel(x, y).G),
                MapRegions (region, (x, y) => image.GetPixel(x, y).B)
            };

        /// <summary>
        /// Calculate the average pixel colour for each of divisionsPerAxis^2 regions in a bitmap.
        /// </summary>
        /// <param name="bmp">The Bitmap to calculate pixel averages for. This may be constructed using new Bitmap(imageFilename), for example.</param>
        /// <param name="divisionsPerAxis">Divide the bitmap into this many sections along each of its X and Y axes, producing divisionsPerAxis^2 regions.</param>
        /// <returns>A List of double arrays. Each array contains three elements (0=red, 1=green, 2=blue).</returns>
        public static Func<Bitmap, int, List<double[]>> PixelColourAverages = (Bitmap bmp, int divisionsPerAxis) => {
            return
                ImageRegion
                .Divide(divisionsPerAxis, bmp.Width, bmp.Height)
                .Select(x => AveragePixelColour(bmp, x))
                .ToList();
        };
    }
}