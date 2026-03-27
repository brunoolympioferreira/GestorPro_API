namespace GestorPro.Tests.Domain;

public sealed class EmailValueObjectTests
{
    // =========================================================
    // Email.Create — casos válidos
    // =========================================================
    [Theory]
    [InlineData("usuario@email.com")]
    [InlineData("  usuario@email.com  ")]       // deve fazer trim
    [InlineData("USUARIO@EMAIL.COM")]           // deve normalizar para lowercase
    [InlineData("user.name+tag@empresa.com.br")]
    public void Create_WhenEmailIsValid_ShouldReturnEmailInstance(string rawEmail)
    {
        // Act
        var email = Email.Create(rawEmail);

        // Assert
        email.Should().NotBeNull();
        email.Value.Should().Be(rawEmail.Trim().ToLowerInvariant());
    }

    // =========================================================
    // Email.Create — casos inválidos
    // =========================================================

    [Theory]
    [InlineData("")]                    // vazio
    [InlineData("   ")]                // só espaços
    [InlineData("semArroba.com")]      // sem @
    [InlineData(null!)]                // null
    public void Create_WhenEmailIsInvalid_ShouldThrowArgumentException(string? invalidEmail)
    {
        // Act
        var act = () => Email.Create(invalidEmail!);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WhenEmailExceeds254Chars_ShouldThrowArgumentException()
    {
        // Arrange — 255 chars mas com @ para passar na checagem de formato
        var longLocal = new string('a', 250);
        var tooLong = $"{longLocal}@x.com"; // 257 chars

        // Act
        var act = () => Email.Create(tooLong);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    // =========================================================
    // Igualdade (IEquatable<Email>)
    // =========================================================

    [Fact]
    public void Equals_WhenSameValue_ShouldBeEqual()
    {
        // Arrange
        var email1 = Email.Create("test@email.com");
        var email2 = Email.Create("test@email.com");

        // Assert
        email1.Should().Be(email2);
        (email1 == email2).Should().BeTrue();
        email1.GetHashCode().Should().Be(email2.GetHashCode());
    }

    [Fact]
    public void Equals_WhenDifferentValues_ShouldNotBeEqual()
    {
        // Arrange
        var email1 = Email.Create("a@email.com");
        var email2 = Email.Create("b@email.com");

        // Assert
        email1.Should().NotBe(email2);
        (email1 != email2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WhenComparingSameEmailWithDifferentCase_ShouldBeEqual()
    {
        // Arrange — Create normaliza para lowercase
        var email1 = Email.Create("Usuario@Email.com");
        var email2 = Email.Create("usuario@email.com");

        // Assert
        email1.Should().Be(email2);
    }

    // =========================================================
    // ToString
    // =========================================================

    [Fact]
    public void ToString_ShouldReturnNormalizedValue()
    {
        // Arrange
        var email = Email.Create("TEST@EMAIL.COM");

        // Assert
        email.ToString().Should().Be("test@email.com");
    }
}
