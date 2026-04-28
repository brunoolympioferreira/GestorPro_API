namespace GestorPro.Tests.Common;

public class CustomerFaker : Faker<Customer>
{
    private const int DefaultSeed = 77;

    // CPFs válidos fixos para uso nos testes
    public static readonly string[] ValidCpfs =
    [
        "85617460099",  // 529.982.247-25
        "17138998090",  // 111.444.777-35
        "47086923079"   // 034.982.900-87
    ];

    // CNPJs válidos fixos para uso nos testes
    public static readonly string[] ValidCnpjs =
    [
        "11222333000181",  // 11.222.333/0001-81
        "45997418000153"   // 45.997.418/0001-53
    ];

    public CustomerFaker(int seed = DefaultSeed)
    {
        Locale = "pt_BR";
        UseSeed(seed);

        CustomInstantiator(f =>
        {
            var document = f.PickRandom(ValidCpfs);
            var status = f.PickRandom<CustomerStatusEnum>();
            return new Customer(
                f.Company.CompanyName(),
                f.Company.CompanyName(),
                document,
                status);
        });
    }

    /// <summary>
    /// Gera um Customer com status Active e CPF.
    /// </summary>
    public static Customer Active()
    {
        var f = new Faker("pt_BR");
        return new Customer(
            f.Company.CompanyName(),
            f.Company.CompanyName(),
            ValidCpfs[0],
            CustomerStatusEnum.Active);
    }

    /// <summary>
    /// Gera um Customer com CNPJ (Pessoa Jurídica).
    /// </summary>
    public static Customer WithCnpj()
    {
        var f = new Faker("pt_BR");
        return new Customer(
            f.Company.CompanyName(),
            f.Company.CompanyName(),
            ValidCnpjs[0],
            CustomerStatusEnum.Active);
    }

    /// <summary>
    /// Gera um Customer com endereços e contatos já vinculados.
    /// Útil para testar projeções detalhadas (GetByIdAsync).
    /// </summary>
    public static Customer WithAddressesAndContacts()
    {
        var customer = Active();

        var address = AddressFaker.ForCustomer(customer.Id);
        var contact = ContactFaker.PrimaryForCustomer(customer.Id);

        customer.AddAddresses([address]);
        customer.AddContacts([contact]);

        return customer;
    }

    /// <summary>
    /// Gera um Customer com documento pré-definido.
    /// Útil para testar unicidade de documento.
    /// </summary>
    public static Customer WithDocument(string document)
    {
        var f = new Faker("pt_BR");
        return new Customer(
            f.Company.CompanyName(),
            f.Company.CompanyName(),
            document,
            CustomerStatusEnum.Active);
    }
}

/// <summary>
/// Gerador de dados falsos para a entidade Address.
/// </summary>
public static class AddressFaker
{
    private static readonly string[] ValidUfs =
        ["SP", "RJ", "MG", "RS", "PR", "SC", "BA", "GO", "DF"];

    public static Address ForCustomer(Guid customerId)
    {
        var f = new Faker("pt_BR");
        return new Address(
            customerId,
            f.Address.StreetName(),
            f.Random.Number(1, 9999).ToString(),
            f.Random.Bool() ? $"Ap {f.Random.Number(1, 200)}" : string.Empty,
            f.Address.County(),
            f.Address.City(),
            f.PickRandom(ValidUfs),
            f.Random.Replace("#####-###"),
            f.PickRandom<AddressTypeEnum>());
    }

    /// <summary>Gera um AddressDTO válido para uso em InputModels.</summary>
    public static AddressDTO ValidDto(Guid? id = null) => new(
        id,
        "Rua das Flores",
        "123",
        "Apto 4",
        "Centro",
        "São Paulo",
        "SP",
        "01310-100",
        nameof(AddressTypeEnum.Billing));
}

/// <summary>
/// Gerador de dados falsos para a entidade Contact.
/// </summary>
public static class ContactFaker
{
    public static Contact PrimaryForCustomer(Guid customerId) =>
        new(customerId, "contato@empresa.com.br", "11999999999", isPrimary: true);

    public static Contact SecondaryForCustomer(Guid customerId) =>
        new(customerId, "secundario@empresa.com.br", "11988888888", isPrimary: false);

    /// <summary>Gera um ContactDTO primário válido para uso em InputModels.</summary>
    public static ContactDTO PrimaryDto(Guid? id = null) =>
        new(id, "contato@empresa.com.br", "11999999999", IsPrimary: true);

    /// <summary>Gera um ContactDTO secundário válido para uso em InputModels.</summary>
    public static ContactDTO SecondaryDto(Guid? id = null) =>
        new(id, "secundario@empresa.com.br", "11988888888", IsPrimary: false);
}
