using GestorPro.Application.Models.InputModels;
using GestorPro.Application.Validators.Customer;

namespace GestorPro.Tests.Validators;

public sealed class UpdateCustomerValidatorTests
{
    private readonly UpdateCustomerValidator _validator = new();

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
    public async Task Validate_WhenNameHasLessThan3Chars_ShouldHaveErrorOnName()
    {
        var input = ValidInput() with { Name = "AB" };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task Validate_WhenNameExceeds200Chars_ShouldHaveErrorOnName()
    {
        var input = ValidInput() with { Name = new string('X', 201) };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task Validate_WhenNameHasExactly200Chars_ShouldNotHaveErrorOnName()
    {
        var input = ValidInput() with { Name = new string('X', 200) };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    // =========================================================
    // TradeName
    // =========================================================

    [Fact]
    public async Task Validate_WhenTradeNameExceeds200Chars_ShouldHaveErrorOnTradeName()
    {
        var input = ValidInput() with { TradeName = new string('Y', 201) };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.TradeName);
    }

    [Fact]
    public async Task Validate_WhenTradeNameIsNull_ShouldNotHaveErrorOnTradeName()
    {
        var input = ValidInput() with { TradeName = null! };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldNotHaveValidationErrorFor(x => x.TradeName);
    }

    [Fact]
    public async Task Validate_WhenTradeNameIsEmpty_ShouldNotHaveErrorOnTradeName()
    {
        // TradeName não tem regra NotEmpty no UpdateCustomerValidator
        var input = ValidInput() with { TradeName = "" };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldNotHaveValidationErrorFor(x => x.TradeName);
    }

    // =========================================================
    // Addresses (opcional, mas se informado deve ser válido)
    // =========================================================

    [Fact]
    public async Task Validate_WhenAddressesListIsEmpty_ShouldHaveErrorOnAddresses()
    {
        var input = ValidInput() with { Addresses = new List<AddressDTO>() };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Addresses);
    }

    [Fact]
    public async Task Validate_WhenAddressHasEmptyNumber_ShouldHaveErrorOnAddress()
    {
        var invalidAddress = AddressFaker.ValidDto() with { Number = "" };
        var input = ValidInput() with { Addresses = [invalidAddress] };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor("Addresses[0].Number");
    }

    [Fact]
    public async Task Validate_WhenAddressHasEmptyNeighborhood_ShouldHaveErrorOnAddress()
    {
        var invalidAddress = AddressFaker.ValidDto() with { Neighborhood = "" };
        var input = ValidInput() with { Addresses = [invalidAddress] };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor("Addresses[0].Neighborhood");
    }

    [Fact]
    public async Task Validate_WhenAddressHasEmptyCity_ShouldHaveErrorOnAddress()
    {
        var invalidAddress = AddressFaker.ValidDto() with { City = "" };
        var input = ValidInput() with { Addresses = [invalidAddress] };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor("Addresses[0].City");
    }

    [Fact]
    public async Task Validate_WhenAddressStateHasSingleChar_ShouldHaveErrorOnAddress()
    {
        var invalidAddress = AddressFaker.ValidDto() with { State = "S" };
        var input = ValidInput() with { Addresses = [invalidAddress] };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor("Addresses[0].State");
    }

    [Fact]
    public async Task Validate_WhenAddressHasEmptyZipCode_ShouldHaveErrorOnAddress()
    {
        var invalidAddress = AddressFaker.ValidDto() with { ZipCode = "" };
        var input = ValidInput() with { Addresses = [invalidAddress] };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor("Addresses[0].ZipCode");
    }

    [Fact]
    public async Task Validate_WhenAddressIsValid_ShouldNotHaveAnyErrors()
    {
        var input = ValidInput() with { Addresses = [AddressFaker.ValidDto()] };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldNotHaveValidationErrorFor(x => x.Addresses);
    }

    // =========================================================
    // Contacts (opcional, mas se informado deve ser válido)
    // =========================================================

    [Fact]
    public async Task Validate_WhenContactsListIsEmpty_ShouldHaveErrorOnContacts()
    {
        var input = ValidInput() with { Contacts = new List<ContactDTO>() };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Contacts);
    }

    [Fact]
    public async Task Validate_WhenNoPrimaryContact_ShouldHaveErrorOnContacts()
    {
        var contacts = new List<ContactDTO>
        {
            ContactFaker.SecondaryDto(),
            ContactFaker.SecondaryDto()
        };
        var input = ValidInput() with { Contacts = contacts };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Contacts);
    }

    [Fact]
    public async Task Validate_WhenMoreThanOnePrimaryContact_ShouldHaveErrorOnContacts()
    {
        var contacts = new List<ContactDTO>
        {
            ContactFaker.PrimaryDto(),
            ContactFaker.PrimaryDto()
        };
        var input = ValidInput() with { Contacts = contacts };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Contacts);
    }

    [Fact]
    public async Task Validate_WhenExactlyOnePrimaryContact_ShouldNotHaveErrorOnContacts()
    {
        var contacts = new List<ContactDTO>
        {
            ContactFaker.PrimaryDto(),
            ContactFaker.SecondaryDto()
        };
        var input = ValidInput() with { Contacts = contacts };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldNotHaveValidationErrorFor(x => x.Contacts);
    }

    [Theory]
    [InlineData("invalido")]
    [InlineData("email com espaco@test.com")]
    [InlineData("@@duplo.com")]
    public async Task Validate_WhenContactEmailIsInvalid_ShouldHaveErrorOnContactEmail(string invalidEmail)
    {
        var contact = ContactFaker.PrimaryDto() with { Email = invalidEmail };
        var input = ValidInput() with { Contacts = [contact] };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor("Contacts[0].Email");
    }

    [Fact]
    public async Task Validate_WhenContactEmailIsEmpty_ShouldNotHaveErrorOnContactEmail()
    {
        // Email em Contact é opcional — string vazia não dispara o validator
        var contact = ContactFaker.PrimaryDto() with { Email = "" };
        var input = ValidInput() with { Contacts = [contact] };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldNotHaveValidationErrorFor("Contacts[0].Email");
    }

    // =========================================================
    // Helpers
    // =========================================================

    /// <summary>Retorna um input 100% válido para ser modificado nos testes.</summary>
    private static UpdateCustomerInputModel ValidInput() =>
        new(
            Id: null,
            Name: "Empresa Atualizada Ltda",
            TradeName: "Nome Fantasia Atualizado",
            Addresses: [AddressFaker.ValidDto()],
            Contacts: [ContactFaker.PrimaryDto()]);
}
