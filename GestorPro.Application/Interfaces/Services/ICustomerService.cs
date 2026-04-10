using GestorPro.Application.Models.InputModels.Customer;
using GestorPro.Application.Models.ViewModels;

namespace GestorPro.Application.Interfaces.Services;

public interface ICustomerService
{
    Task<Guid> CreateAsync(CreateCustomerInputModel inputModel, CancellationToken cancellationToken = default);
    Task<IEnumerable<CustomerViewModel>> GetAllAsync();
    Task<CustomerDetailViewModel> GetByIdAsync(Guid id, bool includeAddress, bool includeContact);
}
