using FluentValidation;
using GestorPro.Application.Models.InputModels;

namespace GestorPro.Application.Validators.ProductCategory;

public class CreateProductCategoryValidator : AbstractValidator<CreateProductCategoryInputModel>
{
    public CreateProductCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("O nome é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Description)
            .MaximumLength(300)
            .WithMessage("A descrição deve ter no máximo 300 caracteres.")
            .When(x => x.Description != null);

    }
}
