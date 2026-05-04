using GestorPro.Application.Models.InputModels.UnitOfMeasure;
using GestorPro.Domain.Entities;

namespace GestorPro.Application.Models.Mappers;

public static class UnitOfMeasureMapper
{
    public static UnitOfMeasure ToEntity(this CreateUnitOfMeasureInputModel model)
        => new(model.Code, model.Name, model.IsActive);
}
