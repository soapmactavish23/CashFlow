using CashFlow.Domain.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;

namespace CashFlow.Infrastructure.Security
{
    public class BCrypt : IPasswordEncripter
    {
        public string Encrypt(string password)
        {
            string passwordHash = BC.HashPassword(password);

            BC.Verify(password, passwordHash);

            return passwordHash;
        }

        public bool Verify(string password, string passwordHash) => BC.Verify(password, passwordHash);
    }
}
