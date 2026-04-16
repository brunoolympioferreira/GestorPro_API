using GestorPro.Application.Models.ViewModels;
using GestorPro.Domain.Entities;

namespace GestorPro.Application.Models.Mappers;

public static class CustomerMapper
{
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
