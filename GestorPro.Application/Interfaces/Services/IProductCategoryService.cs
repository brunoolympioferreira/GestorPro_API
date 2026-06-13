using GestorPro.Application.Models.InputModels;

namespace GestorPro.Application.Interfaces.Services;

public interface IProductCategoryService
{
    Task<Guid> CreateAsync(CreateProductCategoryInputModel inputModel, CancellationToken cancellationToken = default);
}
