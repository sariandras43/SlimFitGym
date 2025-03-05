using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using SlimFitGym.EFData.Repositories;
using SlimFitGym.Models.Models;
using SlimFitGym.Models.Requests;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SlimFitGymBackend.Controllers
{
    [Route("api/purchases")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        readonly PurchasesRepository purchasesRepository;
        public PurchasesController(PurchasesRepository pR)
        {
            this.purchasesRepository = pR;
        }
        // GET: api/purchases
        [HttpGet]
        [Authorize(Roles = "admin")]

        public IActionResult Get()
        {
            return this.Execute(() =>
            {
                return Ok(purchasesRepository.GetAllPurchases());
            });
        }

        // GET api/purchases/5
        [HttpGet("{accountId}")]
        [Authorize]
        public IActionResult Get([FromRoute] string accountId)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(accountId, out idNum))
                {
                    var res = purchasesRepository.GetPurchasesByAccountId(token, idNum);
                    if (res != null)
                        return Ok(res);
                    return NotFound(new { message = "Nem található a felhasználó." });
                }
                throw new Exception("Érvénytelen azonosító.");
            });
        }

        // POST api/purchases
        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] PurchaseRequest purchase)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            return this.Execute(() =>
            {
                return Ok(purchasesRepository.NewPurchase(token, purchase));
            });
        }


    }
}
