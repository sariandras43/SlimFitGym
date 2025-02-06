using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SlimFitGym.EFData.Repositories;
using SlimFitGym.Models.Models;
using SlimFitGym.Models.Requests;

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
        public IActionResult Post([FromBody] RoomAndMachineRequest value)
        {
            return this.Execute(() =>
            {
                var roomAndMachine = new RoomAndMachine() { Id=value.Id,MachineId=value.MachineId,RoomId=value.RoomId,MachineCount=value.MachineCount};
                return Ok(roomsAndMachinesRepository.ConnectRoomAndMachine(roomAndMachine));
            });
        }

        // PUT api/<RoomsAndMachinesController>/5
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute]string id, [FromBody] RoomAndMachineRequest value)
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
