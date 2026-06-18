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
        var unitOfMeasure = await unityOfWork.UnitOfMeasures.GetByIdAsyncNoTracking(id)
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

        unitOfMeasure.Update(inputModel.Code, inputModel.Name);

        await unityOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        bool exists = await unityOfWork.UnitOfMeasures.ExistsAsync(id);
        if (!exists) throw new KeyNotFoundException();

        //Todo: Checar se a unidade de medida está associada a algum produto, se sim, lançar uma exceção;

        await unityOfWork.UnitOfMeasures.DeleteAsync(id);
        await unityOfWork.SaveChangesAsync(cancellationToken);
    }
}
