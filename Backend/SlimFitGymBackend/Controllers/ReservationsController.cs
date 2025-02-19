using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlimFitGym.EFData.Repositories;
using SlimFitGym.Models.Models;
using SlimFitGym.Models.Requests;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SlimFitGymBackend.Controllers
{
    [Route("api/reservations")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        readonly ReservationRepository reservationRepository;
        public ReservationsController(ReservationRepository reservationRepository)
        {
            this.reservationRepository = reservationRepository;
        }

        // GET: api/<ReservationsController>
        [HttpGet]
        public IActionResult Get()
        {
            return this.Execute(() =>
            {
                return Ok(reservationRepository.GetAllReservations());
            });
        }

        // GET api/<ReservationsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id, out idNum))
                {

                    var res = reservationRepository.GetReservationById(idNum);
                    if (res != null)
                        return Ok(res);
                    return NotFound(new { message = "Nem található a foglalás." });

                }
                throw new Exception("Érvénytelen azonosító.");
            });
        }

        // POST api/<ReservationsController>
        [HttpPost]
        //[Authorize]
        //TODO

        public IActionResult Post([FromBody] ReservationRequest reservation)
        {
            return this.Execute(() =>
            {
                return Ok(reservationRepository.NewReservation(reservation));
            });
        }


        // DELETE api/<ReservationsController>/5
        [HttpDelete("{id}")]
        [Authorize]
        //TODO
        public IActionResult Delete(string id)
        {
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id, out idNum))
                {

                    var res = reservationRepository.DeleteReservation(idNum);
                    if (res != null)
                        return Ok(res);
                    return NotFound(new { message = "Nem található a foglalás." });

                }
                throw new Exception("Érvénytelen azonosító.");
            });
        }
    }
}
