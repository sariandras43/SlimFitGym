using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using SlimFitGym.EFData.Repositories;
using SlimFitGym.Models.Requests;
using SlimFitGym.Models.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SlimFitGymBackend.Controllers
{
    [Route("/api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly AccountRepository accountRepository;
        public AuthController(AccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginInfo)
        {
            return this.Execute(() =>
            {
                AccountResponse? account = accountRepository.Login(loginInfo);
                if (account == null)
                    return NotFound(new {message="Nem található a felhasználó." });
                return Ok(account);
            });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegistrationRequest registration)
        {
            return this.Execute(() =>
            {
                return Ok(accountRepository.Register(registration));
            });
        }

        [HttpPut("modify/{id}")]
        [Authorize]
        public IActionResult Modify([FromRoute] string id, [FromBody] ModifyAccountRequest accountInfo)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id, out idNum))
                {
                    var res = accountRepository.UpdateAccountPublic(token,idNum, accountInfo);
                    if (res != null) return Ok(res);
                    return NotFound("Nem található a felhasználó.");

                }
                throw new Exception("Érvénytelen azonosító.");
            });
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public IActionResult Delete([FromRoute] string id)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id, out idNum))
                {
                    var res = accountRepository.DeleteAccount(token,idNum);
                    if (res != null)
                        return Ok(res);
                    return NotFound(new { message = "Nem található a felhasználó.." });

                }
                throw new Exception("Nem érvényes azonosító.");
            });
        }
    }
}
