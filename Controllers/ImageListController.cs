using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JsonTaggerApi.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("[controller]")]
    public class ImageListController : ControllerBase
    {
        private readonly ILogger<ImageListController> _logger;

        private TaggerDbContext _dbContext;

        public ImageListController(ILogger<ImageListController> logger, TaggerDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _dbContext.FileRecords.Select(x => x.FilePath);
        }
    }
}
