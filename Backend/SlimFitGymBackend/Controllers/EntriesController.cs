﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SlimFitGym.EFData.Interfaces;
using SlimFitGym.EFData.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SlimFitGymBackend.Controllers
{
    [Route("api/entries")]
    [ApiController]
    public class EntriesController : ControllerBase
    {
        readonly IEntriesRepository entriesRepository;
        public EntriesController(IEntriesRepository entriesRepository)
        {
            this.entriesRepository = entriesRepository;
        }



        
        //GET api/<EntriesController>/5
        [HttpGet("{accountId}")]
        [Authorize]
        public IActionResult Get([FromRoute] string accountId, [FromQuery] string limit="10", [FromQuery] string offset="0", [FromQuery] string orderDirection="desc")
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            return this.Execute(() =>
            {
                int idNum;
                int limitNum;
                int offsetNum;
                if (int.TryParse(accountId, out idNum) && int.TryParse(limit,out limitNum) && int.TryParse(offset, out offsetNum))
                {
                    var res = entriesRepository.GetEntriesByAccountId(token,idNum, "2025.01.01", limitNum, offsetNum, orderDirection);
                    if (res != null)
                    {
                        Response.Headers.Add("X-Total-Count", entriesRepository.GetEntriesCountByUserId(token, idNum).ToString());
                        return Ok(res);

                    }
                    return NotFound(new { message = "Nem található a felhasználó." });

                }
                throw new Exception("Érvénytelen azonosító vagy paraméter.");
            });
        }

        [HttpGet]
        [Authorize(Roles = "admin,employee")]
        public IActionResult GetAll( [FromQuery] string limit = "10", [FromQuery] string offset = "0", [FromQuery] string orderField = "date",[FromQuery] string orderDirection = "desc")
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            return this.Execute(() =>
            {
                int limitNum;
                int offsetNum;
                if (int.TryParse(limit, out limitNum) && int.TryParse(offset, out offsetNum))
                {
                    var res = entriesRepository.GetAllEntries("2025.01.01", limitNum, offsetNum,orderField ,orderDirection);
                    Response.Headers.Add("X-Total-Count", entriesRepository.GetAllEntriesCount().ToString());
                    return Ok(res);
                }
                throw new Exception("Érvénytelen azonosító vagy paraméter.");
            });
        }

        // POST api/<EntriesController>/4
        [HttpPost("{accountId}")]
        [Authorize(Roles ="admin,employee")]
        public IActionResult Post([FromRoute] string accountId)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(accountId, out idNum))
                {
                    var res = entriesRepository.NewEntry(token, idNum);
                    if (res != null)
                        return Ok(res);

                }
                throw new Exception("Érvénytelen azonosító.");
            });
        }


    }
}
