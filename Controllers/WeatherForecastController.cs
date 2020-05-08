using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JsonTaggerApi.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("[controller]")]
    public class TagListController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<TagListController> _logger;

        private TaggerDbContext _dbContext;

        public TagListController(ILogger<TagListController> logger, TaggerDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _dbContext.FileTagPairs.Select(x => x.Tag).Distinct();
        }
    }
}
