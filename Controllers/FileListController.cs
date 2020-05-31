using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using JsonTaggerApi.Model;
using JsonTaggerApi.Model.Json;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace JsonTaggerApi.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("[controller]")]
    public class FileListController : ControllerBase
    {
        const int ITEMS_PER_PAGE = 20;

        private readonly ILogger<FileListController> _logger;

        private TaggerDbContext _dbContext;

        public FileListController(ILogger<FileListController> logger, TaggerDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        public string Get(string? query, string? page)
        {
            // // TODO: Change all Lists to IEnumerables before committing a working version
            List<string> searchTagsUnsorted = (query?.Split(' ')?.Where(x => !String.IsNullOrWhiteSpace(x)) ?? new List<string>()).ToList();
            List<string> stringsToInclude = searchTagsUnsorted.Where(x => !x.StartsWith('-')).ToList();
            List<string> stringsToExclude = searchTagsUnsorted.Where(x => x.StartsWith('-')).Select(x => x.Substring(1)).Where(x => !String.IsNullOrWhiteSpace(x)).ToList();

            List<List<int>> fileIdsPerIncludeString =
                stringsToInclude.Select
                    (s =>
                        (
                            from pair in _dbContext.FileTagPairs
                            where EF.Functions.Like(pair.Tag, $"%{s}%")
                            select pair.FileRecordId
                        )
                        .ToList()
                    ).ToList();

            List<int> idsPassingIncludeCriteria =
                fileIdsPerIncludeString
                .SelectMany(x => x)
                .Distinct()
                .Where(x => fileIdsPerIncludeString.All(y => y.Contains(x)))
                .ToList();

            List<List<int>> fileIdsPerExcludeString =
                stringsToExclude.Select
                    (s =>
                        (
                            from pair in _dbContext.FileTagPairs
                            where EF.Functions.Like(pair.Tag, $"%{s}%")
                            select pair.FileRecordId
                        )
                        .ToList()
                    ).ToList();

            List<int> idsMatchingExcludeCriteria =
                fileIdsPerExcludeString
                .SelectMany(x => x)
                .Distinct()
                .Where(x => fileIdsPerExcludeString.Any(y => y.Contains(x)))
                .ToList();

            return
                _dbContext.FileRecords
                .Where(fileRec => stringsToInclude.Count == 0 ? true : idsPassingIncludeCriteria.Contains(fileRec.Id))
                .Where(fileRec => !idsMatchingExcludeCriteria.Contains(fileRec.Id))
                .Take(ITEMS_PER_PAGE)
                .AsEnumerable()
                .Select(x => (
                    new ImageListWebResult {
                        origFilePath = x.OriginalFilePath,
                        guid = Path.GetFileNameWithoutExtension(x.GuidFilePath)
                    }
                ))
                .ToList()
                .ToJson();
        }
    }
}
