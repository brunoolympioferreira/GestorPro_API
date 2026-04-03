using GestorPro.Application.Interfaces.Services;
using GestorPro.Application.Models.InputModels.Customer;
using GestorPro.Domain.Interfaces.Contracts;

namespace GestorPro.Application.Services;

public class CustomerService(IUnityOfWork unityOfWork) : ICustomerService
{
    public async Task<Guid> CreateAsync(CreateCustomerInputModel inputModel, CancellationToken cancellationToken = default)
    {
        var contacts = inputModel.Contacts.Select(c => c.ToEntity()).ToList() ?? [];

        var addresses = inputModel.Addresses.Select(a => a.ToEntity()).ToList() ?? [];

        var customer = inputModel.ToEntity(addresses, contacts);

        await unityOfWork.Customers.AddAsync(customer);
        await unityOfWork.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }
}
