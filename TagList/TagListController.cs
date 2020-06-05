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
    public class TagListController : ControllerBase
    {
        private readonly ILogger<TagListController> _logger;

        private TaggerDbContext _dbContext;

        public TagListController(ILogger<TagListController> logger, TaggerDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        public List<string> Get()
        {
            return _dbContext.FileTagPairs.Select(x => x.Tag).Distinct().ToList();
        }
    }
}
