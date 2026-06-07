using FluentValidation;
using GestorPro.Application.Models.InputModels;

namespace GestorPro.Application.Validators.User;

public class LoginValidator : AbstractValidator<LoginInputModel>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email é obrigatório.")
            .EmailAddress().WithMessage("O email deve ser válido.")
            .MaximumLength(255).WithMessage("O email deve ter no máximo 255 caracteres.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MaximumLength(100).WithMessage("A senha deve ter no máximo 100 caracteres.")
            .Matches(@"^(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9]).{8,}$")
            .WithMessage("A senha deve ter no mínimo 8 caracteres, ao menos uma letra maiúscula, um número e um caractere especial.");
    }
}
