using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ConnectingApps.MultiDelegatingHandler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchEngineController : ControllerBase
    {
        private readonly ISearchEngineService _searchEngineService;
        private readonly ILogger<SearchEngineController> _logger;

        public SearchEngineController(ISearchEngineService searchEngineService, ILogger<SearchEngineController> logger)
        {
            _searchEngineService = searchEngineService;
            _logger = logger;
        }

        [HttpGet("{queryEntry}", Name = "GetNumberOfCharacters")] // http://localhost:5000/api/searchengine/daan
        public async Task<ActionResult<int>> GetNumberOfCharacters(string queryEntry)
        {
            var numberOfCharacters = await _searchEngineService.GetNumberOfCharactersFromSearchQuery(queryEntry);
            return Ok(numberOfCharacters);
        }
    }
}
