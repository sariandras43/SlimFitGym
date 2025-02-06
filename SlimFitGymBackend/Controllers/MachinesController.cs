using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SlimFitGym.EFData.Repositories;
using SlimFitGym.Models.Models;

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
        public IActionResult Get([FromRoute]string id)
        {
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id,out idNum))
                {
                    var res = machinesRepository.GetMachineById(idNum);
                    if (res!=null)
                        return Ok(res);        
                    return NotFound(new {message="Nem található a gép" });
                    
                }throw new Exception("Nem érvényes azonosító.");
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
        public IActionResult Put([FromRoute]string id, [FromBody] dynamic value)
        {
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id,out idNum))
                {
                    Machine machine = Newtonsoft.Json.JsonConvert.DeserializeObject<Machine>(value.ToString());
                    var res = machinesRepository.UpdateMachine(idNum, machine);
                    if (res!=null) return Ok(res);
                    return NotFound("Nem található a gép");
                    
                }throw new Exception("Nem érvényes azonosító.");
            });
        }

        // DELETE api/<MachinesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id, out idNum))
                {
                    var res = machinesRepository.DeleteMachine(idNum);
                    if (res != null)
                        return Ok(res);
                    return NotFound(new { message = "Nem található a gép" });

                }
                throw new Exception("Nem érvényes azonosító.");
            });
        }
    }
}
