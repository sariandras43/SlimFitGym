using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlimFitGym.EFData.Repositories;
using SlimFitGym.Models;

namespace SlimFitGymBackend.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        readonly RoomsRepository roomsRepository;

        public RoomsController(RoomsRepository roomsRepository)
        {
            this.roomsRepository = roomsRepository;
        }
        // GET: api/<RoomsController>
        [HttpGet]
        public IActionResult Get()
        {
            return this.Execute(() =>
            {
                return Ok(roomsRepository.GetAllRooms());
            });
        }

        // GET api/<RoomsController>/5
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            return this.Execute(() =>
            {
                return Ok(roomsRepository.GetRoomById(id));
            });
        }

        // POST api/<RoomsController>
        [HttpPost]
        public IActionResult Post([FromBody] Room value)
        {
            return this.Execute(() =>
            {
                //Room newRoom = Newtonsoft.Json.JsonConvert.DeserializeObject<Room>(value.ToString());
                return Ok(roomsRepository.NewRoom(value));
            });
        }

        // PUT api/<RoomsController>/5
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody] dynamic value)
        {
            return this.Execute(() =>
            {
                //TODO: JSON parse error!!!
                Room room = Newtonsoft.Json.JsonConvert.DeserializeObject<Room>(value.ToString());
                return Ok(roomsRepository.UpdateRoom(id, room));

            });
        }

        // DELETE api/<RoomsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return this.Execute(() =>
            {
                return Ok(roomsRepository.DeleteRoom(id));
            });
        }
    }
}
