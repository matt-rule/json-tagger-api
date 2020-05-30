using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using JsonTaggerApi.Model;
using JsonTaggerApi.Model.BusinessLogic;
using JsonTaggerApi.Model.Json;
using JsonTaggerApi.Model.EntityFrameworkModels;

namespace JsonTaggerApi.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("[controller]")]
    public class ImageListController : ControllerBase
    {
        const int ITEMS_PER_PAGE = 20;

        private readonly ILogger<ImageListController> _logger;

        private TaggerDbContext _dbContext;

        public ImageListController(ILogger<ImageListController> logger, TaggerDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet("{query}/{page}")]
        public string Get(string query, string page)
        {
            IEnumerable<string> searchTagsUnsorted = query.Split(' ').Where(x => !String.IsNullOrWhiteSpace(x));
            IEnumerable<string> searchTagsToInclude = searchTagsUnsorted.Where(x => !x.StartsWith('-'));
            IEnumerable<string> searchTagsToExclude = searchTagsUnsorted.Where(x => x.StartsWith('-')).Select(x => x.Substring(1));

            Func<IEnumerable<string>, IEnumerable<IQueryable<int>>> getFileIdsPerSearchSubStr = (IEnumerable<string> searchPortions) =>
                searchPortions.Select(s => _dbContext.FileTagPairs.Where(pair => pair.Tag.Contains(s)).Select(x => x.FileId));

            var fileIdsMatchingIncludeCriteria = getFileIdsPerSearchSubStr(searchTagsToInclude);
            var fileIdsMatchingExcludeCriteria = getFileIdsPerSearchSubStr(searchTagsToExclude);

            var includesQuery =
                (searchTagsToInclude.Count() == 0)
                ?
                    _dbContext.FileRecords.AsQueryable()
                :
                    _dbContext
                    .FileRecords
                    .Where(
                        fileRec => fileIdsMatchingIncludeCriteria.All(idSeq => idSeq.Contains(fileRec.Id))
                    );

            var excludesQuery =
                includesQuery
                .Where(
                    fileRec => !fileIdsMatchingExcludeCriteria.Any(idSeq => idSeq.Contains(fileRec.Id))
                );

            return
                excludesQuery
                .Take(ITEMS_PER_PAGE)
                .AsEnumerable()
                .Select(x => (
                    new ImageListWebResult {
                        origFilePath = x.OriginalFilePath,
                        guid = Path.GetFileNameWithoutExtension(x.GuidFilePath)
                    }
                ))
                .ToJson();
        }
    }
}
