using GestorPro.Application.Validators.ProductCategory;

namespace GestorPro.Tests.Modules.ProductCategories.Validators;

public class CreateProductCategoryInputModelValidatorTests : UnitTestBase
{
    private readonly CreateProductCategoryValidator _validator = new();

    // ─── Caso feliz ───────────────────────────────────────────────────────────

    [Fact]
    public void Validate_WithValidInput_ShouldNotHaveAnyErrors()
    {
        // Arrange
        var input = ProductCategoryFaker.ValidCreateInput();

        // Act & Assert
        _validator.TestValidate(input).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithNullDescription_ShouldNotHaveAnyErrors()
    {
        // Arrange
        var input = ProductCategoryFaker.ValidCreateInput(description: null);

        // Act & Assert
        _validator.TestValidate(input).ShouldNotHaveAnyValidationErrors();
    }

    // ─── Name ─────────────────────────────────────────────────────────────────

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WithEmptyOrNullName_ShouldHaveErrorForName(string? invalidName)
    {
        // Arrange
        var input = ProductCategoryFaker.ValidCreateInput(name: invalidName!);

        // Act & Assert
        _validator.TestValidate(input).ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_WithNameExceedingMaxLength_ShouldHaveErrorForName()
    {
        // Arrange
        var input = ProductCategoryFaker.ValidCreateInput(name: new string('A', 101));

        // Act & Assert
        _validator.TestValidate(input).ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_WithNameAtMaxLength_ShouldNotHaveErrorForName()
    {
        // Arrange
        var input = ProductCategoryFaker.ValidCreateInput(name: new string('A', 100));

        // Act & Assert
        _validator.TestValidate(input).ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    // ─── Description ──────────────────────────────────────────────────────────

    [Fact]
    public void Validate_WithDescriptionExceedingMaxLength_ShouldHaveErrorForDescription()
    {
        // Arrange
        var input = ProductCategoryFaker.ValidCreateInput(description: new string('A', 301));

        // Act & Assert
        _validator.TestValidate(input).ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Validate_WithDescriptionAtMaxLength_ShouldNotHaveErrorForDescription()
    {
        // Arrange
        var input = ProductCategoryFaker.ValidCreateInput(description: new string('A', 500));

        // Act & Assert
        _validator.TestValidate(input).ShouldNotHaveValidationErrorFor(x => x.Description);
    }
}

public class UpdateProductCategoryInputModelValidatorTests : UnitTestBase
{
    private readonly UpdateProductCategoryValidator _validator = new();

    // ─── Caso feliz ───────────────────────────────────────────────────────────

    [Fact]
    public void Validate_WithValidInput_ShouldNotHaveAnyErrors()
    {
        // Arrange
        var input = ProductCategoryFaker.ValidUpdateInput();

        // Act & Assert
        _validator.TestValidate(input).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithNullDescription_ShouldNotHaveAnyErrors()
    {
        // Arrange
        var input = ProductCategoryFaker.ValidUpdateInput(description: null);

        // Act & Assert
        _validator.TestValidate(input).ShouldNotHaveAnyValidationErrors();
    }

    // ─── Name ─────────────────────────────────────────────────────────────────

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WithEmptyOrNullName_ShouldHaveErrorForName(string? invalidName)
    {
        // Arrange
        var input = ProductCategoryFaker.ValidUpdateInput(name: invalidName!);

        // Act & Assert
        _validator.TestValidate(input).ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_WithNameExceedingMaxLength_ShouldHaveErrorForName()
    {
        // Arrange
        var input = ProductCategoryFaker.ValidUpdateInput(name: new string('A', 101));

        // Act & Assert
        _validator.TestValidate(input).ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_WithNameAtMaxLength_ShouldNotHaveErrorForName()
    {
        // Arrange
        var input = ProductCategoryFaker.ValidUpdateInput(name: new string('A', 100));

        // Act & Assert
        _validator.TestValidate(input).ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    // ─── Description ──────────────────────────────────────────────────────────

    [Fact]
    public void Validate_WithDescriptionExceedingMaxLength_ShouldHaveErrorForDescription()
    {
        // Arrange
        var input = ProductCategoryFaker.ValidUpdateInput(description: new string('A', 301));

        // Act & Assert
        _validator.TestValidate(input).ShouldHaveValidationErrorFor(x => x.Description);
    }
}
