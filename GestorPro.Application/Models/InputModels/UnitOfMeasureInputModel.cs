namespace GestorPro.Application.Models.InputModels;

public record CreateUnitOfMeasureInputModel(string Code, string Name, bool IsActive);

public record UpdateUnitOfMeasureInputModel(string Code, string Name);
