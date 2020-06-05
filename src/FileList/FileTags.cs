using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonTaggerApi.Model.BusinessLogic
{
    public static class FileTags
    {
        public static string ConvertToSpaceDelimitedString(IEnumerable<string> tags) =>
            tags
            .Where(x => !String.IsNullOrWhiteSpace(x))
            .Aggregate(
                "",
                (x, y) =>
                    x == ""
                    ? y
                    : x + ' ' + y
            );

        public static string ConvertToJson(IEnumerable<string> tags) =>
            "{\"tags\":"
            + JsonConvert.ToString(
                ConvertToSpaceDelimitedString(tags)
            )
            + "}";
    }
}
