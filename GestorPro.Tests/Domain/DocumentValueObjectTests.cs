namespace GestorPro.Tests.Domain;

public sealed class DocumentValueObjectTests
{
    // =========================================================
    // Document.Create — CPF válido
    // =========================================================

    [Theory]
    [InlineData("529.982.247-25")]   // formatado com máscara
    [InlineData("52998224725")]      // apenas dígitos
    [InlineData("111.444.777-35")]
    [InlineData("853.926.070-04")]
    public void Create_WhenCpfIsValid_ShouldReturnDocumentInstance(string rawCpf)
    {
        // Act
        var document = Document.Create(rawCpf);

        // Assert
        document.Should().NotBeNull();
        document.IsCPF().Should().BeTrue();
        document.IsPF().Should().BeTrue();
        document.Value.Should().MatchRegex(@"^\d{11}$", "o valor interno deve conter apenas os 11 dígitos");
    }

    // =========================================================
    // Document.Create — CNPJ válido
    // =========================================================

    [Theory]
    [InlineData("11.222.333/0001-81")]  // formatado com máscara
    [InlineData("11222333000181")]       // apenas dígitos
    [InlineData("45.997.418/0001-53")]
    public void Create_WhenCnpjIsValid_ShouldReturnDocumentInstance(string rawCnpj)
    {
        // Act
        var document = Document.Create(rawCnpj);

        // Assert
        document.Should().NotBeNull();
        document.IsCNPJ().Should().BeTrue();
        document.IsPJ().Should().BeTrue();
        document.Value.Should().MatchRegex(@"^\d{14}$", "o valor interno deve conter apenas os 14 dígitos");
    }

    // =========================================================
    // Document.Create — CPF inválido (dígito verificador errado)
    // =========================================================

    [Theory]
    [InlineData("52998224726")]   // último dígito errado
    [InlineData("11111111111")]   // todos iguais (sequência inválida)
    [InlineData("00000000000")]
    [InlineData("99999999999")]
    public void Create_WhenCpfIsInvalid_ShouldThrowArgumentException(string invalidCpf)
    {
        // Act
        var act = () => Document.Create(invalidCpf);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*CPF*");
    }

    // =========================================================
    // Document.Create — CNPJ inválido (dígito verificador errado)
    // =========================================================

    [Theory]
    [InlineData("11222333000182")]   // último dígito errado
    [InlineData("11111111111111")]   // todos iguais
    [InlineData("00000000000000")]
    public void Create_WhenCnpjIsInvalid_ShouldThrowArgumentException(string invalidCnpj)
    {
        // Act
        var act = () => Document.Create(invalidCnpj);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*CNPJ*");
    }

    // =========================================================
    // Document.Create — tamanho de documento inválido
    // =========================================================

    [Theory]
    [InlineData("1234567890")]    // 10 dígitos — nem CPF nem CNPJ
    [InlineData("123456789012345")]  // 15 dígitos
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("abc.def.ghi-jk")]  // só letras/símbolos sem dígitos suficientes
    public void Create_WhenDocumentHasInvalidLength_ShouldThrowArgumentException(string invalidDoc)
    {
        // Act
        var act = () => Document.Create(invalidDoc);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    // =========================================================
    // Document.Create — remove máscara automaticamente
    // =========================================================

    [Fact]
    public void Create_WhenCpfHasMask_ShouldStoreOnlyDigits()
    {
        // Act
        var document = Document.Create("529.982.247-25");

        // Assert
        document.Value.Should().Be("52998224725");
    }

    [Fact]
    public void Create_WhenCnpjHasMask_ShouldStoreOnlyDigits()
    {
        // Act
        var document = Document.Create("11.222.333/0001-81");

        // Assert
        document.Value.Should().Be("11222333000181");
    }

    // =========================================================
    // FormattedValue
    // =========================================================

    [Fact]
    public void FormattedValue_WhenCpf_ShouldReturnFormattedString()
    {
        // Arrange
        var document = Document.Create("52998224725");

        // Assert
        document.FormattedValue.Should().Be("529.982.247-25");
    }

    [Fact]
    public void FormattedValue_WhenCnpj_ShouldReturnFormattedString()
    {
        // Arrange
        var document = Document.Create("11222333000181");

        // Assert
        document.FormattedValue.Should().Be("11.222.333/0001-81");
    }

    // =========================================================
    // DocumentType e CustomerType derivado
    // =========================================================

    [Fact]
    public void Create_WhenCpf_ShouldSetCorrectTypes()
    {
        var document = Document.Create("52998224725");

        document.DocumentType.Should().Be(Document.DocumentTypeEnum.CPF);
        document.CustomerType.Should().Be(Document.CustomerTypeEnum.PF);
    }

    [Fact]
    public void Create_WhenCnpj_ShouldSetCorrectTypes()
    {
        var document = Document.Create("11222333000181");

        document.DocumentType.Should().Be(Document.DocumentTypeEnum.CNPJ);
        document.CustomerType.Should().Be(Document.CustomerTypeEnum.PJ);
    }

    // =========================================================
    // Igualdade (IEquatable<Document>)
    // =========================================================

    [Fact]
    public void Equals_WhenSameDocument_ShouldBeEqual()
    {
        // Arrange
        var doc1 = Document.Create("52998224725");
        var doc2 = Document.Create("529.982.247-25"); // com máscara, mesmo valor

        // Assert
        doc1.Should().Be(doc2);
        (doc1 == doc2).Should().BeTrue();
        doc1.GetHashCode().Should().Be(doc2.GetHashCode());
    }

    [Fact]
    public void Equals_WhenDifferentDocuments_ShouldNotBeEqual()
    {
        // Arrange
        var cpf = Document.Create("52998224725");
        var cnpj = Document.Create("11222333000181");

        // Assert
        cpf.Should().NotBe(cnpj);
        (cpf != cnpj).Should().BeTrue();
    }

    // =========================================================
    // ToString
    // =========================================================

    [Fact]
    public void ToString_ShouldReturnFormattedValue()
    {
        var document = Document.Create("52998224725");

        document.ToString().Should().Be("529.982.247-25");
    }
}
