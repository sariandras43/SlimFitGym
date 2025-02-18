using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlimFitGym.EFData.Repositories;
using SlimFitGym.Models.Models;
using SlimFitGym.Models.Requests;

namespace SlimFitGymBackend.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        readonly RoomsRepository roomsRepository;
        readonly RoomsAndMachinesRepository roomsAndMachinesRepository;

        public RoomsController(RoomsRepository roomsRepository, RoomsAndMachinesRepository roomsAndMachinesRepository)
        {
            this.roomsRepository = roomsRepository;
            this.roomsAndMachinesRepository = roomsAndMachinesRepository;
        }
        // GET: api/<RoomsController>
        [HttpGet]
        public IActionResult Get()
        {
            return this.Execute(() =>
            {
                return Ok(roomsAndMachinesRepository.GetRoomsWithMachines());
            });
        }

        // GET api/<RoomsController>/5
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] string id)
        {
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id,out idNum))
                {
                    var res = roomsAndMachinesRepository.GetRoomWithMachinesById(idNum);
                    if (res!=null)
                    {
                        return Ok(res);
                    }
                    return NotFound(new {message = "Szoba nem található"});
                    
                }
                throw new Exception("Nem érvényes azonosító.");
            });
        }

        // POST api/<RoomsController>
        [HttpPost]
        [Authorize(Roles = "admin")]

        public IActionResult Post([FromBody] RoomRequest value)
        {
            return this.Execute(() =>
            {
                //Room newRoom = Newtonsoft.Json.JsonConvert.DeserializeObject<Room>(value.ToString());
                return Ok(roomsRepository.NewRoom(value));
            });
        }

        // PUT api/<RoomsController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]

        public IActionResult Put([FromRoute] string id, [FromBody] RoomRequest value)
        {
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id,out idNum))
                {
                    //TODO: JSON parse error!!!
                    //Room room = Newtonsoft.Json.JsonConvert.DeserializeObject<Room>(value.ToString());
                    var res = roomsRepository.UpdateRoom(idNum, value);
                    if (res!=null)
                        return Ok(res);
                    return NotFound(new { message = "Szoba nem található" });
                }
                throw new Exception("Nem érvényes azonosító.");

            });
        }

        // DELETE api/<RoomsController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]

        public IActionResult Delete([FromRoute] string id)
        {
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id, out idNum))
                {
                    var res = roomsRepository.DeleteRoom(idNum);
                    if (res!=null)
                        return Ok(res);
                    return NotFound(new { message = "Szoba nem található" });
                }
                throw new Exception("Nem érvényes azonosító.");
            });
        }
    }
}
