namespace GestorPro.Tests.Services;

public sealed class LoginServiceTests : UnitTestBase
{
    private readonly IUnityOfWork _unityOfWork;
    private readonly IAuthService _authService;
    private readonly LoginService _sut;
    private readonly UserFaker _userFaker = new();

    public LoginServiceTests()
    {
        _unityOfWork = Fixture.Freeze<IUnityOfWork>();
        _authService = Fixture.Freeze<IAuthService>();
        _sut = new LoginService(_unityOfWork, _authService);
    }

    [Fact]
    public async Task LoginAsync_WhenCredentialsAreValid_ShouldReturnLoginViewModel()
    {
        // Arrange
        var user = _userFaker.Generate();
        var input = new LoginInputModel(user.Email.Value, "Senha@123");
        var fakeToken = new TokenDTO("jwt.token.aqui", DateTime.UtcNow.AddHours(12));

        _unityOfWork.Users.GetByEmailAsync(input.Email).Returns(user);
        _authService.VerifyPassword(input.Password, user.PasswordHash).Returns(true);
        _authService.GenerateJwtToken(user).Returns(fakeToken);

        // Act
        var result = await _sut.LoginAsync(input, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().Be(fakeToken.token);
        result.ExpiresAt.Should().Be(fakeToken.ExpiresAt);
    }

    [Fact]
    public async Task LoginAsync_WhenUserNotFound_ShouldThrowUnauthorizedAccessException()
    {
        // Arrange
        var input = new LoginInputModel("naoexiste@email.com", "Senha@123");
        _unityOfWork.Users.GetByEmailAsync(input.Email).ReturnsNull();

        // Act
        var act = () => _sut.LoginAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
        _authService.DidNotReceive().GenerateJwtToken(Arg.Any<User>());
    }

    [Fact]
    public async Task LoginAsync_WhenPasswordIsWrong_ShouldThrowUnauthorizedAccessException()
    {
        // Arrange
        var user = _userFaker.Generate();
        var input = new LoginInputModel(user.Email.Value, "SenhaErrada@1");

        _unityOfWork.Users.GetByEmailAsync(input.Email).Returns(user);
        _authService.VerifyPassword(input.Password, user.PasswordHash).Returns(false);

        // Act
        var act = () => _sut.LoginAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
        _authService.DidNotReceive().GenerateJwtToken(Arg.Any<User>());
    }

    [Theory]
    [InlineData("email@valido.com", "SenhaErrada@1")]   // senha errada
    [InlineData("outro@valido.com", "OutraErrada@99")]  // outro caso de senha errada
    public async Task LoginAsync_WithMultipleWrongPasswords_ShouldAlwaysThrowUnauthorized(
    string email, string password)
    {
        // Arrange
        var user = _userFaker.Generate();
        var input = new LoginInputModel(email, password);

        _unityOfWork.Users.GetByEmailAsync(email).Returns(user);
        _authService.VerifyPassword(Arg.Any<string>(), Arg.Any<string>()).Returns(false);

        // Act
        var act = () => _sut.LoginAsync(input, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }
}
