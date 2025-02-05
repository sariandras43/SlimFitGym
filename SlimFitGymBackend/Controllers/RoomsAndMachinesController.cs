using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SlimFitGym.EFData.Repositories;
using SlimFitGym.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SlimFitGymBackend.Controllers
{
    [Route("api/roomsandmachines")]
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
                return Ok(roomsAndMachinesRepository.GetRoomAndMachineConnections());
            });
        }

        // GET api/<RoomsAndMachinesController>/5
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            return this.Execute(() =>
            {
                return Ok(roomsAndMachinesRepository.GetRoomAndMachineConnectionById(id));
            });
        }

        // POST api/<RoomsAndMachinesController>
        [HttpPost]
        public IActionResult Post([FromBody] RoomAndMachine value)
        {
            return this.Execute(() =>
            {
                return Ok(roomsAndMachinesRepository.ConnectRoomAndMachine(value));
            });
        }

        // PUT api/<RoomsAndMachinesController>/5
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute]int id, [FromBody] RoomAndMachine value)
        {
            return this.Execute(() =>
            {
                return Ok(roomsAndMachinesRepository.UpdateRoomAndMachineConnection(id,value));
            });
        }

        // DELETE api/<RoomsAndMachinesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            return this.Execute(() =>
            {
                return Ok(roomsAndMachinesRepository.DeleteConnection(id));
            });
        }
    }
}
