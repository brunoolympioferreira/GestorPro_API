using GestorPro.Application.Interfaces.Services;
using GestorPro.Application.Models.InputModels.Customer;
using GestorPro.Application.Models.Mappers;
using GestorPro.Application.Models.ViewModels;
using GestorPro.Domain.Interfaces.Contracts;

namespace GestorPro.Application.Services;

public class CustomerService(IUnityOfWork unityOfWork) : ICustomerService
{
    public async Task<Guid> CreateAsync(CreateCustomerInputModel inputModel, CancellationToken cancellationToken = default)
    {
        var customer = inputModel.ToEntity();

        var contacts = inputModel.Contacts?.Select(c => c.ToEntity(customer.Id)).ToList() ?? [];

        var addresses = inputModel.Addresses?.Select(a => a.ToEntity(customer.Id)).ToList() ?? [];

        customer.AddAddresses(addresses);
        customer.AddContacts(contacts);

        await unityOfWork.Customers.AddAsync(customer);
        await unityOfWork.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }

    public async Task<IEnumerable<CustomerViewModel>> GetAllAsync()
    {
        var customers = await unityOfWork.Customers.GetAllAsync();

        var customerViewModels = customers.ToViewModelList();

        return customerViewModels;
    }

    public async Task<CustomerDetailViewModel> GetByIdAsync(Guid id, bool includeAddress, bool includeContact)
    {
        var customer = await unityOfWork.Customers.GetByIdAsync(id, true, true)
            ?? throw new KeyNotFoundException();

        var customerViewModel = customer.ToDetailViewModel();

        return customerViewModel;
    }
}
