using GestorPro.Application.Interfaces.Services;
using GestorPro.Application.Models.InputModels.Customer;
using GestorPro.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorPro.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CustomersController(ICustomerService service) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = $"{nameof(RoleEnum.Admin)},{nameof(RoleEnum.Manager)}, {nameof(RoleEnum.Employee)}")]
    public async Task<IActionResult> PostCreate([FromBody] CreateCustomerInputModel inputModel, CancellationToken cancellationToken)
    {
        var id = await service.CreateAsync(inputModel, cancellationToken);

        return CreatedAtAction(nameof(PostCreate), new { id });
    }

    [HttpGet]
    [Authorize(Roles = $"{nameof(RoleEnum.Admin)},{nameof(RoleEnum.Manager)}, {nameof(RoleEnum.Employee)}, {nameof(RoleEnum.Viewer)}")]
    public async Task<IActionResult> GetAll()
    {
        var customers = await service.GetAllAsync();
        return Ok(customers);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = $"{nameof(RoleEnum.Admin)},{nameof(RoleEnum.Manager)}, {nameof(RoleEnum.Employee)}, {nameof(RoleEnum.Viewer)}")]
    public async Task<IActionResult> GetById(Guid id, [FromQuery] bool includeAddress = false, [FromQuery] bool includeContact = false)
    {
        var customer = await service.GetByIdAsync(id, includeAddress, includeContact);
        return Ok(customer);
    }
}
