namespace HomeBankingMindHub.Services.Interfaces
{
    public interface IEncryptsService
    {
        void EncryptPassword(string password, out byte[] salt, out string hash);
        bool VerifyPassword(string password, byte[] salt, string hash);
    }
}
