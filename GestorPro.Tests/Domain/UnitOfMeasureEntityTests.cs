namespace GestorPro.Tests.Domain;

public sealed class UnitOfMeasureEntityTests
{
    // =========================================================
    // Construtor
    // =========================================================

    [Fact]
    public void Constructor_WhenValidArguments_ShouldCreateUnitOfMeasure()
    {
        // Act
        var uom = new UnitOfMeasure("UN", "Unidade", isActive: true);

        // Assert
        uom.Should().NotBeNull();
        uom.Code.Should().Be("UN");
        uom.Name.Should().Be("Unidade");
        uom.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Constructor_WhenCreated_ShouldGenerateNonEmptyId()
    {
        // Act
        var uom = new UnitOfMeasure("KG", "Quilograma", isActive: true);

        // Assert
        uom.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Constructor_WhenCreated_ShouldSetCreatedAt()
    {
        // Arrange
        var before = DateTime.Now.AddSeconds(-1);

        // Act
        var uom = new UnitOfMeasure("LT", "Litro", isActive: true);

        // Assert
        uom.CreatedAt.Should().BeAfter(before);
    }

    [Fact]
    public void Constructor_WhenCreated_ShouldHaveNullUpdatedAt()
    {
        // Act
        var uom = new UnitOfMeasure("MT", "Metro", isActive: false);

        // Assert
        uom.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void Constructor_WhenCreated_ShouldHaveEmptyProductsCollection()
    {
        // Act
        var uom = new UnitOfMeasure("PC", "Peça", isActive: true);

        // Assert
        uom.Products.Should().NotBeNull();
        uom.Products.Should().BeEmpty();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Constructor_ShouldRespectIsActiveArgument(bool isActive)
    {
        // Act
        var uom = new UnitOfMeasure("GL", "Galão", isActive);

        // Assert
        uom.IsActive.Should().Be(isActive);
    }

    // =========================================================
    // TwoDistinctInstances — IDs únicos
    // =========================================================

    [Fact]
    public void TwoInstances_WithSameData_ShouldHaveDifferentIds()
    {
        // Act
        var uom1 = new UnitOfMeasure("UN", "Unidade", true);
        var uom2 = new UnitOfMeasure("UN", "Unidade", true);

        // Assert
        uom1.Id.Should().NotBe(uom2.Id);
    }

    // =========================================================
    // Update
    // =========================================================

    [Fact]
    public void Update_WhenCalledWithValidArguments_ShouldChangeCodeAndName()
    {
        // Arrange
        var uom = new UnitOfMeasure("UN", "Unidade", isActive: true);

        // Act
        uom.Update("KG", "Quilograma");

        // Assert
        uom.Code.Should().Be("KG");
        uom.Name.Should().Be("Quilograma");
    }

    [Fact]
    public void Update_WhenCalled_ShouldSetUpdatedAt()
    {
        // Arrange
        var uom = new UnitOfMeasure("UN", "Unidade", isActive: true);
        var beforeUpdate = DateTime.Now.AddSeconds(-1);

        // Act
        uom.Update("LT", "Litro");

        // Assert
        uom.UpdatedAt.Should().NotBeNull();
        uom.UpdatedAt.Should().BeAfter(beforeUpdate);
    }

    [Fact]
    public void Update_ShouldNotChangeIsActive()
    {
        // Arrange
        var uom = new UnitOfMeasure("UN", "Unidade", isActive: true);

        // Act
        uom.Update("KG", "Quilograma");

        // Assert
        uom.IsActive.Should().BeTrue("Update não deve alterar IsActive");
    }

    [Fact]
    public void Update_ShouldNotChangeId()
    {
        // Arrange
        var uom = new UnitOfMeasure("UN", "Unidade", isActive: true);
        var originalId = uom.Id;

        // Act
        uom.Update("MT", "Metro");

        // Assert
        uom.Id.Should().Be(originalId);
    }

    [Fact]
    public void Update_WhenCalledMultipleTimes_ShouldAlwaysReflectLatestValues()
    {
        // Arrange
        var uom = new UnitOfMeasure("UN", "Unidade", isActive: true);

        // Act
        uom.Update("KG", "Quilograma");
        uom.Update("LT", "Litro");

        // Assert
        uom.Code.Should().Be("LT");
        uom.Name.Should().Be("Litro");
        uom.UpdatedAt.Should().NotBeNull();
    }

    [Theory]
    [InlineData("UN", "Unidade")]
    [InlineData("KG", "Quilograma")]
    [InlineData("LT", "Litro")]
    [InlineData("MT", "Metro")]
    [InlineData("CX", "Caixa")]
    public void Update_WhenCalledWithVariousValidValues_ShouldUpdateCorrectly(
        string code, string name)
    {
        // Arrange
        var uom = new UnitOfMeasure("PC", "Peça", isActive: true);

        // Act
        uom.Update(code, name);

        // Assert
        uom.Code.Should().Be(code);
        uom.Name.Should().Be(name);
    }
}
