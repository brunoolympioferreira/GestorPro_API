using GestorPro.Application.Interfaces.Services;
using GestorPro.Application.Models.InputModels;
using GestorPro.Application.Models.Mappers;
using GestorPro.Application.Models.ViewModels;
using GestorPro.Domain.Interfaces.Contracts;

namespace GestorPro.Application.Services;

public class ProductCategoryService(IUnityOfWork unityOfWork) : IProductCategoryService
{
    public async Task<Guid> CreateAsync(CreateProductCategoryInputModel inputModel, CancellationToken cancellationToken = default)
    {
        var productCategory = inputModel.ToEntity();

        var existingCategoryName = await unityOfWork.ProductCategories
            .ExistsAsync(c => c.Name.ToLower().Replace(" ", string.Empty) == inputModel.Name.Normalize());

        if (existingCategoryName)
            throw new InvalidOperationException("Uma categoria com o mesmo nome já existe.");

        await unityOfWork.ProductCategories.AddAsync(productCategory);

        await unityOfWork.SaveChangesAsync(cancellationToken);

        return productCategory.Id;
    }

    public async Task<IEnumerable<ProductCategoryViewModel>> GetAllAsync()
    {
        var productCategories = await unityOfWork.ProductCategories.GetAllAsync();
      
        var productCategoriesViewModels = productCategories.ToViewModelList();

        return productCategoriesViewModels;
    }

    public async Task<ProductCategoryDetailViewModel> GetByIdAsync(Guid id)
    {
        var productCategory = await unityOfWork.ProductCategories.GetByIdAsyncNoTracking(id)
            ?? throw new KeyNotFoundException("Categoria de produto não encontrada.");

        var productCategoryViewModel = productCategory.ToDetailViewModel();

        return productCategoryViewModel;
    }
}
