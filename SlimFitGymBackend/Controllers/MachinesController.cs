using Microsoft.AspNetCore.Mvc;
using SlimFitGym.EFData.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SlimFitGymBackend.Controllers
{
    [Route("api/machines")]
    [ApiController]
    public class MachinesController : ControllerBase
    {

        readonly MachinesRepository machinesRepository;

        public MachinesController(MachinesRepository machinesRepository)
        {
            this.machinesRepository = machinesRepository;
        }
        // GET: api/<MachinesController>
        [HttpGet]
        public IActionResult Get()
        {
            return this.Execute(() =>
            {
                return Ok(machinesRepository.GetAllMenuItems());
            });
        }

        // GET api/<MachinesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MachinesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<MachinesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MachinesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
