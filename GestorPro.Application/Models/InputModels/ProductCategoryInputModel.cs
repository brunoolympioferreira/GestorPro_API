namespace GestorPro.Application.Models.InputModels;

public record CreateProductCategoryInputModel(string Name, string? Description, bool IsActive);
public record UpdateProductCategoryInputModel(string Name, string? Description, bool IsActive);
