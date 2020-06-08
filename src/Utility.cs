using System;
using Newtonsoft.Json;

namespace JsonTaggerApi
{
    public static class Utility
    {
        public static int? ParseInt(string? toParse) => 
            Int32.TryParse(toParse, out int parseResult) ? parseResult : (int?)null;

        public static string FormatJson(string json) =>
            JsonConvert.SerializeObject(
                JsonConvert.DeserializeObject(json),
                Formatting.Indented
            );
    }
}
