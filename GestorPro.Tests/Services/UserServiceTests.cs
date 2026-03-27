namespace GestorPro.Tests.Services;

public sealed class UserServiceTests : UnitTestBase
{
    private readonly IUnityOfWork _unityOfWork;
    private readonly IAuthService _authService;
    private readonly UserService _sut;
    private readonly UserFaker _userFaker = new();
    private readonly RoleFaker _roleFaker = new();

    public UserServiceTests()
    {
        _unityOfWork = Fixture.Freeze<IUnityOfWork>();
        _authService = Fixture.Freeze<IAuthService>();
        _sut = new UserService(_unityOfWork, _authService);
    }

    // =========================================================
    // CreateAsync
    // =========================================================

    [Fact]
    public async Task CreateAsync_WhenValidInput_ShouldReturnNewUserId()
    {
        // Arrange
        var role = RoleFaker.ForRole(RoleEnum.Employee);
        var input = new CreateUserInputModel("João Silva", "joao@email.com", "Senha@123", "Employee", true);
        const string fakeHash = "abc123hash";

        _authService.ComputeSha256Hash(input.Password).Returns(fakeHash);
        _unityOfWork.Roles.GetByNameAsync(input.Role).Returns(role);
        _unityOfWork.Users.ExistsAsync(Arg.Any<System.Linq.Expressions.Expression<Func<User, bool>>>()).Returns(false);

        // Act
        var resultId = await _sut.CreateAsync(input, CancellationToken.None);

        // Assert
        resultId.Should().NotBeEmpty();

        await _unityOfWork.Users.Received(1).AddAsync(Arg.Is<User>(u =>
            u.Name == input.Name &&
            u.PasswordHash == fakeHash));

        await _unityOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_WhenEmailAlreadyExists_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var role = RoleFaker.ForRole(RoleEnum.Employee);
        var input = new CreateUserInputModel("Maria", "duplicado@email.com", "Senha@123", "Employee", true);

        _authService.ComputeSha256Hash(Arg.Any<string>()).Returns("hash");
        _unityOfWork.Roles.GetByNameAsync(input.Role).Returns(role);
        _unityOfWork.Users.ExistsAsync(Arg.Any<System.Linq.Expressions.Expression<Func<User, bool>>>()).Returns(true);

        // Act
        var act = () => _sut.CreateAsync(input, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("*Email*já existe*");

        await _unityOfWork.Users.DidNotReceive().AddAsync(Arg.Any<User>());
        await _unityOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_WhenRoleDoesNotExist_ShouldThrowArgumentNullException()
    {
        // Arrange
        var input = new CreateUserInputModel("Carlos", "carlos@email.com", "Senha@123", "RoleInexistente", true);

        _authService.ComputeSha256Hash(Arg.Any<string>()).Returns("hash");
        _unityOfWork.Roles.GetByNameAsync(input.Role).ReturnsNull();

        // Act
        var act = () => _sut.CreateAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    // =========================================================
    // GetByIdAsync
    // =========================================================

    [Fact]
    public async Task GetByIdAsync_WhenUserExists_ShouldReturnUserViewModel()
    {
        // Arrange
        var role = RoleFaker.ForRole(RoleEnum.Admin);
        var user = UserFaker.WithRole(role);

        _unityOfWork.Users.GetByIdAsyncWithRole(user.Id).Returns(user);

        // Act
        var result = await _sut.GetByIdAsync(user.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(user.Id);
        result.Name.Should().Be(user.Name);
        result.Email.Should().Be(user.Email.Value);
        result.Role.Should().Be(role.Name);
        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task GetByIdAsync_WhenUserDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        _unityOfWork.Users.GetByIdAsyncWithRole(nonExistentId).ReturnsNull();

        // Act
        var act = () => _sut.GetByIdAsync(nonExistentId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    // =========================================================
    // GetAllAsync
    // =========================================================

    [Fact]
    public async Task GetAllAsync_WhenUsersExist_ShouldReturnAllViewModels()
    {
        // Arrange
        var role = RoleFaker.ForRole(RoleEnum.Employee);
        var users = Enumerable.Range(0, 5)
            .Select(_ => UserFaker.WithRole(role))
            .ToList();

        _unityOfWork.Users.GetAllAsyncWithRole().Returns(users);

        // Act
        var result = await _sut.GetAllAsync(CancellationToken.None);

        // Assert
        result.Should().HaveCount(5);
        result.Should().AllSatisfy(vm =>
        {
            vm.Should().NotBeNull();
            vm!.Role.Should().Be(role.Name);
        });
    }

    [Fact]
    public async Task GetAllAsync_WhenNoUsersExist_ShouldReturnEmptyCollection()
    {
        // Arrange
        _unityOfWork.Users.GetAllAsyncWithRole().Returns([]);

        // Act
        var result = await _sut.GetAllAsync(CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    // =========================================================
    // UpdateAsync
    // =========================================================

    [Fact]
    public async Task UpdateAsync_WhenUserExists_ShouldUpdateAndSave()
    {
        // Arrange
        var role = RoleFaker.ForRole(RoleEnum.Manager);
        var user = UserFaker.WithRole(role);
        var updateInput = new UpdateUserInputModel("Novo Nome", "novo@email.com", "NovaSenha@1", "Manager");
        const string newHash = "newhash456";

        _unityOfWork.Users.GetByIdAsyncWithRoleNoTracking(user.Id).Returns(user);
        _unityOfWork.Roles.GetByNameAsync(updateInput.Role).Returns(role);
        _authService.ComputeSha256Hash(updateInput.Password).Returns(newHash);

        // Act
        await _sut.UpdateAsync(user.Id, updateInput, CancellationToken.None);

        // Assert
        await _unityOfWork.Users.Received(1).UpdateAsync(Arg.Is<User>(u =>
            u.Name == updateInput.Name &&
            u.PasswordHash == newHash));
        await _unityOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateAsync_WhenUserDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _unityOfWork.Users.GetByIdAsyncWithRoleNoTracking(id).ReturnsNull();

        var input = new UpdateUserInputModel("Nome", "email@test.com", "Senha@1", "Admin");

        // Act
        var act = () => _sut.UpdateAsync(id, input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
        await _unityOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    // =========================================================
    // DeleteAsync (soft delete via user.Delete())
    // =========================================================

    [Fact]
    public async Task DeleteAsync_WhenUserExists_ShouldDeactivateAndSave()
    {
        // Arrange
        var role = RoleFaker.ForRole(RoleEnum.Employee);
        var user = UserFaker.WithRole(role);

        _unityOfWork.Users.GetByIdAsyncWithRoleNoTracking(user.Id).Returns(user);

        // Act
        await _sut.DeleteAsync(user.Id, CancellationToken.None);

        // Assert — o método Delete() da entidade seta IsActive = false
        user.IsActive.Should().BeFalse();

        await _unityOfWork.Users.Received(1).UpdateAsync(Arg.Is<User>(u => !u.IsActive));
        await _unityOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteAsync_WhenUserDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _unityOfWork.Users.GetByIdAsyncWithRoleNoTracking(id).ReturnsNull();

        // Act
        var act = () => _sut.DeleteAsync(id, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
        await _unityOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
