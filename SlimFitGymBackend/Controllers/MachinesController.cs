using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SlimFitGym.EFData.Repositories;
using SlimFitGym.Models;

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
                return Ok(machinesRepository.GetAllMachine());
            });
        }

        // GET api/<MachinesController>/5
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute]int id)
        {
            return this.Execute(() =>
            {
                //TODO: is id really an int?
                return Ok(machinesRepository.GetMachineById(id));        
            });
        }

        // POST api/<MachinesController>
        [HttpPost]
        public IActionResult Post([FromBody] dynamic value)
        {
            return this.Execute(() =>
            {
                Machine newMachine = Newtonsoft.Json.JsonConvert.DeserializeObject<Machine>(value.ToString());
                return Ok(machinesRepository.NewMachine(newMachine));
            });
        }

        // PUT api/<MachinesController>/5
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute]int id, [FromBody] dynamic value)
        {
            return this.Execute(() =>
            {
                Machine machine = Newtonsoft.Json.JsonConvert.DeserializeObject<Machine>(value.ToString());
                return Ok(machinesRepository.UpdateMachine(id,machine));
            });
        }

        // DELETE api/<MachinesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return this.Execute(() =>
            {
                return Ok(machinesRepository.DeleteMachine(id));
            });
        }
    }
}
