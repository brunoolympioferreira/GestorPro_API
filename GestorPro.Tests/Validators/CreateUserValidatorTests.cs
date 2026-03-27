namespace GestorPro.Tests.Validators;

public sealed class CreateUserValidatorTests
{
    private readonly CreateUserValidator _validator = new();

    // =========================================================
    // Input válido (happy path)
    // =========================================================

    [Fact]
    public async Task Validate_WhenInputIsValid_ShouldHaveNoErrors()
    {
        // Arrange
        var input = ValidInput();

        // Act
        var result = await _validator.TestValidateAsync(input);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    // =========================================================
    // Name
    // =========================================================

    [Fact]
    public async Task Validate_WhenNameIsEmpty_ShouldHaveErrorOnName()
    {
        var input = ValidInput() with { Name = "" };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task Validate_WhenNameExceeds150Chars_ShouldHaveErrorOnName()
    {
        var input = ValidInput() with { Name = new string('A', 151) };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    // =========================================================
    // Email
    // =========================================================

    [Fact]
    public async Task Validate_WhenEmailIsEmpty_ShouldHaveErrorOnEmail()
    {
        var input = ValidInput() with { Email = "" };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData("semArroba")]
    [InlineData("@semUsuario.com")]
    [InlineData("espacos no email@test.com")]
    [InlineData("duplo@@email.com")]
    public async Task Validate_WhenEmailIsInvalid_ShouldHaveErrorOnEmail(string invalidEmail)
    {
        var input = ValidInput() with { Email = invalidEmail };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public async Task Validate_WhenEmailExceeds255Chars_ShouldHaveErrorOnEmail()
    {
        // 256+ chars, mas mantendo formato de e-mail
        var longLocal = new string('a', 250);
        var input = ValidInput() with { Email = $"{longLocal}@test.com" };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    // =========================================================
    // Password
    // =========================================================

    [Fact]
    public async Task Validate_WhenPasswordIsEmpty_ShouldHaveErrorOnPassword()
    {
        var input = ValidInput() with { Password = "" };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [InlineData("curta")]           // menos de 8 chars
    [InlineData("semmaiuscula!")]  // sem maiúscula
    [InlineData("SemNumero!")]      // sem número
    [InlineData("SemEspecial1")]    // sem caractere especial
    public async Task Validate_WhenPasswordDoesNotMeetRequirements_ShouldHaveErrorOnPassword(
        string weakPassword)
    {
        var input = ValidInput() with { Password = weakPassword };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    // =========================================================
    // Role
    // =========================================================

    [Fact]
    public async Task Validate_WhenRoleIsEmpty_ShouldHaveErrorOnRole()
    {
        var input = ValidInput() with { Role = "" };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Role);
    }

    [Theory]
    [InlineData("SuperAdmin")]
    [InlineData("Usuario")]
    [InlineData("root")]
    [InlineData("123")]
    public async Task Validate_WhenRoleIsInvalid_ShouldHaveErrorOnRole(string invalidRole)
    {
        var input = ValidInput() with { Role = invalidRole };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Role);
    }

    [Theory]
    [InlineData("Admin")]
    [InlineData("Manager")]
    [InlineData("Employee")]
    [InlineData("Viewer")]
    public async Task Validate_WhenRoleIsValid_ShouldNotHaveErrorOnRole(string validRole)
    {
        var input = ValidInput() with { Role = validRole };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldNotHaveValidationErrorFor(x => x.Role);
    }

    // =========================================================
    // Helper
    // =========================================================

    /// <summary>Retorna um input 100% válido para ser modificado nos testes.</summary>
    private static CreateUserInputModel ValidInput() =>
        new("João Silva", "joao@email.com", "Senha@123", "Employee", true);
}
