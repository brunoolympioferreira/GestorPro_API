using GestorPro.Application.Models.InputModels.UnitOfMeasure;
using GestorPro.Application.Models.ViewModels;
using GestorPro.Domain.Entities;

namespace GestorPro.Application.Interfaces.Services;

public interface IUnitOfMeasureService
{
    Task<Guid> CreateAsync(CreateUnitOfMeasureInputModel inputModel, CancellationToken cancellationToken = default);
    Task<UnitOfMeasureDetailViewModel> GetByIdAsync(Guid id);
    Task<IEnumerable<UnitOfMeasureViewModel>> GetAllAsync();
}
