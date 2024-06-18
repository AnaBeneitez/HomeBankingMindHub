using System.Security.Claims;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOS;

namespace HomeBankingMindHub.Services.Interfaces
{
    public interface IAuthService
    {
        ResponseModel<ClaimsIdentity> Login(ClientLoginDTO loginDTO);
    }
}
