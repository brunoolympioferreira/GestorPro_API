using GestorPro.Application.Interfaces.Services;
using GestorPro.Application.Models.InputModels.User;
using GestorPro.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorPro.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController(IUserService service) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> PostCreate([FromBody] CreateUserInputModel inputModel, CancellationToken cancellationToken)
    {
        var id = await service.CreateAsync(inputModel, cancellationToken);

        return CreatedAtAction(nameof(PostCreate), new { id });
    }

    [HttpGet]
    [Authorize(Roles = $"{nameof(RoleEnum.Admin)},{nameof(RoleEnum.Manager)}")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var users = await service.GetAllAsync(cancellationToken);

        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = $"{nameof(RoleEnum.Admin)},{nameof(RoleEnum.Manager)}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var user = await service.GetByIdAsync(id, cancellationToken);

        return Ok(user);
    }
}
