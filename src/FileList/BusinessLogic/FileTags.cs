using System;
using System.Collections.Generic;
using System.Linq;
using JsonTaggerApi.FileList.EFTypes;
using Newtonsoft.Json;

namespace JsonTaggerApi.FileList.BusinessLogic
{
    public static class FileTags
    {
        /// <summary>
        /// If input enumerable is empty, return empty string.
        /// Otherwise, return input strings with spaces in between.
        /// </summary>
        /// <param name="enumerable">Strings to delimit with spaces</param>
        /// <param name="func"></param>
        /// <returns>Space-delimited strings as one string.</returns>
        public static string SpaceDelimit(this IEnumerable<string> tags)
        {
            return 
                tags
                .Where(x => !String.IsNullOrWhiteSpace(x))
                .Aggregate(
                    "",
                    (x, y) =>
                        x == ""
                        ? y
                        : x + ' ' + y
                );
        }

        public static string GetSpaceDelimitedTags(IndexedFile? indexedFile) =>
            indexedFile
            ?.FileTagPairs
            ?.Select(x => x.Tag)
            ?.Where(x => !String.IsNullOrWhiteSpace(x))
            ?.ToList()
            ?.SpaceDelimit()
            ?? "";
    }
}
