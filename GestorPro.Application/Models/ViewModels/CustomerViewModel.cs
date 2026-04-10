using GestorPro.Application.Models.DTO;

namespace GestorPro.Application.Models.ViewModels;

public record CustomerViewModel(
    string Name, 
    string TradeName, 
    string Document, 
    string Status
);
public record CustomerDetailViewModel(
    string Name, 
    string TradeName, 
    string Document, 
    string Status, 
    ICollection<AddressDTO?> Addresses,
    ICollection<ContactDTO?> Contacts
);
