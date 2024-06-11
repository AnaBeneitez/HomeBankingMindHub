using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOS;

namespace HomeBankingMindHub.Services.Interfaces
{
    public interface IAuthService
    {
        Response Login(ClientLoginDTO loginDTO);
        string MakeToken(string email, int minutes);
    }
}
