using System.Text.RegularExpressions;

namespace GestorPro.Domain.ValueObjects;

/// <summary>
/// Value Object Document (CPF/CNPJ)
/// </summary>
public sealed class Document : IEquatable<Document>
{
    private readonly string _value;
    private readonly DocumentTypeEnum _type;

    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="value">Documento</param>
    /// <param name="type">Tipo</param>
    private Document(string value, DocumentTypeEnum type)
    {
        _value = value;
        _type = type;
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
        // Verifica se todos os dígitos são iguais
        if (Regex.IsMatch(cpf, @"^(\d)\1{10}$"))
            return false;

        // Valida primeiro dígito verificador
        var sum = 0;
        for (var i = 0; i < 9; i++)
        {
            sum += int.Parse(cpf[i].ToString()) * (10 - i);
        }
        var digit = 11 - (sum % 11);
        if (digit >= 10) digit = 0;
        if (digit != int.Parse(cpf[9].ToString()))
            return false;

        // Valida segundo dígito verificador
        sum = 0;
        for (var i = 0; i < 10; i++)
        {
            sum += int.Parse(cpf[i].ToString()) * (11 - i);
        }
        digit = 11 - (sum % 11);
        if (digit >= 10) digit = 0;
        if (digit != int.Parse(cpf[10].ToString()))
            return false;

        return true;
    }

    private static bool IsValidCNPJ(string cnpj)
    {
        // Verifica se todos os dígitos são iguais
        if (Regex.IsMatch(cnpj, @"^(\d)\1{13}$"))
            return false;

        // Valida primeiro dígito verificador
        int[] weights1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var sum = 0;
        for (var i = 0; i < 12; i++)
        {
            sum += int.Parse(cnpj[i].ToString()) * weights1[i];
        }
        var digit = sum % 11 < 2 ? 0 : 11 - (sum % 11);
        if (digit != int.Parse(cnpj[12].ToString()))
            return false;

        // Valida segundo dígito verificador
        int[] weights2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        sum = 0;
        for (var i = 0; i < 13; i++)
        {
            sum += int.Parse(cnpj[i].ToString()) * weights2[i];
        }
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
    public DocumentTypeEnum Type => _type;
    /// <summary>
    /// Documento formatado
    /// </summary>
    public string FormattedValue => _type switch
    {
        DocumentTypeEnum.CPF => Regex.Replace(_value, @"(\d{3})(\d{3})(\d{3})(\d{2})", "$1.$2.$3-$4"),
        DocumentTypeEnum.CNPJ => Regex.Replace(_value, @"(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})", "$1.$2.$3/$4-$5"),
        _ => _value
    };
    /// <summary>
    /// Verifica se é CPF
    /// </summary>
    /// <returns></returns>
    public bool IsCPF() => _type == DocumentTypeEnum.CPF;
    /// <summary>
    /// Verifica se é CNPJ
    /// </summary>
    /// <returns></returns>
    public bool IsCNPJ() => _type == DocumentTypeEnum.CNPJ;
    /// <summary>
    /// Compara igualdade entre DocumentTypes
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Document? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return _value == other._value && _type == other._type;
    }
    /// <summary>
    /// Determina igualdade entre objetos
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) => Equals(obj as Document);
    /// <summary>
    /// HashCode do DocumentType
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => HashCode.Combine(_value, _type);
    /// <summary>
    /// Retorna o documento formatado
    /// </summary>
    /// <returns></returns>
    public override string ToString() => FormattedValue;

    public static bool operator ==(Document? left, Document? right) =>
        Equals(left, right);

    public static bool operator !=(Document? left, Document? right) =>
        !Equals(left, right);

    public enum DocumentTypeEnum
    {
        CPF,
        CNPJ
    }
}
