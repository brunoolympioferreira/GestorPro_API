using GestorPro.Domain.Entities;

namespace GestorPro.Application.Models.ViewModels;

public record class UnitOfMeasureViewModel(Guid Id, string Code, string Name, bool IsActive);
public record class UnitOfMeasureDetailViewModel(Guid Id, string Code, string Name, bool IsActive, ICollection<Product> Products);
