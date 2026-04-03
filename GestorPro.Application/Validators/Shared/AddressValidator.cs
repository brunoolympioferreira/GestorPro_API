using FluentValidation;
using GestorPro.Application.Models.DTO;
using GestorPro.Domain.Enums;

namespace GestorPro.Application.Validators.Shared;

public class AddressValidator : AbstractValidator<AddressDTO>
{
    private static readonly string[] ValidStates =
    [
        "AC","AL","AP","AM","BA","CE","DF","ES","GO",
        "MA","MT","MS","MG","PA","PB","PR","PE","PI",
        "RJ","RN","RS","RO","RR","SC","SP","SE","TO"
    ];
    public AddressValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("O ID do cliente é obrigatório.");

        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage("O logradouro é obrigatório.")
            .MaximumLength(200)
            .WithMessage("O logradouro deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Number)
            .NotEmpty()
            .WithMessage("O número é obrigatório.")
            .MaximumLength(20)
            .WithMessage("O número deve ter no máximo 10 caracteres.");

        RuleFor(x => x.Complement)
            .MaximumLength(100)
            .WithMessage("O complemento deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Neighborhood)
            .NotEmpty()
            .WithMessage("O bairro é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O bairro deve ter no máximo 100 caracteres.");

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("A cidade é obrigatória.")
            .MaximumLength(100)
            .WithMessage("A cidade deve ter no máximo 100 caracteres.");

        RuleFor(x => x.State)
            .NotEmpty()
            .WithMessage("O estado é obrigatório.")
            .Length(2)
            .WithMessage("O estado deve conter exatamente 2 caracteres (UF).")
            .Must(s => ValidStates.Contains(s?.ToUpper()))
            .WithMessage("O estado informado não é uma UF válida.");

        RuleFor(x => x.ZipCode)
            .NotEmpty()
            .WithMessage("O CEP é obrigatório.");

        RuleFor(x => x.AddressType)
            .NotEmpty()
            .WithMessage("O tipo de endereço é obrigatório.")
            .Must(BeValidAddressType)
            .WithMessage($"Tipo de endereço inválido. Valores aceitos: {string.Join(", ", Enum.GetNames<AddressTypeEnum>())}.");
    }
    private bool BeValidAddressType(string addressType)
    {
        if (string.IsNullOrWhiteSpace(addressType))
            return false;

        return Enum.TryParse<AddressTypeEnum>(addressType, true, out var addressTypeEnum) &&
               Enum.IsDefined(addressTypeEnum);
    }
}
