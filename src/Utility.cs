using System;

namespace JsonTaggerApi
{
    public static class Utility
    {
        public static int ParseIntDefaultZero(string? toParse)
        {
            int parseResult = 0;
            bool parseSuccess = Int32.TryParse(toParse, out parseResult);
            return parseResult;
        }
    }
}
