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
        public IActionResult Get([FromRoute] string id)
        {
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id,out idNum))
                {
                    var res = roomsAndMachinesRepository.GetRoomAndMachineConnectionById(idNum);
                    if (res!=null)
                        return Ok(res);
                    return NotFound(new { message = "Nem található terem-gép kapcsolat." });
                }throw new Exception("Nem érvényes azonosító.");
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
        public IActionResult Put([FromRoute]string id, [FromBody] RoomAndMachine value)
        {
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id, out idNum))
                {
                    var res = roomsAndMachinesRepository.UpdateRoomAndMachineConnection(idNum,value);
                    if (res != null)
                        return Ok(res);
                    return NotFound(new { message = "Nem található terem-gép kapcsolat." });
                }
                throw new Exception("Nem érvényes azonosító.");
            });
        }

        // DELETE api/<RoomsAndMachinesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id, out idNum))
                {
                    var res = roomsAndMachinesRepository.DeleteConnection(idNum);
                    if (res != null)
                        return Ok(res);
                    return NotFound(new { message = "Nem található terem-gép kapcsolat." });
                }
                throw new Exception("Nem érvényes azonosító.");
            });
        }
    }
}
