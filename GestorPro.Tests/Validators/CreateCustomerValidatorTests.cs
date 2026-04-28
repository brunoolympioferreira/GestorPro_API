using GestorPro.Application.Models.InputModels.Customer;
using GestorPro.Application.Validators.Customer;

namespace GestorPro.Tests.Validators;

public class CreateCustomerValidatorTests
{
    private readonly CreateCustomerValidator _validator = new();

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
        var input = ValidInput() with { Name = new string('A', 201) };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task Validate_WhenNameHasExactly3Chars_ShouldNotHaveErrorOnName()
    {
        var input = ValidInput() with { Name = "ABC" };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    // =========================================================
    // TradeName
    // =========================================================

    [Fact]
    public async Task Validate_WhenTradeNameExceeds200Chars_ShouldHaveErrorOnTradeName()
    {
        var input = ValidInput() with { TradeName = new string('B', 201) };
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

    // =========================================================
    // Document
    // =========================================================

    [Fact]
    public async Task Validate_WhenDocumentIsEmpty_ShouldHaveErrorOnDocument()
    {
        var input = ValidInput() with { Document = "" };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Document);
    }

    [Fact]
    public async Task Validate_WhenDocumentIsWhiteSpace_ShouldHaveErrorOnDocument()
    {
        var input = ValidInput() with { Document = "   " };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Document);
    }

    // =========================================================
    // Status
    // =========================================================

    [Fact]
    public async Task Validate_WhenStatusIsEmpty_ShouldHaveErrorOnStatus()
    {
        var input = ValidInput() with { Status = "" };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Status);
    }

    [Theory]
    [InlineData("Ativo")]
    [InlineData("active")]    // case insensitive — valor não existe no enum
    [InlineData("Pendente")]
    [InlineData("123")]
    public async Task Validate_WhenStatusIsInvalid_ShouldHaveErrorOnStatus(string invalidStatus)
    {
        var input = ValidInput() with { Status = invalidStatus };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Status);
    }

    [Theory]
    [InlineData("Active")]
    [InlineData("Disabled")]
    public async Task Validate_WhenStatusIsValid_ShouldNotHaveErrorOnStatus(string validStatus)
    {
        var input = ValidInput() with { Status = validStatus };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldNotHaveValidationErrorFor(x => x.Status);
    }

    // =========================================================
    // Addresses (opcional, mas se informado deve ser válido)
    // =========================================================

    [Fact]
    public async Task Validate_WhenAddressesListIsEmpty_ShouldHaveErrorOnAddresses()
    {
        // Uma lista vazia (não nula) deve falhar: "não pode ser vazia"
        var input = ValidInput() with { Addresses = new List<AddressDTO>() };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor(x => x.Addresses);
    }

    [Fact]
    public async Task Validate_WhenAddressHasEmptyStreet_ShouldHaveErrorOnAddress()
    {
        var invalidAddress = AddressFaker.ValidDto() with { Street = "" };
        var input = ValidInput() with { Addresses = [invalidAddress] };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor("Addresses[0].Street");
    }

    [Fact]
    public async Task Validate_WhenAddressHasInvalidState_ShouldHaveErrorOnAddress()
    {
        var invalidAddress = AddressFaker.ValidDto() with { State = "XX" };
        var input = ValidInput() with { Addresses = [invalidAddress] };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor("Addresses[0].State");
    }

    [Fact]
    public async Task Validate_WhenAddressHasInvalidAddressType_ShouldHaveErrorOnAddress()
    {
        var invalidAddress = AddressFaker.ValidDto() with { AddressType = "Comercial" };
        var input = ValidInput() with { Addresses = [invalidAddress] };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor("Addresses[0].AddressType");
    }

    [Theory]
    [InlineData("Billing")]
    [InlineData("Shipping")]
    [InlineData("BillingAndShipping")]
    public async Task Validate_WhenAddressTypeIsValid_ShouldNotHaveErrorOnAddressType(string validType)
    {
        var address = AddressFaker.ValidDto() with { AddressType = validType };
        var input = ValidInput() with { Addresses = [address] };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldNotHaveValidationErrorFor("Addresses[0].AddressType");
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
        // Dois contatos, nenhum primário
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
        // Dois contatos primários — regra: exatamente 1
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
        // Um primário e um secundário — válido
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
    [InlineData("email invalido")]
    [InlineData("semArroba.com")]
    [InlineData("duplo@@email.com")]
    public async Task Validate_WhenContactEmailIsInvalid_ShouldHaveErrorOnContactEmail(string invalidEmail)
    {
        var contact = ContactFaker.PrimaryDto() with { Email = invalidEmail };
        var input = ValidInput() with { Contacts = [contact] };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldHaveValidationErrorFor("Contacts[0].Email");
    }

    [Fact]
    public async Task Validate_WhenContactEmailIsNull_ShouldNotHaveErrorOnContactEmail()
    {
        // E-mail é opcional no contato
        var contact = ContactFaker.PrimaryDto() with { Email = null };
        var input = ValidInput() with { Contacts = [contact] };
        var result = await _validator.TestValidateAsync(input);
        result.ShouldNotHaveValidationErrorFor("Contacts[0].Email");
    }

    // =========================================================
    // Helpers
    // =========================================================

    /// <summary>Retorna um input 100% válido para ser modificado nos testes.</summary>
    private static CreateCustomerInputModel ValidInput() =>
        new(
            Name: "Empresa Teste Ltda",
            TradeName: "Empresa Teste",
            Document: CustomerFaker.ValidCpfs[0],
            Status: nameof(CustomerStatusEnum.Active),
            Addresses: [AddressFaker.ValidDto()],
            Contacts: [ContactFaker.PrimaryDto()]);
}
