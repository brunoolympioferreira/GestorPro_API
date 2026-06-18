using GestorPro.Application.Models.InputModels;
using GestorPro.Application.Models.ViewModels;
using GestorPro.Domain.Entities;

namespace GestorPro.Application.Models.Mappers;

public static class ProductCategoryMapper
{
    public static ProductCategory ToEntity(this CreateProductCategoryInputModel model)
        => new(model.Name, model.Description, model.IsActive);

    public static ProductCategoryViewModel ToViewModel(this ProductCategory productCategory) => new(
        productCategory.Id,
        productCategory.Name,
        productCategory.Description,
        productCategory.IsActive
    );

    public static ProductCategoryDetailViewModel ToDetailViewModel(this ProductCategory productCategory) => new(
        productCategory.Id,
        productCategory.Name,
        productCategory.Description,
        productCategory.IsActive
    //todo: mapear productos associados com categoria.
    );

    public static IEnumerable<ProductCategoryViewModel> ToViewModelList(this IEnumerable<ProductCategory> productCategories) =>
        productCategories.Select(u => u.ToViewModel());
}
