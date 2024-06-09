using System.Security.Claims;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOS;
using HomeBankingMindHub.Repositories.Interfaces;
using HomeBankingMindHub.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IClientRepository _clientRepository;
        private readonly IEncryptsService _encryptsService;
        public AuthController(IClientRepository clientRepository, IEncryptsService encryptsService)
        {
            _clientRepository = clientRepository;
            _encryptsService = encryptsService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]ClientLoginDTO loginDTO)
        {
            try
            {
                Client user = _clientRepository.FindByEmail(loginDTO.Email);

                if (user == null || !_encryptsService.VerifyPassword(loginDTO.Password, user.Salt, user.Password))
                {
                    return Unauthorized();
                }

                var claims = new List<Claim>();

                if (user.Email.Equals("ana@gmail.com"))
                    claims.Add(new Claim ("Admin", user.Email ));

                claims.Add(new Claim("Client", user.Email ));

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
