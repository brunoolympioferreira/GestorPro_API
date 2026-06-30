namespace GestorPro.Tests.Modules.ProductCategories.Services;

public class ProductCategoryServiceTests : UnitTestBase
{
    private readonly IUnityOfWork _unityOfWork;
    private readonly IProductCategoryService _service;

    public ProductCategoryServiceTests()
    {
        _unityOfWork = Fixture.Freeze<IUnityOfWork>();
        _service = Fixture.Create<ProductCategoryService>();
    }

    // ─── GetAll ───────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetAllAsync_WhenCategoriesExist_ShouldReturnMappedViewModels()
    {
        // Arrange
        var categories = new ProductCategoryFaker().Generate(3);
        _unityOfWork.ProductCategories.GetAllAsync().Returns(categories);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        await _unityOfWork.ProductCategories.Received(1).GetAllAsync();
    }

    [Fact]
    public async Task GetAllAsync_WhenNoCategoriesExist_ShouldReturnEmptyList()
    {
        // Arrange
        _unityOfWork.ProductCategories.GetAllAsync().Returns(new List<ProductCategory>());

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    // ─── GetById ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetByIdAsync_WhenCategoryExists_ShouldReturnDetailViewModel()
    {
        // Arrange
        var id = Guid.NewGuid();
        var category = ProductCategoryFaker.Active();
        _unityOfWork.ProductCategories.GetByIdAsyncNoTracking(id).Returns(category);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(category.Name);
        await _unityOfWork.ProductCategories.Received(1).GetByIdAsyncNoTracking(id);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCategoryDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _unityOfWork.ProductCategories.GetByIdAsyncNoTracking(id).ReturnsNull();

        // Act
        var act = async () => await _service.GetByIdAsync(id);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Categoria de produto não encontrada.");
    }

    // ─── Create ───────────────────────────────────────────────────────────────

    [Fact]
    public async Task CreateAsync_WithUniqueName_ShouldAddCategoryAndSaveChanges()
    {
        // Arrange
        var input = ProductCategoryFaker.ValidCreateInput();

        // ExistsAsync(Expression<Func<T, bool>>) — sobrecarga usada pelo service
        _unityOfWork.ProductCategories
            .ExistsAsync(Arg.Any<Expression<Func<ProductCategory, bool>>>())
            .Returns(false);

        // Act
        var result = await _service.CreateAsync(input);

        // Assert
        result.Should().NotBe(Guid.Empty);
        await _unityOfWork.ProductCategories.Received(1)
            .AddAsync(Arg.Is<ProductCategory>(c =>
                c.Name == input.Name &&
                c.Description == input.Description));
        await _unityOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateName_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var input = ProductCategoryFaker.ValidCreateInput();

        // ExistsAsync(Expression<Func<T, bool>>) — sobrecarga usada pelo service
        _unityOfWork.ProductCategories
            .ExistsAsync(Arg.Any<Expression<Func<ProductCategory, bool>>>())
            .Returns(true);

        // Act
        var act = async () => await _service.CreateAsync(input);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Uma categoria com o mesmo nome já existe.");

        await _unityOfWork.ProductCategories.DidNotReceive().AddAsync(Arg.Any<ProductCategory>());
        await _unityOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    // ─── Update ───────────────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateAsync_WhenCategoryExists_ShouldUpdateAndSaveChanges()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existingCategory = ProductCategoryFaker.Active();
        _unityOfWork.ProductCategories.GetByIdAsync(id).Returns(existingCategory);

        var input = ProductCategoryFaker.ValidUpdateInput(isActive: false);

        // Act
        await _service.Update(id, input);

        // Assert
        existingCategory.Name.Should().Be(input.Name);
        existingCategory.Description.Should().Be(input.Description);
        existingCategory.IsActive.Should().BeFalse();
        existingCategory.UpdatedAt.Should().NotBeNull();
        await _unityOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateAsync_WhenCategoryDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _unityOfWork.ProductCategories.GetByIdAsync(id).ReturnsNull();

        var input = ProductCategoryFaker.ValidUpdateInput();

        // Act
        var act = async () => await _service.Update(id, input);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Categoria de produto não encontrada.");

        await _unityOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    // ─── Delete ───────────────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteAsync_WhenCategoryExists_ShouldDeleteAndSaveChanges()
    {
        // Arrange
        var id = Guid.NewGuid();

        // ExistsAsync(Guid) — sobrecarga usada pelo service no Delete
        _unityOfWork.ProductCategories
            .ExistsAsync(Arg.Any<Guid>())
            .Returns(true);

        // Act
        await _service.DeleteAsync(id);

        // Assert
        await _unityOfWork.ProductCategories.Received(1).DeleteAsync(id);
        await _unityOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteAsync_WhenCategoryDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();

        // ExistsAsync(Guid) — sobrecarga usada pelo service no Delete
        _unityOfWork.ProductCategories
            .ExistsAsync(Arg.Any<Guid>())
            .Returns(false);

        // Act
        var act = async () => await _service.DeleteAsync(id);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
        await _unityOfWork.ProductCategories.DidNotReceive().DeleteAsync(Arg.Any<Guid>());
        await _unityOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}