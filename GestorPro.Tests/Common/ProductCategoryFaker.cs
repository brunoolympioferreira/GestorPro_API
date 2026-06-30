namespace GestorPro.Tests.Common;

public sealed class ProductCategoryFaker : Faker<ProductCategory>
{
    private const int DefaultSeed = 42;

    public ProductCategoryFaker(int seed = DefaultSeed)
    {
        Locale = "pt_BR";
        UseSeed(seed);

        CustomInstantiator(f => new ProductCategory(
            f.Commerce.Department(),
            f.Lorem.Sentence(),
            f.Random.Bool(weight: 0.8f)));
    }

    /// <summary>Gera uma ProductCategory ativa com dados fixos e previsíveis.</summary>
    public static ProductCategory Active() =>
        new("Eletrônicos", "Categoria de produtos eletrônicos", isActive: true);

    /// <summary>Gera uma ProductCategory inativa.</summary>
    public static ProductCategory Inactive() =>
        new("Vestuário", "Categoria de roupas e acessórios", isActive: false);

    /// <summary>Gera uma ProductCategory ativa sem descrição.</summary>
    public static ProductCategory WithoutDescription() =>
        new("Ferramentas", null, isActive: true);

    /// <summary>Gera uma ProductCategory ativa com nome pré-definido.</summary>
    public static ProductCategory WithName(string name) =>
        new(name, "Descrição padrão", isActive: true);

    /// <summary>
    /// Gera um CreateProductCategoryInputModel válido para uso nos testes de serviço e validator.
    /// </summary>
    public static CreateProductCategoryInputModel ValidCreateInput(
        string name = "Eletrônicos",
        string? description = "Categoria de produtos eletrônicos",
        bool isActive = true) =>
        new(name, description, isActive);

    /// <summary>
    /// Gera um UpdateProductCategoryInputModel válido para uso nos testes de serviço e validator.
    /// </summary>
    public static UpdateProductCategoryInputModel ValidUpdateInput(
        string name = "Informática",
        string? description = "Computadores e periféricos",
        bool isActive = true) =>
        new(name, description, isActive);
}
