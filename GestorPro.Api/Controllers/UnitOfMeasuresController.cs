using GestorPro.Application.Interfaces.Services;
using GestorPro.Application.Models.InputModels;
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

    [HttpGet("{id:guid}")]
    [Authorize(Roles = $"{nameof(RoleEnum.Admin)},{nameof(RoleEnum.Manager)}, {nameof(RoleEnum.Employee)}, {nameof(RoleEnum.Viewer)}")]
    public async Task<IActionResult> GetByid(Guid id)
    {
        var unitOfMeasure = await service.GetByIdAsync(id);
        return Ok(unitOfMeasure);
    }

    [HttpGet]
    [Authorize(Roles = $"{nameof(RoleEnum.Admin)},{nameof(RoleEnum.Manager)}, {nameof(RoleEnum.Employee)}, {nameof(RoleEnum.Viewer)}")]
    public async Task<IActionResult> GetAll()
    {
        var unitOfMeasures = await service.GetAllAsync();
        return Ok(unitOfMeasures);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUnitOfMeasureInputModel inputModel, CancellationToken cancellationToken)
    {
        await service.Update(id, inputModel, cancellationToken);

        return NoContent();
    }
}
