using System.IO;
using System.Linq;
using JsonTaggerApi.FileList.BusinessLogic;
using JsonTaggerApi.FileList.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JsonTaggerApi.FileList
{
    [EnableCors]
    [ApiController]
    [Route("[controller]")]
    public class FileListController : ControllerBase
    {
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
            return Search.GetPage(_dbContext, new ProcessedInput(query, page))
                .Select(x => (
                    new FileInfoItem {
                        origFilePath = x.OriginalFilePath,
                        guid = Path.GetFileNameWithoutExtension(x.GuidFilePath)
                    }
                ))
                .ToJson();
        }
    }
}
