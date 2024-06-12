using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOS;
using HomeBankingMindHub.Repositories.Interfaces;
using HomeBankingMindHub.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace HomeBankingMindHub.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IEncryptsService _encryptsService;
        private readonly IConfiguration _configuration;
        private readonly IClientRepository _clientRepository;

        public AuthService(IEncryptsService encryptsService, IConfiguration configuration, IClientRepository clientRepository)
        {
            _encryptsService = encryptsService;
            _configuration = configuration;
            _clientRepository = clientRepository;
        }

        public Response Login(ClientLoginDTO loginDTO)
        {
            Response response;
            
            if(loginDTO.Email.IsNullOrEmpty() || loginDTO.Password.IsNullOrEmpty())
            {
                return new Response(400, "Faltan campos");
            }

            Client user = _clientRepository.FindByEmail(loginDTO.Email);

            if (user == null || !_encryptsService.VerifyPassword(loginDTO.Password, user.Salt, user.Password))
            {
                return new Response(401, "Credenciales inválidas");
            }

            return new Response(200, "Ok");
        }

        public string MakeToken(string email, int minutes)
        {
            var claims = new List<Claim>();

            if (email.Equals("ana@gmail.com"))
                claims.Add(new Claim("Admin", email));

            claims.Add(new Claim("Client", email));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Key").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds);

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return token;
        }
    }
}
