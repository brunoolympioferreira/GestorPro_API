using GestorPro.Application.Models.DTO;

namespace GestorPro.Application.Interfaces.Services;

public interface IAuthService
{
    string ComputeSha256Hash(string password);
    TokenDTO GenerateJwtToken(Domain.Entities.User user);
    bool VerifyPassword(string password, string passwordHash);
}
