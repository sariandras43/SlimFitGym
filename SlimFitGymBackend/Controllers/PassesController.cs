using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlimFitGym.EFData.Repositories;
using SlimFitGym.Models.Models;
using SlimFitGym.Models.Requests;
using SlimFitGym.Models.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SlimFitGymBackend.Controllers
{
    [Route("api/passes")]
    [ApiController]
    public class PassesController : ControllerBase
    {

        readonly PassesRepository passesRepository;
        public PassesController(PassesRepository passesRepository)
        {
            this.passesRepository = passesRepository;
        }

        [HttpGet]
        [Authorize(Roles ="admin")]
        public IActionResult Get()
        {
            return this.Execute(() =>
            {
                return Ok(passesRepository.GetAllPasses());
            });
        }


        // GET: api/<PassesController>/active
        [HttpGet("active")]
        public IActionResult GetAllActivePasses()
        {
            return this.Execute(() =>
            {
                return Ok(passesRepository.GetAllActivePasses());
            });
        }


        [HttpGet("{id}")]
        public IActionResult GetPassById([FromRoute] string id)
        {
            return this.Execute(() =>
            {
                int idNum;

                if (int.TryParse(id, out idNum))
                {
                    if (idNum < 0)
                        throw new Exception("Érvénytelen azonosító");

                    PassResponse? p = passesRepository.GetOnlyActivePassById(idNum);
                    if (p == null||!p.isActive)
                        return NotFound(new { message = "A keresett bérlet nem található" });
                    return Ok(p);

                }
                throw new Exception("Érvénytelen azonosító");
            });
        }




        // GET api/<PassesController>/active/5
        [HttpGet("active/{id}")]
        [Authorize(Roles = "admin")]

        public IActionResult GetActivePassById([FromRoute] string id)
        {
            return this.Execute(() =>
            {
                int idNum;

                if (int.TryParse(id, out idNum))
                {
                    if (idNum<0)
                        throw new Exception("Érvénytelen azonosító");

                    PassResponse? p = passesRepository.GetOnlyActivePassById(idNum);
                    if (p==null)
                        return NotFound(new { message = "A keresett bérlet nem található" });
                    return Ok(p);

                }
                throw new Exception("Érvénytelen azonosító");
            });
        }

        // POST api/<PassesController>
        [HttpPost]
        [Authorize(Roles = "admin")]

        public IActionResult Post([FromBody] PassRequest pass)
        {
            return this.Execute(() =>
            {
                return Ok(passesRepository.NewPass(pass));
            });



        }

        // PUT api/<PassesController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]

        public IActionResult Put([FromRoute] string id, [FromBody] PassRequest pass)
        {
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id, out idNum))
                {
                    var res = passesRepository.UpdatePass(idNum, pass);
                    if (res != null) return Ok(res);
                    return NotFound("Nem található a bérlet.");

                }
                throw new Exception("Nem érvényes azonosító.");
            });
        }

        // DELETE api/<PassesController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]

        public IActionResult Delete(string id)
        {
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id, out idNum))
                {
                    var res = passesRepository.MakePassInactive(idNum);
                    if (res != null)
                        return Ok(res);
                    return NotFound(new { message = "Nem található a bérlet." });

                }
                throw new Exception("Nem érvényes azonosító.");
            });
        }
    }
}
