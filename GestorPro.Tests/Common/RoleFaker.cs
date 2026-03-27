namespace GestorPro.Tests.Common;

/// <summary>
/// Gerador de dados falsos para a entidade Role.
/// </summary>
public sealed class RoleFaker : Faker<Role>
{
    private const int DefaultSeed = 10;

    public RoleFaker(int seed = DefaultSeed)
    {
        Locale = "pt_BR";
        UseSeed(seed);

        // Usa CustomInstantiator para respeitar o construtor da entidade
        CustomInstantiator(f =>
        {
            var roleName = f.PickRandom(Enum.GetNames<RoleEnum>());
            var description = f.Lorem.Sentence();
            return new Role(roleName, description);
        });
    }

    /// <summary>
    /// Gera um Role com um nome específico do enum.
    /// Útil para testar regras de autorização.
    /// </summary>
    public static Role ForRole(RoleEnum role)
        => new(role.ToString(), $"Role: {role}");
}
