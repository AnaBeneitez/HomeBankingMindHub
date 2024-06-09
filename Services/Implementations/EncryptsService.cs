using System.Security.Cryptography;
using HomeBankingMindHub.Services.Interfaces;

namespace HomeBankingMindHub.Services.Implementations
{
    public class EncryptsService : IEncryptsService
    {
        public void EncryptPassword(string password, out byte[] salt, out string hash)
        {
            using (var encrypter = new HMACSHA512())
            {
                salt = encrypter.Key;
                var hashByte = encrypter.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                hash = Convert.ToBase64String(hashByte);
            }
        }

        public bool VerifyPassword(string password, byte[] salt, string hash)
        {
            using (var encrypter = new HMACSHA512(salt))
            {
                var compareHash = encrypter.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                byte[] hashByte = Convert.FromBase64String(hash);

                return compareHash.SequenceEqual(hashByte);
            }
        }
    }
}
