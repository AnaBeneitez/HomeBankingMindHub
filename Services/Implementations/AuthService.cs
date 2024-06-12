using System.Security.Claims;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOS;
using HomeBankingMindHub.Repositories.Interfaces;
using HomeBankingMindHub.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;

namespace HomeBankingMindHub.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IEncryptsService _encryptsService;
        
        public AuthService(IClientRepository clientRepository, IEncryptsService encryptsService)
        {
            _clientRepository = clientRepository;
            _encryptsService = encryptsService;
        }
        public ResponseModel<ClaimsIdentity> Login(ClientLoginDTO loginDTO)
        {
            ResponseModel<ClaimsIdentity> response;

            if (loginDTO.Email.IsNullOrEmpty() || loginDTO.Password.IsNullOrEmpty())
            {
                return new ResponseModel<ClaimsIdentity>(400, "Faltan campos", null);
            }

            Client user = _clientRepository.FindByEmail(loginDTO.Email);

            if (user == null || !_encryptsService.VerifyPassword(loginDTO.Password, user.Salt, user.Password))
            {
                return new ResponseModel<ClaimsIdentity>(401, "Credenciales inválidas", null);
            }

            var claims = new List<Claim>();

            if (user.Email.Equals("ana@gmail.com"))
                claims.Add(new Claim("Admin", user.Email));

            claims.Add(new Claim("Client", user.Email));

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            return new ResponseModel<ClaimsIdentity>(200, "Ok", claimsIdentity);
        }
    }
}
