namespace GestorPro.Tests.Common;

public sealed class UnitOfMeasureFaker : Faker<UnitOfMeasure>
{
    private const int DefaultSeed = 55;

    private static readonly string[] ValidCodes =
    [
        "UN", "KG", "LT", "MT", "CM", "CX", "PC", "GL", "TB", "SC"
    ];

    public UnitOfMeasureFaker(int seed = DefaultSeed)
    {
        Locale = "pt_BR";
        UseSeed(seed);

        CustomInstantiator(f => new UnitOfMeasure(
            f.PickRandom(ValidCodes),
            f.Commerce.ProductMaterial(),
            f.Random.Bool(weight: 0.8f)));
    }

    /// <summary>Gera uma UnitOfMeasure ativa com dados fixos e previsíveis.</summary>
    public static UnitOfMeasure Active() =>
        new("UN", "Unidade", isActive: true);

    /// <summary>Gera uma UnitOfMeasure inativa.</summary>
    public static UnitOfMeasure Inactive() =>
        new("KG", "Quilograma", isActive: false);

    /// <summary>Gera uma UnitOfMeasure com código pré-definido.</summary>
    public static UnitOfMeasure WithCode(string code) =>
        new(code, "Nome Qualquer", isActive: true);

    /// <summary>
    /// Gera um CreateUnitOfMeasureInputModel válido para uso nos testes de serviço e validator.
    /// </summary>
    public static CreateUnitOfMeasureInputModel ValidCreateInput(
        string code = "UN",
        string name = "Unidade",
        bool isActive = true) =>
        new(code, name, isActive);

    /// <summary>
    /// Gera um UpdateUnitOfMeasureInputModel válido para uso nos testes de serviço e validator.
    /// </summary>
    public static UpdateUnitOfMeasureInputModel ValidUpdateInput(
        string code = "KG",
        string name = "Quilograma") =>
        new(code, name);
}
