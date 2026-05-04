using GestorPro.Application.Interfaces.Services;
using GestorPro.Application.Models.InputModels.UnitOfMeasure;
using GestorPro.Application.Models.Mappers;
using GestorPro.Domain.Interfaces.Contracts;

namespace GestorPro.Application.Services;

public class UnitOfMeasureService(IUnityOfWork unityOfWork) : IUnitOfMeasureService
{
    public async Task<Guid> CreateAsync(CreateUnitOfMeasureInputModel inputModel, CancellationToken cancellationToken = default)
    {
        var unitOfMeasure = inputModel.ToEntity();

        //await unityOfWork.UnitOfMeasures.AddAsync(unitOfMeasure); Testar a necessidade do ADD no repositorio.

        await unityOfWork.SaveChangesAsync(cancellationToken);

        return unitOfMeasure.Id;
    }
}
