using GestorPro.Application.Models.InputModels;
using GestorPro.Application.Models.ViewModels;
using GestorPro.Domain.Interfaces.Repositories;

namespace GestorPro.Tests.Services;

public sealed class CustomerServiceTests : UnitTestBase
{
    private readonly IUnityOfWork _unityOfWork;
    private readonly ICustomerRepository _customerRepository;
    private readonly CustomerService _sut;

    public CustomerServiceTests()
    {
        _unityOfWork = Fixture.Freeze<IUnityOfWork>();
        _customerRepository = Fixture.Freeze<ICustomerRepository>();
        _unityOfWork.Customers.Returns(_customerRepository);
        _sut = new CustomerService(_unityOfWork);
    }

    // =========================================================
    // CreateAsync
    // =========================================================

    [Fact]
    public async Task CreateAsync_WhenValidInput_ShouldReturnNewCustomerId()
    {
        // Arrange
        var input = ValidCreateInput();

        // Act
        var resultId = await _sut.CreateAsync(input, CancellationToken.None);

        // Assert
        resultId.Should().NotBeEmpty();

        await _customerRepository.Received(1).AddAsync(Arg.Is<Customer>(c =>
            c.Name == input.Name &&
            c.TradeName == input.TradeName));

        await _unityOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_WhenInputHasAddresses_ShouldAddAddressesToCustomer()
    {
        // Arrange
        var input = ValidCreateInput(withAddresses: true);

        // Act
        var resultId = await _sut.CreateAsync(input, CancellationToken.None);

        // Assert
        resultId.Should().NotBeEmpty();
        await _customerRepository.Received(1).AddAsync(Arg.Is<Customer>(c =>
            c.Addresses.Count == 1));
    }

    [Fact]
    public async Task CreateAsync_WhenInputHasContacts_ShouldAddContactsToCustomer()
    {
        // Arrange
        var input = ValidCreateInput(withContacts: true);

        // Act
        var resultId = await _sut.CreateAsync(input, CancellationToken.None);

        // Assert
        resultId.Should().NotBeEmpty();
        await _customerRepository.Received(1).AddAsync(Arg.Is<Customer>(c =>
            c.Contacts.Count == 1));
    }

    [Fact]
    public async Task CreateAsync_WhenInputHasNoAddressesOrContacts_ShouldCreateCustomerWithEmptyCollections()
    {
        // Arrange
        var input = ValidCreateInput(); // sem endereços e contatos

        // Act
        var resultId = await _sut.CreateAsync(input, CancellationToken.None);

        // Assert
        resultId.Should().NotBeEmpty();
        await _customerRepository.Received(1).AddAsync(Arg.Is<Customer>(c =>
            c.Addresses.Count == 0 && c.Contacts.Count == 0));
    }

    // =========================================================
    // UpdateAsync
    // =========================================================

    [Fact]
    public async Task UpdateAsync_WhenCustomerExists_ShouldUpdateAndSave()
    {
        // Arrange
        var customer = CustomerFaker.WithAddressesAndContacts();
        var input = ValidUpdateInput();

        _customerRepository
            .GetByIdAsync(customer.Id, includeAddress: true, includeContact: true, trackChanges: true)
            .Returns(customer);

        // Act
        await _sut.UpdateAsync(customer.Id, input, CancellationToken.None);

        // Assert
        customer.Name.Should().Be(input.Name);
        customer.TradeName.Should().Be(input.TradeName);
        await _unityOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateAsync_WhenCustomerDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var input = ValidUpdateInput();

        _customerRepository
            .GetByIdAsync(id, includeAddress: true, includeContact: true, trackChanges: true)
            .ReturnsNull();

        // Act
        var act = () => _sut.UpdateAsync(id, input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
        await _unityOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    // =========================================================
    // GetAllAsync
    // =========================================================

    [Fact]
    public async Task GetAllAsync_WhenCustomersExist_ShouldReturnAllViewModels()
    {
        // Arrange
        var customers = new CustomerFaker().Generate(5);
        _customerRepository.GetAllAsync().Returns(customers);

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().HaveCount(5);
        result.Should().AllBeAssignableTo<CustomerViewModel>();
        result.Should().AllSatisfy(vm =>
        {
            vm.Id.Should().NotBeEmpty();
            vm.Name.Should().NotBeNullOrEmpty();
            vm.Document.Should().NotBeNullOrEmpty();
        });
    }

    [Fact]
    public async Task GetAllAsync_WhenNoCustomersExist_ShouldReturnEmptyCollection()
    {
        // Arrange
        _customerRepository.GetAllAsync().Returns([]);

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    // =========================================================
    // GetByIdAsync
    // =========================================================

    [Fact]
    public async Task GetByIdAsync_WhenCustomerExistsWithDetails_ShouldReturnDetailViewModel()
    {
        // Arrange
        var customer = CustomerFaker.WithAddressesAndContacts();

        _customerRepository
            .GetByIdAsync(customer.Id, includeAddress: true, includeContact: true, trackChanges: false)
            .Returns(customer);

        // Act
        var result = await _sut.GetByIdAsync(customer.Id, includeAddress: true, includeContact: true);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(customer.Id);
        result.Name.Should().Be(customer.Name);
        result.Document.Should().Be(customer.Document.Value);
        result.Addresses.Should().HaveCount(1);
        result.Contacts.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCustomerDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        _customerRepository
            .GetByIdAsync(nonExistentId, includeAddress: true, includeContact: true, trackChanges: false)
            .ReturnsNull();

        // Act
        var act = () => _sut.GetByIdAsync(nonExistentId, includeAddress: true, includeContact: true);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task GetByIdAsync_WhenCustomerHasNoAddressesOrContacts_ShouldReturnEmptyCollections()
    {
        // Arrange
        var customer = CustomerFaker.Active();

        _customerRepository
            .GetByIdAsync(customer.Id, includeAddress: true, includeContact: true, trackChanges: false)
            .Returns(customer);

        // Act
        var result = await _sut.GetByIdAsync(customer.Id, includeAddress: true, includeContact: true);

        // Assert
        result.Addresses.Should().BeEmpty();
        result.Contacts.Should().BeEmpty();
    }

    // =========================================================
    // DeleteAsync (soft delete via customer.Delete())
    // =========================================================

    [Fact]
    public async Task DeleteAsync_WhenCustomerExists_ShouldDisableAndSave()
    {
        // Arrange
        var customer = CustomerFaker.Active();

        _customerRepository
            .GetByIdAsync(customer.Id, false, false, true)
            .Returns(customer);

        // Act
        await _sut.DeleteAsync(customer.Id, CancellationToken.None);

        // Assert — Delete() da entidade muda Status para Disabled
        customer.Status.Should().Be(CustomerStatusEnum.Disabled);
        await _unityOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteAsync_WhenCustomerDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();

        _customerRepository
            .GetByIdAsync(id, false, false, true)
            .ReturnsNull();

        // Act
        var act = () => _sut.DeleteAsync(id, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
        await _unityOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteAsync_WhenCustomerAlreadyDisabled_ShouldStillSave()
    {
        // Arrange — Customer começa como Active, Delete() o desativa
        var customer = CustomerFaker.Active();
        _customerRepository
            .GetByIdAsync(customer.Id, false, false, true)
            .Returns(customer);

        // Act
        await _sut.DeleteAsync(customer.Id, CancellationToken.None);

        // Assert
        customer.Status.Should().Be(CustomerStatusEnum.Disabled);
        await _unityOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    // =========================================================
    // Helpers
    // =========================================================

    private static CreateCustomerInputModel ValidCreateInput(
        bool withAddresses = false,
        bool withContacts = false)
    {
        var addresses = withAddresses
            ? new List<AddressDTO> { AddressFaker.ValidDto() }
            : new List<AddressDTO>();

        var contacts = withContacts
            ? new List<ContactDTO> { ContactFaker.PrimaryDto() }
            : new List<ContactDTO>();

        return new CreateCustomerInputModel(
            Name: "Empresa Teste Ltda",
            TradeName: "Empresa Teste",
            Document: CustomerFaker.ValidCpfs[0],
            Status: nameof(CustomerStatusEnum.Active),
            Addresses: addresses,
            Contacts: contacts);
    }

    private static UpdateCustomerInputModel ValidUpdateInput() =>
        new(
            Id: null,
            Name: "Empresa Atualizada Ltda",
            TradeName: "Empresa Atualizada",
            Addresses: new List<AddressDTO> { AddressFaker.ValidDto() },
            Contacts: new List<ContactDTO> { ContactFaker.PrimaryDto() });
}
