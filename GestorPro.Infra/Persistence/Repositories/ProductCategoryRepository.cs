using GestorPro.Domain.Entities;
using GestorPro.Domain.Interfaces.Repositories;

namespace GestorPro.Infra.Persistence.Repositories;

public class ProductCategoryRepository(AppDbContext context) : BaseRepository<ProductCategory>(context), IProductCategoryRepository
{
}
