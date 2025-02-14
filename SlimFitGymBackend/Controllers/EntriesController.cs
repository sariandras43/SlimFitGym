using Microsoft.AspNetCore.Mvc;
using SlimFitGym.EFData.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SlimFitGymBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntriesController : ControllerBase
    {
        readonly EntriesRepository entriesRepository;
        public EntriesController(EntriesRepository entriesRepository)
        {
            this.entriesRepository = entriesRepository;
        }

        // GET: api/<EntriesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<EntriesController>/5
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] string accountId)
        {
            entriesRepository.GetEntriesByAccountId()
        }

        // POST api/<EntriesController>
        [HttpPost]
        public void Post([FromRoute] string accountId)
        {
        }


    }
}
