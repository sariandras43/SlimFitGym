using Microsoft.AspNetCore.Mvc;
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
        public TrainerApplicantsController(TrainerApplicantsRepository taR)
        {
            trainerApplicantsRepository = taR;
        }

        // GET: api/<TrainerApplicantsController>
        [HttpGet]
        public IActionResult Get()
        {
            return this.Execute(() =>
            {
                return Ok(trainerApplicantsRepository.GetAllApplicants());
            });
        }

        // GET api/<TrainerApplicantsController>/5
        [HttpGet("{id}")]
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
                throw new Exception("Nem érvényes azonosító.");
            });
        }

        // POST api/<TrainerApplicantsController>
        [HttpPost("/accept/{id}")]
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
                throw new Exception("Nem érvényes azonosító.");
            });
        }

        [HttpPost()]
        public IActionResult Post([FromBody] TrainerApplicant applicant)
        {
            return this.Execute(() =>
            {
                return Ok(trainerApplicantsRepository.NewApplicant(applicant));
            });
        }

        // DELETE api/<TrainerApplicantsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
