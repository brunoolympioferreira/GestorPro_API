using GestorPro.Application.Models.DTO;

namespace GestorPro.Application.Models.ViewModels;

public record CustomerViewModel(
    Guid Id,
    string Name, 
    string TradeName, 
    string Document, 
    string Status
);
public record CustomerDetailViewModel(
    Guid Id,
    string Name, 
    string TradeName, 
    string Document, 
    string Status, 
    ICollection<AddressDTO?> Addresses,
    ICollection<ContactDTO?> Contacts
);
