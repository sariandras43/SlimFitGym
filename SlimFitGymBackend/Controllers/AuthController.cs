using Microsoft.AspNetCore.Mvc;
using SlimFitGym.EFData.Repositories;
using SlimFitGym.Models.Requests;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SlimFitGymBackend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly AccountRepository accountRepository;
        public AuthController(AccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        // POST api/<AuthController>
        [HttpPost("/login")]
        public IActionResult Login([FromBody] LoginRequest loginInfo)
        {
            return this.Execute(() =>
            {
                return Ok(accountRepository.Login(loginInfo));
            });
        }

        [HttpPost("/register")]
        public IActionResult Register([FromBody] LoginRequest loginInfo)
        {

        }

    }
}
