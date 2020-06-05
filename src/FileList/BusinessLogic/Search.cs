using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace JsonTaggerApi.FileList.BusinessLogic
{
    public static class Search
    {
        const int ITEMS_PER_PAGE = 20;
        
        public static string Run(TaggerDbContext dbContext, ProcessedInput userQuery)
        {
            ImmutableList<ImmutableList<int>> fileIdsPerIncludeString =
                userQuery.IncludeTerms.Select
                    (s =>
                        (
                            from pair in dbContext.FileTagPairs
                            where EF.Functions.Like(pair.Tag, $"%{s}%")
                            select pair.FileRecordId
                        )
                        .ToImmutableList()
                    ).ToImmutableList();

            ImmutableList<int> idsPassingIncludeCriteria =
                fileIdsPerIncludeString
                .SelectMany(x => x)
                .Distinct()
                .Where(x => fileIdsPerIncludeString.All(y => y.Contains(x)))
                .ToImmutableList();

            ImmutableList<ImmutableList<int>> fileIdsPerExcludeString =
                userQuery.ExcludeTerms.Select
                    (s =>
                        (
                            from pair in dbContext.FileTagPairs
                            where EF.Functions.Like(pair.Tag, $"%{s}%")
                            select pair.FileRecordId
                        )
                        .ToImmutableList()
                    ).ToImmutableList();

            ImmutableList<int> idsMatchingExcludeCriteria =
                fileIdsPerExcludeString
                .SelectMany(x => x)
                .Distinct()
                .Where(x => fileIdsPerExcludeString.Any(y => y.Contains(x)))
                .ToImmutableList();

            return
                dbContext.FileRecords
                .Where(fileRec => userQuery.IncludeTerms.Count == 0 ? true : idsPassingIncludeCriteria.Contains(fileRec.Id))
                .Where(fileRec => !idsMatchingExcludeCriteria.Contains(fileRec.Id))
                .Take(ITEMS_PER_PAGE)
                .AsEnumerable()
                .Select(x => (
                    new ImageListWebResult {
                        origFilePath = x.OriginalFilePath,
                        guid = Path.GetFileNameWithoutExtension(x.GuidFilePath)
                    }
                ))
                .ToImmutableList()
                .ToJson();
        }
    }
}
