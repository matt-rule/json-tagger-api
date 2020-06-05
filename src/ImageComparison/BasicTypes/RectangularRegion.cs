namespace JsonTaggerApi.ImageComparison.BasicTypes
{
    /// <summary>
    /// Holds the extents (inclusive minimum and maximum X and Y) of a rectangular region.
    /// </summary>
    public struct RectangularRegion
    {
        public int X1;
        public int X2;
        public int Y1;
        public int Y2;

        public RectangularRegion(int x1, int x2, int y1, int y2)
        {
            X1 = x1;
            X2 = x2;
            Y1 = y1;
            Y2 = y2;
        }
    }
}
