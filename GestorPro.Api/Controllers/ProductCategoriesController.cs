using GestorPro.Application.Interfaces.Services;
using GestorPro.Application.Models.InputModels;
using GestorPro.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorPro.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductCategoriesController(IProductCategoryService service) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = $"{nameof(RoleEnum.Admin)},{nameof(RoleEnum.Manager)}, {nameof(RoleEnum.Employee)}")]
    public async Task<IActionResult> PostCreate([FromBody] CreateProductCategoryInputModel inputModel, CancellationToken cancellationToken)
    {
        var id = await service.CreateAsync(inputModel, cancellationToken);

        return CreatedAtAction(nameof(PostCreate), new { id });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{nameof(RoleEnum.Admin)},{nameof(RoleEnum.Manager)}, {nameof(RoleEnum.Employee)}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCategoryInputModel inputModel, CancellationToken cancellationToken)
    {
        await service.Update(id, inputModel, cancellationToken);
        return NoContent();
    }

    [HttpGet]
    [Authorize(Roles = $"{nameof(RoleEnum.Admin)},{nameof(RoleEnum.Manager)}, {nameof(RoleEnum.Employee)}, {nameof(RoleEnum.Viewer)}")]
    public async Task<IActionResult> GetAll()
    {
        var productCategories = await service.GetAllAsync();
        return Ok(productCategories);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = $"{nameof(RoleEnum.Admin)},{nameof(RoleEnum.Manager)}, {nameof(RoleEnum.Employee)}, {nameof(RoleEnum.Viewer)}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var productCategory = await service.GetByIdAsync(id);
        return Ok(productCategory);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = $"{nameof(RoleEnum.Admin)},{nameof(RoleEnum.Manager)}, {nameof(RoleEnum.Employee)}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await service.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
