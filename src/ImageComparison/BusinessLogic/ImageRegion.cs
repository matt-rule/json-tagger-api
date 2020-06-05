using System.Collections.Generic;
using System.Linq;

using JsonTaggerApi.ImageComparison.BasicTypes;

namespace JsonTaggerApi.ImageComparison.BusinessLogic
{
    /// <summary>
    /// Methods relevant to rectangular image regions.
    /// </summary>
    public static class ImageRegion
    {
        /// <summary>
        /// Divides a length along an axis into ranges.
        /// </summary>
        /// <returns></returns>
        public static (int, int)[] RangesForAxis (int divisions, int axisLength) =>
            Enumerable.Range(0, divisions)
            .Select (
                x => (
                    (int)(axisLength / ((double)divisions) * x),
                    (int)(axisLength / ((double)divisions) * (x + 1)) - 1
                )
            )
            .ToArray();

        public static List<RectangularRegion> GetRegionsFromXYRanges((int, int)[] xRanges, (int, int)[] yRanges) =>
            yRanges
                .SelectMany(yRange =>
                    xRanges
                    .Select(xRange =>
                        new RectangularRegion(
                            xRange.Item1,
                            xRange.Item2,
                            yRange.Item1,
                            yRange.Item2
                        )
                    )
                )
                .ToList();

        public static List<RectangularRegion> Divide(int divisionsPerAxis, int width, int height) =>
            GetRegionsFromXYRanges(
                RangesForAxis(divisionsPerAxis, width),
                RangesForAxis(divisionsPerAxis, height)
            )
            .ToList();
    }
}