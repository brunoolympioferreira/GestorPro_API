using GestorPro.Domain.Entities;

namespace GestorPro.Application.Models.ViewModels;

public record class UnitOfMeasureViewModel(string Code, string Name, bool IsActive);
public record class UnitOfMeasureDetailViewModel(string Code, string Name, bool IsActive, ICollection<Product> Products);
