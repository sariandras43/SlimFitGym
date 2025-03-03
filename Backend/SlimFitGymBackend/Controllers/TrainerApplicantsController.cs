using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using SlimFitGym.EFData.Repositories;
using SlimFitGym.Models.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SlimFitGymBackend.Controllers
{
    [Route("api/applicants")]
    [ApiController]
    public class TrainerApplicantsController : ControllerBase
    {
        readonly TrainerApplicantsRepository trainerApplicantsRepository;
        public TrainerApplicantsController(TrainerApplicantsRepository trainerApplicantsRepository)
        {
            this.trainerApplicantsRepository = trainerApplicantsRepository;
        }

        // GET: api/<TrainerApplicantsController>
        [HttpGet]
        [Authorize(Roles = "admin")]

        public IActionResult Get()
        {
            return this.Execute(() =>
            {
                return Ok(trainerApplicantsRepository.GetAllApplicants());
            });
        }

        // GET api/<TrainerApplicantsController>/5
        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]

        public IActionResult Get(string id)
        {
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id, out idNum))
                {
                    var res = trainerApplicantsRepository.GetApplicantById(idNum);
                    if (res != null)
                        return Ok(res);
                    return NotFound(new { message = "Nem található a jelentkezés." });

                }
                throw new Exception("Érvénytelen azonosító.");
            });
        }

        // POST api/<TrainerApplicantsController>
        [HttpPost("accept/{id}")]
        [Authorize(Roles = "admin")]

        public IActionResult Accept([FromRoute] string id)
        {
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id, out idNum))
                {
                    var res = trainerApplicantsRepository.AcceptAsTrainer(idNum);
                    if (res != null)
                        return Ok(res);
                    return NotFound(new { message = "Nem található a jelentkező." });

                }
                throw new Exception("Érvénytelen azonosító.");
            });
        }

        [HttpPost]
        [Authorize]

        public IActionResult Post([FromBody] TrainerApplicant applicant)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            return this.Execute(() =>
            {
                return Ok(trainerApplicantsRepository.NewApplicant(token, applicant));
            });
        }

        // DELETE api/<TrainerApplicantsController>/5
        [HttpDelete("reject/{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(string id)
        {
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id, out idNum))
                {
                    var res = trainerApplicantsRepository.Reject(idNum);
                    if (res != null)
                        return Ok(res);
                    return NotFound(new { message = "Nem található a jelentkező." });

                }
                throw new Exception("Érvénytelen azonosító.");
            });
        }
    }
}
