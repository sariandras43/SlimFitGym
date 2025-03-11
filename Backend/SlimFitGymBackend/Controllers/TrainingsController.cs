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

        //TODO: something with the url, cause its messy
        //[HttpGet("/search/{limit}&{offset}.{trainingName?}")]
        //public IActionResult Filter([FromRoute] string? trainingName="" ,[FromRoute] string? limit ="1" ,[FromRoute] string? offset = "1")
        //{
        //    return this.Execute(() =>
        //    {
        //        int limitNum;
        //        int offsetNum;

        //        if (trainingName.StartsWith("%20"))
        //            throw new Exception("Érvénytelen paraméterek.");
        //        if (!int.TryParse(limit,out limitNum))
        //            throw new Exception("Érvénytelen paraméterek.");
        //        if (!int.TryParse(offset, out offsetNum))
        //            throw new Exception("Érvénytelen paraméterek.");
        //        List<Training> res = trainingsRepository.FilterTrainings(trainingName.Trim(),limitNum,offsetNum);
        //        if (res == null||res.Count==0)
        //            return NotFound(new { message = "Nem találhatók edzések ilyen paraméterek mellett." });
        //        return Ok(res);

        //    });
        //}


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
        public IActionResult Put([FromRoute]string id, [FromBody] TrainingRequest training)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id, out idNum))
                {
                    var res = trainingsRepository.UpdateTraining(token, idNum, training);
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
