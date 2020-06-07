using System;

namespace JsonTaggerApi
{
    public static class Utility
    {
        public static int? ParseInt(string? toParse)
        {
            return Int32.TryParse(toParse, out int parseResult) ? parseResult : (int?)null;
        }
    }
}
