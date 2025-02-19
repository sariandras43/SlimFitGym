using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlimFitGym.EFData;
using SlimFitGym.EFData.Repositories;
using SlimFitGym.Models.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SlimFitGymBackend.Controllers
{
    [Route("api/trainings")]
    [ApiController]
    public class TrainingsController : ControllerBase
    {
        public readonly TrainingsRepository trainingsRepository;

        public TrainingsController(TrainingsRepository trainingsRepository)
        {
            this.trainingsRepository = trainingsRepository;
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
        //[Authorize(Roles ="admin")]
        public IActionResult GetAllTrainingsIncludedTheDeletedOnes()
        {
            return this.Execute(() =>
            {
                return Ok(trainingsRepository.GetAllTrainings());
            });
        }

        [HttpGet("account/{accountId}")]
        //[Authorize]
        //TODO
        public IActionResult GetActiveTrainings(string accountId)
        {
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(accountId, out idNum))
                {
                    var res = trainingsRepository.GetTrainingById(idNum);
                    if (res != null)
                        return Ok(res);
                    return NotFound(new { message = "Nem található az edzés." });

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
        //[Authorize(Roles = "admin,trainer")]
        //TODO
        public IActionResult Post([FromBody] Training training)
        {
            return this.Execute(() =>
            {
                return Ok(trainingsRepository.NewTraining(training));
            });
        }

        // PUT api/<TrainingsController>/5
        [HttpPut("{id}")]
        //[Authorize(Roles = "admin,trainer")]
        //TODO
        public IActionResult Put([FromRoute]string id, [FromBody] Training training)
        {
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id, out idNum))
                {
                    var res = trainingsRepository.UpdateTraining(idNum, training);
                    if (res != null) return Ok(res);
                    return NotFound("Nem található az edzés.");

                }
                throw new Exception("Nem érvényes azonosító.");
            });
        }

        // DELETE api/<TrainingsController>/5
        [HttpDelete("{id}")]
        //[Authorize(Roles = "admin")]
        //TODO
        public IActionResult Delete([FromRoute]string id)
        {
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id, out idNum))
                {
                    var res = trainingsRepository.DeleteOrMakeInactive(idNum);
                    if (res != null)
                        return Ok(res);
                    return NotFound(new { message = "Nem található az edzés." });

                }
                throw new Exception("Nem érvényes azonosító.");
            });
        }

    }
}
