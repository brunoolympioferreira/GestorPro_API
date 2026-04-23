namespace GestorPro.Application.Models.DTO;

public record AddressDTO(
    Guid? Id,
    string Street,
    string Number,
    string Complement,
    string Neighborhood,
    string City,
    string State,
    string ZipCode,
    string AddressType
);
