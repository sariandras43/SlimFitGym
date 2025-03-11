using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SlimFitGym.EFData.Interfaces;
using SlimFitGym.EFData.Repositories;
using SlimFitGym.Models.Models;
using SlimFitGym.Models.Requests;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SlimFitGymBackend.Controllers
{
    [Route("api/machines")]
    [ApiController]
    public class MachinesController : ControllerBase
    {

        readonly IMachinesRepository machinesRepository;

        public MachinesController(IMachinesRepository machinesRepository)
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
                    return NotFound(new {message="Nem található a gép." });
                    
                }throw new Exception("Érvénytelen azonosító.");
            });
        }

        // POST api/<MachinesController>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Post([FromBody] MachineRequest newMachine)
        {
            return this.Execute(() =>
            {
                return Ok(machinesRepository.NewMachine(newMachine));
            });
        }

        // PUT api/<MachinesController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult Put([FromRoute]string id, MachineRequest machine)
        {
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id,out idNum))
                {
                    var res = machinesRepository.UpdateMachine(idNum, machine);
                    if (res!=null) return Ok(res);
                    return NotFound("Nem található a gép.");
                    
                }throw new Exception("Érvénytelen azonosító.");
            });
        }

        // DELETE api/<MachinesController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
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
                    return NotFound(new { message = "Nem található a gép." });

                }
                throw new Exception("Érvénytelen azonosító.");
            });
        }
    }
}
