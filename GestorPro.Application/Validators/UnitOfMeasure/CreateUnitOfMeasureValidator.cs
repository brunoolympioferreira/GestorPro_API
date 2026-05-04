using FluentValidation;
using GestorPro.Application.Models.InputModels.UnitOfMeasure;

namespace GestorPro.Application.Validators.UnitOfMeasure;

public class CreateUnitOfMeasureValidator : AbstractValidator<CreateUnitOfMeasureInputModel>
{
    public CreateUnitOfMeasureValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("O código é obrigatório.")
            .MaximumLength(10)
            .WithMessage("O código deve ter no máximo 10 caracteres.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("O nome é obrigatório.")
            .MaximumLength(50)
            .WithMessage("O nome deve ter no máximo 50 caracteres.");
    }
}
