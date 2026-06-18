namespace GestorPro.Application.Models.ViewModels;

public record class ProductCategoryViewModel(Guid Id, string Name, string? Description, bool IsActive);
public record class ProductCategoryDetailViewModel(Guid Id, string Name, string? Description, bool IsActive);

//todo: adicionar view de produtos vinculados em detalhes quando desenvolver modulo de produtos.
