using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using JsonTaggerApi.FileList.ViewModels;

namespace JsonTaggerApi.FileList.BusinessLogic
{
    public static class Search
    {
        const int ITEMS_PER_PAGE = 20;
        
        public static ImmutableList<int> FileIdsMatchingCriteria(TaggerDbContext dbContext, ImmutableList<string> terms, Func<IEnumerable<ImmutableList<int>>, Func<ImmutableList<int>, bool>, bool> anyOrAll)
        {
            ImmutableList<ImmutableList<int>> fileIdsPerTerm =
                terms.Select
                    (s =>
                        (
                            from pair in dbContext.FileTagPairs
                            where EF.Functions.Like(pair.Tag, $"%{s}%")
                            select pair.FileRecordId
                        )
                        .ToImmutableList()
                    ).ToImmutableList();

            return fileIdsPerTerm
                .SelectMany(x => x)
                .Distinct()
                .Where(x => anyOrAll(fileIdsPerTerm, y => y.Contains(x)))
                .ToImmutableList();
        }

        public static string Run(TaggerDbContext dbContext, ProcessedInput userQuery)
        {
            var idsMatchingIncludeCriteria = FileIdsMatchingCriteria(dbContext, userQuery.IncludeTerms, Enumerable.All);
            var idsMatchingExcludeCriteria = FileIdsMatchingCriteria(dbContext, userQuery.ExcludeTerms, Enumerable.Any);

            return
                dbContext.FileRecords
                .Where(fileRec => userQuery.IncludeTerms.Count == 0 ? true : idsMatchingIncludeCriteria.Contains(fileRec.Id))
                .Where(fileRec => !idsMatchingExcludeCriteria.Contains(fileRec.Id))
                .Take(ITEMS_PER_PAGE)
                .AsEnumerable()
                .Select(x => (
                    new FileInfoItem {
                        origFilePath = x.OriginalFilePath,
                        guid = Path.GetFileNameWithoutExtension(x.GuidFilePath)
                    }
                ))
                .ToImmutableList()
                .ToJson();
        }
    }
}
