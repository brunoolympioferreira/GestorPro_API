using GestorPro.Application.Interfaces.Services;
using GestorPro.Application.Models.InputModels.UnitOfMeasure;
using GestorPro.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorPro.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UnitOfMeasuresController(IUnitOfMeasureService service) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = $"{nameof(RoleEnum.Admin)},{nameof(RoleEnum.Manager)}, {nameof(RoleEnum.Employee)}")]
    public async Task<IActionResult> PostCreate([FromBody] CreateUnitOfMeasureInputModel inputModel, CancellationToken cancellationToken)
    {
        var id = await service.CreateAsync(inputModel, cancellationToken);
        
        return CreatedAtAction(nameof(PostCreate), new { id });
    }
}
