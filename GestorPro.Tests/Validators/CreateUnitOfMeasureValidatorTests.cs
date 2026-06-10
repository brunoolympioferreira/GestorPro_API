using GestorPro.Application.Validators.UnitOfMeasure;

namespace GestorPro.Tests.Validators;

public sealed class CreateUnitOfMeasureValidatorTests
{
    private readonly CreateUnitOfMeasureValidator _validator = new();

    // =========================================================
    // Input válido (happy path)
    // =========================================================

    [Fact]
    public async Task Validate_WhenInputIsValid_ShouldHaveNoErrors()
    {
        // Arrange
        var input = UnitOfMeasureFaker.ValidCreateInput();

        // Act
        var result = await _validator.TestValidateAsync(input);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    // =========================================================
    // Code
    // =========================================================

    [Fact]
    public async Task Validate_WhenCodeIsEmpty_ShouldHaveErrorOnCode()
    {
        var input = UnitOfMeasureFaker.ValidCreateInput(code: "");
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public async Task Validate_WhenCodeIsWhiteSpace_ShouldHaveErrorOnCode()
    {
        var input = UnitOfMeasureFaker.ValidCreateInput(code: "   ");
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public async Task Validate_WhenCodeExceeds10Chars_ShouldHaveErrorOnCode()
    {
        var input = UnitOfMeasureFaker.ValidCreateInput(code: new string('A', 11));
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public async Task Validate_WhenCodeHasExactly10Chars_ShouldNotHaveErrorOnCode()
    {
        var input = UnitOfMeasureFaker.ValidCreateInput(code: new string('A', 10));
        var result = await _validator.TestValidateAsync(input);
        result.ShouldNotHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public async Task Validate_WhenCodeHasExactly1Char_ShouldNotHaveErrorOnCode()
    {
        var input = UnitOfMeasureFaker.ValidCreateInput(code: "X");
        var result = await _validator.TestValidateAsync(input);
        result.ShouldNotHaveValidationErrorFor(x => x.Code);
    }

    [Theory]
    [InlineData("UN")]
    [InlineData("KG")]
    [InlineData("LT")]
    [InlineData("PC")]
    public async Task Validate_WhenCodeIsValid_ShouldNotHaveErrorOnCode(string validCode)
    {
        var input = UnitOfMeasureFaker.ValidCreateInput(code: validCode);
        var result = await _validator.TestValidateAsync(input);
        result.ShouldNotHaveValidationErrorFor(x => x.Code);
    }

    // =========================================================
    // Name
    // =========================================================

    [Fact]
    public async Task Validate_WhenNameIsEmpty_ShouldHaveErrorOnName()
    {
        var input = UnitOfMeasureFaker.ValidCreateInput(name: "");
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task Validate_WhenNameIsWhiteSpace_ShouldHaveErrorOnName()
    {
        var input = UnitOfMeasureFaker.ValidCreateInput(name: "   ");
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task Validate_WhenNameExceeds50Chars_ShouldHaveErrorOnName()
    {
        var input = UnitOfMeasureFaker.ValidCreateInput(name: new string('N', 51));
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task Validate_WhenNameHasExactly50Chars_ShouldNotHaveErrorOnName()
    {
        var input = UnitOfMeasureFaker.ValidCreateInput(name: new string('N', 50));
        var result = await _validator.TestValidateAsync(input);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task Validate_WhenNameHasExactly1Char_ShouldNotHaveErrorOnName()
    {
        var input = UnitOfMeasureFaker.ValidCreateInput(name: "A");
        var result = await _validator.TestValidateAsync(input);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Theory]
    [InlineData("Unidade")]
    [InlineData("Quilograma")]
    [InlineData("Litro")]
    [InlineData("Metro")]
    public async Task Validate_WhenNameIsValid_ShouldNotHaveErrorOnName(string validName)
    {
        var input = UnitOfMeasureFaker.ValidCreateInput(name: validName);
        var result = await _validator.TestValidateAsync(input);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    // =========================================================
    // Combinações de erros simultâneos
    // =========================================================

    [Fact]
    public async Task Validate_WhenCodeAndNameAreEmpty_ShouldHaveErrorsOnBothFields()
    {
        var input = UnitOfMeasureFaker.ValidCreateInput(code: "", name: "");
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Code);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
}

public sealed class UpdateUnitOfMeasureValidatorTests
{
    private readonly UpdateUnitOfMeasureValidator _validator = new();

    // =========================================================
    // Input válido (happy path)
    // =========================================================

    [Fact]
    public async Task Validate_WhenInputIsValid_ShouldHaveNoErrors()
    {
        // Arrange
        var input = UnitOfMeasureFaker.ValidUpdateInput();

        // Act
        var result = await _validator.TestValidateAsync(input);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    // =========================================================
    // Code
    // =========================================================

    [Fact]
    public async Task Validate_WhenCodeIsEmpty_ShouldHaveErrorOnCode()
    {
        var input = UnitOfMeasureFaker.ValidUpdateInput(code: "");
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public async Task Validate_WhenCodeIsWhiteSpace_ShouldHaveErrorOnCode()
    {
        var input = UnitOfMeasureFaker.ValidUpdateInput(code: "  ");
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public async Task Validate_WhenCodeExceeds10Chars_ShouldHaveErrorOnCode()
    {
        var input = UnitOfMeasureFaker.ValidUpdateInput(code: new string('Z', 11));
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public async Task Validate_WhenCodeHasExactly10Chars_ShouldNotHaveErrorOnCode()
    {
        var input = UnitOfMeasureFaker.ValidUpdateInput(code: new string('Z', 10));
        var result = await _validator.TestValidateAsync(input);
        result.ShouldNotHaveValidationErrorFor(x => x.Code);
    }

    // =========================================================
    // Name
    // =========================================================

    [Fact]
    public async Task Validate_WhenNameIsEmpty_ShouldHaveErrorOnName()
    {
        var input = UnitOfMeasureFaker.ValidUpdateInput(name: "");
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task Validate_WhenNameIsWhiteSpace_ShouldHaveErrorOnName()
    {
        var input = UnitOfMeasureFaker.ValidUpdateInput(name: "  ");
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task Validate_WhenNameExceeds50Chars_ShouldHaveErrorOnName()
    {
        var input = UnitOfMeasureFaker.ValidUpdateInput(name: new string('N', 51));
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task Validate_WhenNameHasExactly50Chars_ShouldNotHaveErrorOnName()
    {
        var input = UnitOfMeasureFaker.ValidUpdateInput(name: new string('N', 50));
        var result = await _validator.TestValidateAsync(input);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    // =========================================================
    // Combinações de erros simultâneos
    // =========================================================

    [Fact]
    public async Task Validate_WhenCodeAndNameAreEmpty_ShouldHaveErrorsOnBothFields()
    {
        var input = UnitOfMeasureFaker.ValidUpdateInput(code: "", name: "");
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Code);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task Validate_WhenBothFieldsAreAtMaxLength_ShouldNotHaveErrors()
    {
        var input = UnitOfMeasureFaker.ValidUpdateInput(
            code: new string('A', 10),
            name: new string('B', 50));
        var result = await _validator.TestValidateAsync(input);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
