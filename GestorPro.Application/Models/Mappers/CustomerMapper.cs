using GestorPro.Application.Models.InputModels;
using GestorPro.Application.Models.ViewModels;
using GestorPro.Domain.Entities;
using GestorPro.Domain.Enums;

namespace GestorPro.Application.Models.Mappers;

public static class CustomerMapper
{
    public static void ApplyUpdate(this Customer customer, UpdateCustomerInputModel input)
    {
        var addresses = input.Addresses
            .Select(a => a.ToEntity(customer.Id))
            .ToList();

        var contacts = input.Contacts
            .Select(c => c.ToEntity(customer.Id))
            .ToList();

        customer.Update(input.Name, input.TradeName, addresses, contacts);
    }

    public static Customer ToEntity(this CreateCustomerInputModel input)
    {
        var customerStatusEnum = Enum.Parse<CustomerStatusEnum>(input.Status, ignoreCase: true);
        var customer = new Customer(input.Name, input.TradeName, input.Document, customerStatusEnum);

        return customer;
    }

    public static CustomerViewModel ToViewModel(this Customer customer) => new(
        customer.Id,
        customer.Name,
        customer.TradeName,
        customer.Document.Value,
        customer.Status.ToString()
    );

    public static CustomerDetailViewModel ToDetailViewModel(this Customer customer) => new(
        customer.Id,
        customer.Name,
        customer.TradeName,
        customer.Document.Value,
        customer.Status.ToString(),
        customer.Addresses.ToAddressDTOList(),
        customer.Contacts.ToContactDTOList()
    );

    public static IEnumerable<CustomerViewModel> ToViewModelList(this IEnumerable<Customer> customers) =>
        customers.Select(c => c.ToViewModel());
}
