using System.Security.Cryptography;
using System.Text;
using Domain.Helpers.Interfaces;

namespace Domain.Helpers
{
    public class PasswordHelper : IPasswordHelper
    {
        public string ComputeSha256Hash(string password)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));

            StringBuilder sb = new();
            for (int i = 0; i < bytes.Length; ++i)
            {
                _ = sb.Append(bytes[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }
}