using JsonTaggerApi.EfCompatibleTypes;
//using Microsoft.EntityFrameworkCore; // to remove
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace JsonTaggerApi.BusinessLogic
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

        // Call with new Bitmap(imageFilename)
        public static IEnumerable<double[]> PixelColourAverages(Bitmap bmp, int divisionsPerAxis) =>
            ImageRegion
            .Divide(divisionsPerAxis, bmp.Width, bmp.Height)
            .Select(x => AveragePixelColour(bmp, x));
    }
}