namespace GestorPro.Application.Models.DTO;

public record AddressDTO(
    Guid CustomerId,
    string Street,
    string Number,
    string Complement,
    string Neighborhood,
    string City,
    string State,
    string ZipCode,
    string AddressType)
{
}
