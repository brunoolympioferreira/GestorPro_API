using GestorPro.Application.Interfaces.Services;
using GestorPro.Application.Models.InputModels;
using GestorPro.Application.Models.Mappers;
using GestorPro.Domain.Interfaces.Contracts;

namespace GestorPro.Application.Services;

public class ProductCategoryService(IUnityOfWork unityOfWork) : IProductCategoryService
{
    public async Task<Guid> CreateAsync(CreateProductCategoryInputModel inputModel, CancellationToken cancellationToken = default)
    {
        var productCategory = inputModel.ToEntity();

        await unityOfWork.ProductCategories.AddAsync(productCategory);

        await unityOfWork.SaveChangesAsync(cancellationToken);

        return productCategory.Id;
    }
}
