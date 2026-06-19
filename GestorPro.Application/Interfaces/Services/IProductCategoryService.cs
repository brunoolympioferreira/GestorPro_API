using GestorPro.Application.Models.InputModels;
using GestorPro.Application.Models.ViewModels;

namespace GestorPro.Application.Interfaces.Services;

public interface IProductCategoryService
{
    Task<Guid> CreateAsync(CreateProductCategoryInputModel inputModel, CancellationToken cancellationToken = default);
    Task Update(Guid id, UpdateProductCategoryInputModel inputModel, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductCategoryViewModel>> GetAllAsync();
    Task<ProductCategoryDetailViewModel> GetByIdAsync(Guid id);
}
