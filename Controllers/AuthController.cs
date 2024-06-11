using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOS;
using HomeBankingMindHub.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) 
        { 
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]ClientLoginDTO loginDTO)
        {
            try
            {
                Response response = _authService.Login(loginDTO);
                
                if(response.StatusCode != 200)
                    return StatusCode(response.StatusCode, response.Message);

                string token = _authService.MakeToken(loginDTO.Email, 10);

                return StatusCode(response.StatusCode, token);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
