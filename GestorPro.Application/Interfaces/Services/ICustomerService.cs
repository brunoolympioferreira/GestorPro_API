using GestorPro.Application.Models.InputModels.Customer;

namespace GestorPro.Application.Interfaces.Services;

public interface ICustomerService
{
    Task<Guid> CreateAsync(CreateCustomerInputModel inputModel, CancellationToken cancellationToken = default);
}
