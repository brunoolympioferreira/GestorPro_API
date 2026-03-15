using GestorPro.Application.Interfaces.Services;
using System.Security.Cryptography;
using System.Text;

namespace GestorPro.Application.Services;

public class AuthService : IAuthService
{
    public string ComputeSha256Hash(string password)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));

        StringBuilder builder = new();

        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }

        return builder.ToString();
    }
}
