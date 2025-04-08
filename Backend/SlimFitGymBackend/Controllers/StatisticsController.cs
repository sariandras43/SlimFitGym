using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlimFitGym.EFData.Interfaces;

namespace SlimFitGymBackend.Controllers
{
    [Route("api/statistics")]
    [Authorize(Roles = "admin")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsRepository statisticsRepository;
        public StatisticsController(IStatisticsRepository statisticsRepository)
        {
            this.statisticsRepository = statisticsRepository;
        }

        [HttpGet("purchases")]
        public IActionResult GetPurchasesStatistics([FromQuery] string year) 
        {
            return this.Execute(() =>
            {
                if (int.TryParse(year,out int yearNum))
                {
                    return Ok(statisticsRepository.PurchasesAndIncomePerMonth(yearNum));
                }
                return BadRequest(new {message = "Érvénytelen évszám." });
            });
        }

        [HttpGet("entries")]
        public IActionResult GetEntriesStatistics([FromQuery] string year)
        {
            return this.Execute(() =>
            {
                if (int.TryParse(year, out int yearNum))
                {
                    return Ok(statisticsRepository.EntriesPerMonth(yearNum));
                }
                return BadRequest(new { message = "Érvénytelen évszám." });
            });
        }
    }
}
