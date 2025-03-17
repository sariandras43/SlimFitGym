using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using SlimFitGym.EFData.Interfaces;
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
        readonly IAccountRepository accountRepository;
        public AuthController(IAccountRepository accountRepository)
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
                    return NotFound(new { message = "Nem található a felhasználó." });
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
        public IActionResult Modify([FromRoute] string id, [FromBody] dynamic accountInfo)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var account = Newtonsoft.Json.JsonConvert.DeserializeObject<ModifyAccountRequest>(accountInfo.ToString());
            return this.Execute(() =>
            {
                int idNum;
                if (int.TryParse(id, out idNum))
                {
                    var res = accountRepository.UpdateAccountPublic(token, idNum, account);
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
                    var res = accountRepository.DeleteAccount(token, idNum);
                    if (res != null)
                        return Ok(res);
                    return NotFound(new { message = "Nem található a felhasználó.." });

                }
                throw new Exception("Nem érvényes azonosító.");
            });
        }
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetLoggedInUser()
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            return this.Execute(() =>
            {

                var res = accountRepository.GetLoggedInAccountFromToken(token);
                if (res != null) return Ok(res);
                return NotFound("Nem található a felhasználó.");

                throw new Exception("Érvénytelen azonosító.");
            });
        }
        [HttpGet("accounts/all")]
        [Authorize(Roles = "admin")]
        public IActionResult GetAllUsers()
        {
            return this.Execute(() =>
            {
                var res = accountRepository.GetAllAccounts(false);
                return Ok(res);
            });
        }

        [HttpGet("accounts/active")]
        [Authorize(Roles = "admin")]
        public IActionResult GetAllActiveUser()
        {
            return this.Execute(() =>
            {
                var res = accountRepository.GetAllAccounts(true);
                return Ok(res);
            });
        }
    }
}
