using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using JsonTaggerApi.FileList.BusinessLogic;

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
            return Search.Run(_dbContext, new ProcessedInput(query, page));
        }
    }
}
