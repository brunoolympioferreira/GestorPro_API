namespace GestorPro.Tests.Common;

/// <summary>
/// Gerador de dados falsos para a entidade User.
///
/// Respeita o construtor público:
///   User(string name, string email, string passwordHash, Guid roleId, bool isActive)
///
/// E o Value Object Email, que exige:
///   - Não vazio
///   - Deve conter '@'
///   - Máximo 254 caracteres
/// </summary>
public sealed class UserFaker : Faker<User>
{
    private const int DefaultSeed = 42;

    // IDs dos roles com seed fixo da migration (RoleConfiguration.cs)
    public static readonly Guid AdminRoleId = new("11111111-0000-0000-0000-000000000001");
    public static readonly Guid ManagerRoleId = new("11111111-0000-0000-0000-000000000002");
    public static readonly Guid EmployeeRoleId = new("11111111-0000-0000-0000-000000000003");
    public static readonly Guid ViewerRoleId = new("11111111-0000-0000-0000-000000000004");

    public UserFaker(int seed = DefaultSeed)
    {
        Locale = "pt_BR";
        UseSeed(seed);

        CustomInstantiator(f =>
        {
            var name = f.Person.FullName;
            // Garante e-mail válido conforme Email.Create(): contém '@', não vazio, ≤254 chars
            var email = f.Internet.Email(name).ToLowerInvariant();
            // Hash SHA256 simulado (64 chars hexadecimais)
            var passwordHash = f.Random.Hash(64);
            var roleId = f.PickRandom(AdminRoleId, ManagerRoleId, EmployeeRoleId, ViewerRoleId);
            var isActive = f.Random.Bool(weight: 0.8f);

            return new User(name, email, passwordHash, roleId, isActive);
        });
    }

    /// <summary>
    /// Gera um usuário inativo.
    /// Útil para testar regras de negócio de usuários desativados.
    /// </summary>
    public static User Inactive()
        => new UserFaker().CustomInstantiator(f =>
        {
            var name = f.Person.FullName;
            var email = f.Internet.Email(name).ToLowerInvariant();
            return new User(name, email, f.Random.Hash(64), EmployeeRoleId, isActive: false);
        }).Generate();

    /// <summary>
    /// Gera um usuário com e-mail pré-definido.
    /// Útil para testar unicidade de e-mail.
    /// </summary>
    public static User WithEmail(string email)
        => new UserFaker().CustomInstantiator(f =>
            new User(f.Person.FullName, email, f.Random.Hash(64), EmployeeRoleId, isActive: true)
        ).Generate();

    /// <summary>
    /// Gera um usuário com um Role pré-definido (entity).
    /// Útil para testar projeção de UserViewModel.
    /// </summary>
    public static User WithRole(Role role)
    {
        var faker = new Faker("pt_BR");
        var user = new User(
            faker.Person.FullName,
            faker.Internet.Email().ToLowerInvariant(),
            faker.Random.Hash(64),
            role.Id,
            isActive: true);
        user.Role = role;
        return user;
    }
}
