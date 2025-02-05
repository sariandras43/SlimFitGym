using Microsoft.AspNetCore.Mvc;
using SlimFitGym.EFData.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SlimFitGymBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsAndMachinesController : ControllerBase
    {

        readonly RoomsAndMachinesRepository roomsAndMachinesRepository;

        public RoomsAndMachinesController(RoomsAndMachinesRepository roomsAndMachinesRepository)
        {
            this.roomsAndMachinesRepository = roomsAndMachinesRepository;
        }
        // GET: api/<RoomsAndMachinesController>
        [HttpGet]
        public IActionResult Get()
        {
            return this.Execute(() =>
            {
                return Ok(roomsAndMachinesRepository.GetRoomsWithMachines());
            });
        }

        // GET api/<RoomsAndMachinesController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return this.Execute(() =>
            {
                return Ok(roomsAndMachinesRepository.GetRoomWithMachinesById(id));
            });
        }

        // POST api/<RoomsAndMachinesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<RoomsAndMachinesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RoomsAndMachinesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
