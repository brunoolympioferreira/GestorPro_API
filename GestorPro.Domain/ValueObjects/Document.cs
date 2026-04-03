using System.Text.RegularExpressions;

namespace GestorPro.Domain.ValueObjects;

/// <summary>
/// Value Object Document (CPF/CNPJ)
/// </summary>
public sealed class Document : IEquatable<Document>
{
    private readonly string _value;

    private Document(string value, DocumentTypeEnum documentType)
    {
        _value = value;
        DocumentType = documentType;
        CustomerType = documentType switch
        {
            DocumentTypeEnum.CPF => CustomerTypeEnum.PF,
            DocumentTypeEnum.CNPJ => CustomerTypeEnum.PJ,
            _ => throw new InvalidOperationException("Tipo de documento não mapeado para CustomerType")
        };
    }

    /// <summary>
    /// Cria uma nova instância de Document
    /// </summary>
    /// <param name="document">Documento</param>
    /// <returns>Documento Validado</returns>
    public static Document Create(string document)
    {
        var cleanDocument = Regex.Replace(document, @"\D", "");

        return cleanDocument.Length switch
        {
            11 => CreateCPF(cleanDocument),
            14 => CreateCNPJ(cleanDocument),
            _ => throw new ArgumentException("Documento deve conter 11 dígitos (CPF) ou 14 dígitos (CNPJ)")
        };
    }

    private static Document CreateCPF(string cpf)
    {
        if (!IsValidCPF(cpf))
            throw new ArgumentException("CPF inválido");

        return new Document(cpf, DocumentTypeEnum.CPF);
    }

    private static Document CreateCNPJ(string cnpj)
    {
        if (!IsValidCNPJ(cnpj))
            throw new ArgumentException("CNPJ inválido");

        return new Document(cnpj, DocumentTypeEnum.CNPJ);
    }

    private static bool IsValidCPF(string cpf)
    {
        if (Regex.IsMatch(cpf, @"^(\d)\1{10}$"))
            return false;

        var sum = 0;
        for (var i = 0; i < 9; i++)
            sum += int.Parse(cpf[i].ToString()) * (10 - i);

        var digit = 11 - (sum % 11);
        if (digit >= 10) digit = 0;
        if (digit != int.Parse(cpf[9].ToString()))
            return false;

        sum = 0;
        for (var i = 0; i < 10; i++)
            sum += int.Parse(cpf[i].ToString()) * (11 - i);

        digit = 11 - (sum % 11);
        if (digit >= 10) digit = 0;
        if (digit != int.Parse(cpf[10].ToString()))
            return false;

        return true;
    }

    private static bool IsValidCNPJ(string cnpj)
    {
        if (Regex.IsMatch(cnpj, @"^(\d)\1{13}$"))
            return false;

        int[] weights1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var sum = 0;
        for (var i = 0; i < 12; i++)
            sum += int.Parse(cnpj[i].ToString()) * weights1[i];

        var digit = sum % 11 < 2 ? 0 : 11 - (sum % 11);
        if (digit != int.Parse(cnpj[12].ToString()))
            return false;

        int[] weights2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        sum = 0;
        for (var i = 0; i < 13; i++)
            sum += int.Parse(cnpj[i].ToString()) * weights2[i];

        digit = sum % 11 < 2 ? 0 : 11 - (sum % 11);
        if (digit != int.Parse(cnpj[13].ToString()))
            return false;

        return true;
    }

    /// <summary>
    /// Documento sem formatação
    /// </summary>
    public string Value => _value;

    /// <summary>
    /// Tipo de Documento
    /// </summary>
    public DocumentTypeEnum DocumentType { get; private set; }

    /// <summary>
    /// Tipo de Cliente derivado do tipo de documento
    /// </summary>
    public CustomerTypeEnum CustomerType { get; private set; }

    /// <summary>
    /// Documento formatado
    /// </summary>
    public string FormattedValue => DocumentType switch
    {
        DocumentTypeEnum.CPF => Regex.Replace(_value, @"(\d{3})(\d{3})(\d{3})(\d{2})", "$1.$2.$3-$4"),
        DocumentTypeEnum.CNPJ => Regex.Replace(_value, @"(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})", "$1.$2.$3/$4-$5"),
        _ => _value
    };

    /// <summary>
    /// Verifica se é CPF
    /// </summary>
    public bool IsCPF() => DocumentType == DocumentTypeEnum.CPF;

    /// <summary>
    /// Verifica se é CNPJ
    /// </summary>
    public bool IsCNPJ() => DocumentType == DocumentTypeEnum.CNPJ;

    /// <summary>
    /// Verifica se é Pessoa Física
    /// </summary>
    public bool IsPF() => CustomerType == CustomerTypeEnum.PF;

    /// <summary>
    /// Verifica se é Pessoa Jurídica
    /// </summary>
    public bool IsPJ() => CustomerType == CustomerTypeEnum.PJ;

    /// <summary>
    /// Compara igualdade entre Documents
    /// </summary>
    public bool Equals(Document? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return _value == other._value && DocumentType == other.DocumentType;
    }

    /// <summary>
    /// Determina igualdade entre objetos
    /// </summary>
    public override bool Equals(object? obj) => Equals(obj as Document);

    /// <summary>
    /// HashCode do Document
    /// </summary>
    public override int GetHashCode() => HashCode.Combine(_value, DocumentType);

    /// <summary>
    /// Retorna o documento formatado
    /// </summary>
    public override string ToString() => FormattedValue;

    public static bool operator ==(Document? left, Document? right) => Equals(left, right);
    public static bool operator !=(Document? left, Document? right) => !Equals(left, right);

    public enum DocumentTypeEnum
    {
        CPF,
        CNPJ
    }

    public enum CustomerTypeEnum
    {
        PF,
        PJ
    }
}