using GestorPro.Application.Models.InputModels;
using GestorPro.Domain.Entities;

namespace GestorPro.Application.Models.Mappers;

public static class ProductCategoryMapper
{
    public static ProductCategory ToEntity(this CreateProductCategoryInputModel model) 
        => new(model.Name, model.Description, model.IsActive);
}
