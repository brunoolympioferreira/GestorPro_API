using GestorPro.Application.Interfaces.Services;
using GestorPro.Application.Models.InputModels;
using GestorPro.Application.Models.Mappers;
using GestorPro.Application.Models.ViewModels;
using GestorPro.Domain.Interfaces.Contracts;

namespace GestorPro.Application.Services;

public class UnitOfMeasureService(IUnityOfWork unityOfWork) : IUnitOfMeasureService
{
    public async Task<Guid> CreateAsync(CreateUnitOfMeasureInputModel inputModel, CancellationToken cancellationToken = default)
    {
        var unitOfMeasure = inputModel.ToEntity();

        await unityOfWork.UnitOfMeasures.AddAsync(unitOfMeasure);

        await unityOfWork.SaveChangesAsync(cancellationToken);

        return unitOfMeasure.Id;
    }

    public async Task<UnitOfMeasureDetailViewModel> GetByIdAsync(Guid id)
    {
        var unitOfMeasure = await unityOfWork.UnitOfMeasures.GetByIdAsync(id)
            ?? throw new KeyNotFoundException();

        var unitOfMeasureViewModel = unitOfMeasure.ToDetailViewModel();

        return unitOfMeasureViewModel;
    }

    public async Task<IEnumerable<UnitOfMeasureViewModel>> GetAllAsync()
    {
        var unitOfMeasures = await unityOfWork.UnitOfMeasures.GetAllAsync();

        var unitOfMeasureViewModels = unitOfMeasures.ToViewModelList();

        return unitOfMeasureViewModels;
    }

    public async Task Update(Guid id, UpdateUnitOfMeasureInputModel inputModel, CancellationToken cancellationToken = default)
    {
        var unitOfMeasure = await unityOfWork.UnitOfMeasures.GetByIdAsync(id)
            ?? throw new KeyNotFoundException();

        unitOfMeasure.Update(inputModel.Code, inputModel.Name, inputModel.IsActive);

        await unityOfWork.SaveChangesAsync(cancellationToken);
    }
}
