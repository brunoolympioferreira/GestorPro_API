using GestorPro.Application.Models.InputModels.UnitOfMeasure;
using GestorPro.Application.Models.ViewModels;
using GestorPro.Domain.Entities;

namespace GestorPro.Application.Models.Mappers;

public static class UnitOfMeasureMapper
{
    public static UnitOfMeasure ToEntity(this CreateUnitOfMeasureInputModel model)
        => new(model.Code, model.Name, model.IsActive);

    public static UnitOfMeasureViewModel ToViewModel(this UnitOfMeasure unitOfMeasure) => new(
        unitOfMeasure.Id,
        unitOfMeasure.Code,
        unitOfMeasure.Name,
        unitOfMeasure.IsActive
    );

    public static UnitOfMeasureDetailViewModel ToDetailViewModel(this UnitOfMeasure unitOfMeasure) => new(
        unitOfMeasure.Id,
        unitOfMeasure.Code,
        unitOfMeasure.Name,
        unitOfMeasure.IsActive,
        [] //TODO: Mapear produtos associados quando iniciar modulo de produtos;
    );

    public static IEnumerable<UnitOfMeasureViewModel> ToViewModelList(this IEnumerable<UnitOfMeasure> unitOfMeasures) =>
        unitOfMeasures.Select(u => u.ToViewModel());
}
