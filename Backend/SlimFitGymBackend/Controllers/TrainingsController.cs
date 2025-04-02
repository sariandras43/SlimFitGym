using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SlimFitGym.EFData;
using SlimFitGym.EFData.Interfaces;
using SlimFitGym.EFData.Repositories;
using SlimFitGym.Models.Models;
using SlimFitGym.Models.Requests;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SlimFitGymBackend.Controllers
{
    [Route("api/trainings")]
    [ApiController]
    public class TrainingsController : ControllerBase
    {
        public readonly ITrainingsRepository trainingsRepository;
        public readonly IReservationRepository reservationRepository;

        public TrainingsController(ITrainingsRepository trainingsRepository, IReservationRepository reservationRepository)
        {
            this.trainingsRepository = trainingsRepository;
            this.reservationRepository = reservationRepository;
        }

        // GET: api/<TrainingsController>
        [HttpGet]
        public IActionResult Get()
        {
            return this.Execute(() =>
            {
                return Ok(trainingsRepository.GetActiveTrainings());
            });
        }

        [HttpGet("all")]
        [Authorize(Roles = "admin")]
        public IActionResult GetAllTrainingsIncludedTheDeletedOnes()
        {
            return this.Execute(() =>
            {
                return Ok(trainingsRepository.GetAllTrainings());
            });
        }

        [HttpGet("account/{accountId}")]
        [Authorize]
        public IActionResult GetTrainingsByAccountId(string accountId)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(accountId, out idNum))
                {
                    var res = trainingsRepository.GetTrainingsByAccountId(token, idNum);
                    if (res != null)
                        return Ok(res);
                    return NotFound(new { message = "Nem található a felhasználó." });

                }
                throw new Exception("Érvénytelen azonosító.");
            });
        }

        // GET api/<TrainingsController>/5
        [HttpGet("{id}")]
        public IActionResult GetActiveTrainingById([FromRoute]string id)
        {
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id, out idNum))
                {
                    var res = trainingsRepository.GetActiveTraningById(idNum);
                    if (res != null)
                        return Ok(res);
                    return NotFound(new { message = "Nem található az edzés." });

                }
                throw new Exception("Érvénytelen azonosító.");
            });
        }

        [HttpGet("room/{id}")]
        public IActionResult GetActiveTrainingsByRoomId([FromRoute] string id)
        {
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id, out idNum))
                {
                    var res = trainingsRepository.GetActiveTrainingsByRoomId(idNum);
                    if (res != null)
                        return Ok(res);
                    return NotFound(new { message = "Nem található a szoba." });

                }
                throw new Exception("Érvénytelen azonosító.");
            });
        }

        [HttpGet("trainer/{id}")]
        public IActionResult GetActiveTrainingsByTrainerId([FromRoute] string id)
        {
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id, out idNum))
                {
                    var res = trainingsRepository.GetActiveTrainingsByTrainerId(idNum);
                    if (res != null)
                        return Ok(res);
                    return NotFound(new { message = "Nem található az edző." });

                }
                throw new Exception("Érvénytelen azonosító.");
            });
        }

        [HttpPost]
        [Authorize(Roles = "admin,trainer")]
        public IActionResult Post([FromBody] TrainingRequest training)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            return this.Execute(() =>
            {
                return Ok(trainingsRepository.NewTraining(token, training));
            });
        }

        // PUT api/<TrainingsController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "admin,trainer")]
        public IActionResult Put([FromRoute]string id, [FromBody] dynamic training)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            return this.Execute(() =>
            {
                int idNum;
                TrainingRequest trainingToUpdate = Newtonsoft.Json.JsonConvert.DeserializeObject<TrainingRequest>(training.ToString());
                if (int.TryParse(id, out idNum))
                {
                    var res = trainingsRepository.UpdateTraining(token, idNum, trainingToUpdate);
                    if (res != null) return Ok(res);
                    return NotFound(new { message = "Nem található az edzés." });

                }
                throw new Exception("Nem érvényes azonosító.");
            });
        }

        // DELETE api/<TrainingsController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin,trainer")]
        public IActionResult Delete([FromRoute]string id)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id, out idNum))
                {
                    var res = trainingsRepository.DeleteOrMakeInactive(token, idNum);
                    if (res != null)
                        return Ok(res);
                    return NotFound(new { message = "Nem található az edzés." });

                }
                throw new Exception("Nem érvényes azonosító.");
            });
        }

        [HttpPost("signup")]
        [Authorize]
        public IActionResult SignUpForATraining([FromBody] ReservationRequest reservation)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            return this.Execute(() => {
                var res = reservationRepository.NewReservation(token,reservation);
                if (res == null)
                    return NotFound( new { message = "Nem található az edzés." });
                return  Ok(res);

            });
        }


        [HttpPost("signout")]
        [Authorize]
        public IActionResult SignOutFromATraining([FromBody] ReservationRequest reservation)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            return this.Execute(() => {
                var res = reservationRepository.DeleteReservationByTrainingAndAccountId(token, reservation.AccountId, reservation.TrainingId);
                if (res == null)
                    return NotFound(new { message = "Nem található a jelentkezés." });
                return Ok();

            });
        }

    }
}
