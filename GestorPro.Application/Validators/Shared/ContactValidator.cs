using FluentValidation;
using GestorPro.Application.Models.DTO;

namespace GestorPro.Application.Validators.Shared;

public class ContactValidator : AbstractValidator<ContactDTO>
{
    public ContactValidator()
    {
        RuleFor(x => x.Email)
            .Must(email => !email.Contains(' '))
            .WithMessage("O email não pode conter espaços.")
            .EmailAddress()
            .WithMessage("O email deve ser válido.")
            .MaximumLength(255)
            .WithMessage("O email deve ter no máximo 255 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Email));
    }
}
