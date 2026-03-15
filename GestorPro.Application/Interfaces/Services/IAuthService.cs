namespace GestorPro.Application.Interfaces.Services;

public interface IAuthService
{
    string ComputeSha256Hash(string password);
}
