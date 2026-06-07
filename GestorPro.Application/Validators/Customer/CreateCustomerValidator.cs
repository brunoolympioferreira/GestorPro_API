using FluentValidation;
using GestorPro.Application.Models.InputModels;
using GestorPro.Application.Validators.Shared;
using GestorPro.Domain.Enums;

namespace GestorPro.Application.Validators.Customer;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerInputModel>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("O nome é obrigatório.")
            .MinimumLength(3)
            .WithMessage("O nome deve ter no mínimo 3 caracteres.")
            .MaximumLength(200)
            .WithMessage("O nome deve ter no máximo 150 caracteres.");

        RuleFor(x => x.TradeName)
            .MaximumLength(200)
            .WithMessage("O nome fantasia deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Document)
            .NotEmpty()
            .WithMessage("O documento é obrigatório.");

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("O status é obrigatório.")
            .Must(BeValidStatus)
            .WithMessage($"Status inválido. Valores aceitos: {string.Join(", ", Enum.GetNames<CustomerStatusEnum>())}.");

        When(x => x.Addresses is not null, () =>
        {
            RuleFor(x => x.Addresses)
                .NotEmpty()
                .WithMessage("A lista de endereços não pode ser vazia.");

            RuleForEach(x => x.Addresses)
                .SetValidator(new AddressValidator());
        });

        When(x => x.Contacts is not null, () =>
        {
            RuleFor(x => x.Contacts)
                .NotEmpty()
                .WithMessage("A lista de contatos não pode ser vazia.")
                .Must(contacts => contacts.Count(c => c.IsPrimary) == 1)
                .WithMessage("Exatamente um contato deve ser marcado como primário.");

            RuleForEach(x => x.Contacts)
                .SetValidator(new ContactValidator());
        });
    }

    private bool BeValidStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return false;

        return Enum.TryParse<CustomerStatusEnum>(status, false, out var statusEnum) &&
               Enum.IsDefined(statusEnum);
    }
}
