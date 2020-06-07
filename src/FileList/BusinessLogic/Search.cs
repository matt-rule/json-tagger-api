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
        
        private static ImmutableList<int> FileIdsMatchingCriteria(TaggerDbContext dbContext, ImmutableList<string> terms, Func<IEnumerable<ImmutableList<int>>, Func<ImmutableList<int>, bool>, bool> anyOrAll)
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

        private static IQueryable<EFTypes.IndexedFile> Filter(TaggerDbContext dbContext, ProcessedInput userQuery)
        {
            var idsMatchingIncludeCriteria = FileIdsMatchingCriteria(dbContext, userQuery.IncludeTerms, Enumerable.All);
            var idsMatchingExcludeCriteria = FileIdsMatchingCriteria(dbContext, userQuery.ExcludeTerms, Enumerable.Any);

            return dbContext.FileRecords
                .Where(fileRec => userQuery.IncludeTerms.Count == 0 ? true : idsMatchingIncludeCriteria.Contains(fileRec.Id))
                .Where(fileRec => !idsMatchingExcludeCriteria.Contains(fileRec.Id));
        }

        public static IEnumerable<EFTypes.IndexedFile> GetPage(TaggerDbContext dbContext, ProcessedInput userQuery)
        {

            var page = Math.Max(1, userQuery.Page);

            return
                Filter(dbContext, userQuery)
                .Skip(ITEMS_PER_PAGE * (page - 1))
                .Take(ITEMS_PER_PAGE)
                .AsEnumerable();
        }
    }
}
