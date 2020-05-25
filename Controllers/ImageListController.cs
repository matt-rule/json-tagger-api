using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using JsonTaggerApi.Types.JsonSerialisable;

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
                JsonConvert.SerializeObject(
                _dbContext
                    .FileRecords
                    .Take(ITEMS_PER_PAGE)
                    .AsEnumerable()
                    .Select(x => (
                        new ImageListWebResult {
                            origFilePath = x.OriginalFilePath,
                            thumb =
                                BusinessLogic.ImageProcessing.GetThumbFileName(
                                    Path.GetFileNameWithoutExtension(x.GuidFilePath)
                                )
                        }
                    ))
                );
        }
    }
}
