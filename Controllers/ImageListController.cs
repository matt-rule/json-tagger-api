using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using JsonTaggerApi.Model;
using JsonTaggerApi.Model.BusinessLogic;
using JsonTaggerApi.Model.Json;

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

        [HttpGet]
        public string Get()
        {
            return
                _dbContext
                .FileRecords
                .Take(ITEMS_PER_PAGE)
                .AsEnumerable()
                .Select(x => (
                    new ImageListWebResult {
                        origFilePath = x.OriginalFilePath,
                        thumb =
                            Thumbnails.GetThumbnailFilePath(
                                Path.GetFileNameWithoutExtension(x.GuidFilePath)
                            )
                            ?? throw new InvalidDataException("Invalid GUID from file record table in DB.")
                    }
                ))
                .ToJson();
        }
    }
}
