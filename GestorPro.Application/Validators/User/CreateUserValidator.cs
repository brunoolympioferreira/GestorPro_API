using FluentValidation;
using GestorPro.Application.Models.InputModels.User;
using GestorPro.Domain.Enums;

namespace GestorPro.Application.Validators.User;

public class CreateUserValidator : AbstractValidator<CreateUserInputModel>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(150).WithMessage("O nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email é obrigatório.")
            .EmailAddress().WithMessage("O email deve ser válido.")
            .MaximumLength(255).WithMessage("O email deve ter no máximo 255 caracteres.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres.")
            .MaximumLength(100).WithMessage("A senha deve ter no máximo 100 caracteres.");

        RuleFor(u => u.Role)
            .NotEmpty().WithMessage("Role é obrigatório.")
            .Must(BeValidRole).WithMessage(r =>
                $"O papel '{r.Role}' não é válido. Valores aceitos: {string.Join(", ", Enum.GetNames<RoleEnum>())}");
    }

    private bool BeValidRole(string role)
    {
        if (string.IsNullOrWhiteSpace(role))
            return false;

        return Enum.TryParse<RoleEnum>(role, true, out var roleEnum) &&
               Enum.IsDefined(roleEnum);
    }
}
