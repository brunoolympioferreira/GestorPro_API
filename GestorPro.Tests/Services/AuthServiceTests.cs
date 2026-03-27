namespace GestorPro.Tests.Services;

public sealed class AuthServiceTests
{
    // System Under Test — instanciado diretamente pois AuthService não tem dependências
    private readonly AuthService _sut = new();

    // =========================================================
    // ComputeSha256Hash
    // =========================================================

    [Fact]
    public void ComputeSha256Hash_WhenSameInput_ShouldReturnSameHash()
    {
        // Arrange
        const string password = "MinhaSenha@123";

        // Act
        var hash1 = _sut.ComputeSha256Hash(password);
        var hash2 = _sut.ComputeSha256Hash(password);

        // Assert — SHA256 é determinístico
        hash1.Should().Be(hash2);
    }

    [Fact]
    public void ComputeSha256Hash_WhenDifferentInputs_ShouldReturnDifferentHashes()
    {
        // Act
        var hash1 = _sut.ComputeSha256Hash("Senha@123");
        var hash2 = _sut.ComputeSha256Hash("Senha@456");

        // Assert
        hash1.Should().NotBe(hash2);
    }

    [Fact]
    public void ComputeSha256Hash_ShouldReturnLowercase64CharHex()
    {
        // Act
        var hash = _sut.ComputeSha256Hash("qualquersenha");

        // Assert — SHA256 em hex tem sempre 64 caracteres
        hash.Should().HaveLength(64);
        hash.Should().MatchRegex("^[0-9a-f]+$", "o hash deve estar em hexadecimal minúsculo");
    }

    // =========================================================
    // VerifyPassword
    // =========================================================

    [Fact]
    public void VerifyPassword_WhenPasswordMatchesHash_ShouldReturnTrue()
    {
        // Arrange
        const string password = "Senha@Correta1";
        var hash = _sut.ComputeSha256Hash(password);

        // Act
        var result = _sut.VerifyPassword(password, hash);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void VerifyPassword_WhenPasswordDoesNotMatchHash_ShouldReturnFalse()
    {
        // Arrange
        var hash = _sut.ComputeSha256Hash("Senha@Correta1");

        // Act
        var result = _sut.VerifyPassword("SenhaErrada@2", hash);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("Senha@1")]
    [InlineData("OutraSenha@999")]
    [InlineData("ABC!123xyz")]
    public void VerifyPassword_ShouldBeConsistentWithComputeHash(string password)
    {
        // Arrange
        var hash = _sut.ComputeSha256Hash(password);

        // Act & Assert — deve sempre verificar corretamente para qualquer senha
        _sut.VerifyPassword(password, hash).Should().BeTrue();
        _sut.VerifyPassword(password + "x", hash).Should().BeFalse();
    }
}
