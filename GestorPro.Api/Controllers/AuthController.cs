using GestorPro.Application.Interfaces.Services;
using GestorPro.Application.Models.InputModels.User;
using Microsoft.AspNetCore.Mvc;

namespace GestorPro.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(ILoginService service) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginInputModel inputModel, CancellationToken cancellationToken)
    {
        var response = await service.LoginAsync(inputModel, cancellationToken);

        return Ok(response);
    }
}
