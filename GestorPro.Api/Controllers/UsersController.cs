using GestorPro.Application.Interfaces.Services;
using GestorPro.Application.Models.InputModels.User;
using Microsoft.AspNetCore.Mvc;

namespace GestorPro.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PostCreateUser([FromBody] CreateUserInputModel inputModel, CancellationToken cancellationToken)
    {
        var id = await service.CreateAsync(inputModel, cancellationToken);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return CreatedAtAction(nameof(PostCreateUser), new { id });
    }
}
