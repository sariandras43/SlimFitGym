using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlimFitGym.EFData.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SlimFitGymBackend.Controllers
{
    [Route("api/entries")]
    [ApiController]
    public class EntriesController : ControllerBase
    {
        readonly EntriesRepository entriesRepository;
        public EntriesController(EntriesRepository entriesRepository)
        {
            this.entriesRepository = entriesRepository;
        }



        
        //GET api/<EntriesController>/5
        [HttpGet("{accountId}")]
        [Authorize]
        //TODO
        public IActionResult Get([FromRoute] string accountId)
        {
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(accountId, out idNum))
                {
                    var res = entriesRepository.GetEntriesByAccountId(idNum);
                    if (res != null)
                        return Ok(res);
                    return NotFound(new { message = "Nem található a felhasználó." });

                }
                throw new Exception("Érvénytelen azonosító.");
            });
        }

        // POST api/<EntriesController>/4
        [HttpPost("{accountId}")]
        [Authorize]
        //TODO
        public IActionResult Post([FromRoute] string accountId)
        {
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(accountId, out idNum))
                {
                    var res = entriesRepository.NewEntry(idNum);
                    if (res != null)
                        return Ok(res);
                    //return NotFound(new { message = "Nem található a felhasználó." });

                }
                throw new Exception("Érvénytelen azonosító.");
            });
        }


    }
}
