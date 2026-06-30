using GestorPro.Domain.Helpers;

namespace GestorPro.Tests.Domain;

public class ProductCategoryEntityTests : UnitTestBase
{
    [Fact]
    public void Constructor_WithValidData_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var category = ProductCategoryFaker.Active();

        // Assert
        category.Name.Should().Be("Eletrônicos");
        category.Description.Should().Be("Categoria de produtos eletrônicos");
        category.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Constructor_WithNullDescription_ShouldCreateSuccessfully()
    {
        // Arrange & Act
        var category = ProductCategoryFaker.WithoutDescription();

        // Assert
        category.Description.Should().BeNull();
        category.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Constructor_WithIsActiveFalse_ShouldCreateInactive()
    {
        // Arrange & Act
        var category = ProductCategoryFaker.Inactive();

        // Assert
        category.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Constructor_ShouldGenerateNonEmptyId()
    {
        // Arrange & Act
        var category = ProductCategoryFaker.Active();

        // Assert
        category.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Constructor_ShouldSetCreatedAtAndLeaveUpdatedAtNull()
    {
        // Arrange
        var before = DateTimeHelper.NowInBrasilia().AddSeconds(-1);

        // Act
        var category = ProductCategoryFaker.Active();

        // Assert
        category.CreatedAt.Should().BeAfter(before);
        category.UpdatedAt.Should().BeNull();
    }

    // ─── Update ───────────────────────────────────────────────────────────────

    [Fact]
    public void Update_WithValidData_ShouldChangeAllProperties()
    {
        // Arrange
        var category = ProductCategoryFaker.Active();
        var inputModel = ProductCategoryFaker.ValidUpdateInput(isActive: false);

        // Act
        category.Update(inputModel.Name, inputModel.Description, inputModel.IsActive);

        // Assert
        category.Name.Should().Be(inputModel.Name);
        category.Description.Should().Be(inputModel.Description);
        category.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Update_ShouldSetUpdatedAt()
    {
        // Arrange
        var category = ProductCategoryFaker.Active();

        // Act
        category.Update("Novo Nome", null, true);

        // Assert
        category.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Update_WithNullDescription_ShouldClearDescription()
    {
        // Arrange
        var category = ProductCategoryFaker.Active(); // tem descrição

        // Act
        category.Update(category.Name, null, true);

        // Assert
        category.Description.Should().BeNull();
    }

    [Fact]
    public void Update_TogglingIsActive_ShouldReflectNewValue()
    {
        // Arrange
        var category = ProductCategoryFaker.Active(); // IsActive = true

        // Act
        category.Update(category.Name, category.Description, false);

        // Assert
        category.IsActive.Should().BeFalse();
    }

    [Fact]
    public void TwoInstances_GeneratedWithSameSeed_ShouldHaveEqualData()
    {
        // Arrange & Act
        var faker = new ProductCategoryFaker(seed: 42);
        var c1 = faker.Generate();

        faker = new ProductCategoryFaker(seed: 42);
        var c2 = faker.Generate();

        // Assert
        c1.Name.Should().Be(c2.Name);
        c1.Description.Should().Be(c2.Description);
        c1.IsActive.Should().Be(c2.IsActive);
    }
}
