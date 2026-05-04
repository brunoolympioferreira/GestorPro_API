using GestorPro.Application.Models.InputModels.UnitOfMeasure;

namespace GestorPro.Application.Interfaces.Services;

public interface IUnitOfMeasureService
{
    Task<Guid> CreateAsync(CreateUnitOfMeasureInputModel inputModel, CancellationToken cancellationToken = default);
}
