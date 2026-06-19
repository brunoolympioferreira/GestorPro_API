using GestorPro.Application.Models.ViewModels;
using GestorPro.Domain.Interfaces.Repositories;

namespace GestorPro.Tests.Services;

public sealed class UnitOfMeasureServiceTests : UnitTestBase
{
    private readonly IUnityOfWork _unityOfWork;
    private readonly IUnitOfMeasureRepository _unitOfMeasureRepository;
    private readonly UnitOfMeasureService _sut;

    public UnitOfMeasureServiceTests()
    {
        _unityOfWork = Fixture.Freeze<IUnityOfWork>();
        _unitOfMeasureRepository = Fixture.Freeze<IUnitOfMeasureRepository>();
        _unityOfWork.UnitOfMeasures.Returns(_unitOfMeasureRepository);
        _sut = new UnitOfMeasureService(_unityOfWork);
    }

    // =========================================================
    // CreateAsync
    // =========================================================

    [Fact]
    public async Task CreateAsync_WhenValidInput_ShouldReturnNewId()
    {
        // Arrange
        var input = UnitOfMeasureFaker.ValidCreateInput();

        // Act
        var resultId = await _sut.CreateAsync(input, CancellationToken.None);

        // Assert
        resultId.Should().NotBeEmpty();

        await _unitOfMeasureRepository.Received(1).AddAsync(Arg.Is<UnitOfMeasure>(u =>
            u.Code == input.Code &&
            u.Name == input.Name &&
            u.IsActive == input.IsActive));

        await _unityOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_WhenIsActiveFalse_ShouldCreateInactiveUnitOfMeasure()
    {
        // Arrange
        var input = UnitOfMeasureFaker.ValidCreateInput(isActive: false);

        // Act
        var resultId = await _sut.CreateAsync(input, CancellationToken.None);

        // Assert
        resultId.Should().NotBeEmpty();

        await _unitOfMeasureRepository.Received(1).AddAsync(Arg.Is<UnitOfMeasure>(u =>
            !u.IsActive));
    }

    [Fact]
    public async Task CreateAsync_WhenCalled_ShouldSaveChangesExactlyOnce()
    {
        // Arrange
        var input = UnitOfMeasureFaker.ValidCreateInput();

        // Act
        await _sut.CreateAsync(input, CancellationToken.None);

        // Assert
        await _unityOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    // =========================================================
    // GetByIdAsync
    // =========================================================

    [Fact]
    public async Task GetByIdAsync_WhenUnitOfMeasureExists_ShouldReturnDetailViewModel()
    {
        // Arrange
        var uom = UnitOfMeasureFaker.Active();

        _unitOfMeasureRepository.GetByIdAsyncNoTracking(uom.Id).Returns(uom);

        // Act
        var result = await _sut.GetByIdAsync(uom.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(uom.Id);
        result.Code.Should().Be(uom.Code);
        result.Name.Should().Be(uom.Name);
        result.IsActive.Should().Be(uom.IsActive);
        result.Should().BeOfType<UnitOfMeasureDetailViewModel>();
    }

    [Fact]
    public async Task GetByIdAsync_WhenUnitOfMeasureDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        _unitOfMeasureRepository.GetByIdAsync(nonExistentId).ReturnsNull();

        // Act
        var act = () => _sut.GetByIdAsync(nonExistentId);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task GetByIdAsync_WhenUnitOfMeasureIsInactive_ShouldStillReturnDetailViewModel()
    {
        // Arrange
        var uom = UnitOfMeasureFaker.Inactive();
        _unitOfMeasureRepository.GetByIdAsyncNoTracking(uom.Id).Returns(uom);

        // Act
        var result = await _sut.GetByIdAsync(uom.Id);

        // Assert
        result.Should().NotBeNull();
        result.IsActive.Should().BeFalse();
    }

    // =========================================================
    // GetAllAsync
    // =========================================================

    [Fact]
    public async Task GetAllAsync_WhenUnitOfMeasuresExist_ShouldReturnAllViewModels()
    {
        // Arrange
        var uoms = new UnitOfMeasureFaker().Generate(5);
        _unitOfMeasureRepository.GetAllAsync().Returns(uoms);

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().HaveCount(5);
        result.Should().AllBeAssignableTo<UnitOfMeasureViewModel>();
        result.Should().AllSatisfy(vm =>
        {
            vm.Id.Should().NotBeEmpty();
            vm.Code.Should().NotBeNullOrEmpty();
            vm.Name.Should().NotBeNullOrEmpty();
        });
    }

    [Fact]
    public async Task GetAllAsync_WhenNoUnitOfMeasuresExist_ShouldReturnEmptyCollection()
    {
        // Arrange
        _unitOfMeasureRepository.GetAllAsync().Returns([]);

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_WhenCalled_ShouldCallRepositoryOnce()
    {
        // Arrange
        _unitOfMeasureRepository.GetAllAsync().Returns([]);

        // Act
        await _sut.GetAllAsync();

        // Assert
        await _unitOfMeasureRepository.Received(1).GetAllAsync();
    }

    // =========================================================
    // Update
    // =========================================================

    [Fact]
    public async Task Update_WhenUnitOfMeasureExists_ShouldUpdateAndSave()
    {
        // Arrange
        var uom = UnitOfMeasureFaker.Active();
        var input = UnitOfMeasureFaker.ValidUpdateInput("LT", "Litro");

        _unitOfMeasureRepository.GetByIdAsync(uom.Id).Returns(uom);

        // Act
        await _sut.Update(uom.Id, input, CancellationToken.None);

        // Assert
        uom.Code.Should().Be(input.Code);
        uom.Name.Should().Be(input.Name);
        await _unityOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Update_WhenUnitOfMeasureDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var input = UnitOfMeasureFaker.ValidUpdateInput();

        _unitOfMeasureRepository.GetByIdAsync(id).ReturnsNull();

        // Act
        var act = () => _sut.Update(id, input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
        await _unityOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Update_WhenCalled_ShouldUpdateCodeAndNameOnEntity()
    {
        // Arrange
        var uom = UnitOfMeasureFaker.WithCode("UN");
        var input = UnitOfMeasureFaker.ValidUpdateInput(code: "MT", name: "Metro");

        _unitOfMeasureRepository.GetByIdAsync(uom.Id).Returns(uom);

        // Act
        await _sut.Update(uom.Id, input, CancellationToken.None);

        // Assert
        uom.Code.Should().Be("MT");
        uom.Name.Should().Be("Metro");
    }

    [Fact]
    public async Task Update_WhenCalled_ShouldSetUpdatedAtTimestamp()
    {
        // Arrange
        var uom = UnitOfMeasureFaker.Active();
        var input = UnitOfMeasureFaker.ValidUpdateInput();

        _unitOfMeasureRepository.GetByIdAsync(uom.Id).Returns(uom);

        // Act
        await _sut.Update(uom.Id, input, CancellationToken.None);

        // Assert
        uom.UpdatedAt.Should().NotBeNull();
    }

    // =========================================================
    // DeleteAsync
    // =========================================================

    [Fact]
    public async Task DeleteAsync_WhenUnitOfMeasureExists_ShouldDeleteAndSave()
    {
        // Arrange
        var uom = UnitOfMeasureFaker.Active();

        _unitOfMeasureRepository.ExistsAsync(uom.Id).Returns(true);

        // Act
        await _sut.DeleteAsync(uom.Id, CancellationToken.None);

        // Assert
        await _unitOfMeasureRepository.Received(1).DeleteAsync(uom.Id);
        await _unityOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteAsync_WhenUnitOfMeasureDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _unitOfMeasureRepository.ExistsAsync(id).Returns(false);

        // Act
        var act = () => _sut.DeleteAsync(id, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task DeleteAsync_WhenUnitOfMeasureDoesNotExist_ShouldNotCallDeleteOrSave()
    {
        // Arrange
        var id = Guid.NewGuid();
        _unitOfMeasureRepository.ExistsAsync(id).Returns(false);

        // Act
        try { await _sut.DeleteAsync(id, CancellationToken.None); } catch { }

        // Assert
        await _unitOfMeasureRepository.DidNotReceive().DeleteAsync(Arg.Any<Guid>());
        await _unityOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
