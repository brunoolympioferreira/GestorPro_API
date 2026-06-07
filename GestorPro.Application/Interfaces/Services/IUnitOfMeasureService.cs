using GestorPro.Application.Models.InputModels;
using GestorPro.Application.Models.ViewModels;

namespace GestorPro.Application.Interfaces.Services;

public interface IUnitOfMeasureService
{
    Task<Guid> CreateAsync(CreateUnitOfMeasureInputModel inputModel, CancellationToken cancellationToken = default);
    Task<UnitOfMeasureDetailViewModel> GetByIdAsync(Guid id);
    Task<IEnumerable<UnitOfMeasureViewModel>> GetAllAsync();
    Task Update(Guid id, UpdateUnitOfMeasureInputModel inputModel, CancellationToken cancellationToken = default);
}
