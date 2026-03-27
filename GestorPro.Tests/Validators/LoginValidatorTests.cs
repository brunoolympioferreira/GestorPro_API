namespace GestorPro.Tests.Validators;

public sealed class LoginValidatorTests
{
    private readonly LoginValidator _validator = new();

    [Fact]
    public async Task Validate_WhenInputIsValid_ShouldHaveNoErrors()
    {
        var input = new LoginInputModel("usuario@email.com", "Senha@123");
        var result = await _validator.TestValidateAsync(input);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("", "Senha@123")]          // e-mail vazio
    [InlineData("invalido", "Senha@123")]  // e-mail inválido
    public async Task Validate_WhenEmailIsInvalid_ShouldHaveErrorOnEmail(
        string email, string password)
    {
        var input = new LoginInputModel(email, password);
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData("usuario@email.com", "")]          // senha vazia
    [InlineData("usuario@email.com", "fraca")]      // senha fraca
    [InlineData("usuario@email.com", "SemNumero!")] // sem número
    public async Task Validate_WhenPasswordIsInvalid_ShouldHaveErrorOnPassword(
        string email, string password)
    {
        var input = new LoginInputModel(email, password);
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}
