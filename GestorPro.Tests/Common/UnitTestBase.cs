namespace GestorPro.Tests.Common;

/// <summary>
/// Classe base para todos os testes unitários do GestorPro.
/// Fornece um Fixture pré-configurado com NSubstitute para criação automática de mocks.
///
/// Uso típico no constructor do teste:
///   _unityOfWork = Fixture.Freeze&lt;IUnityOfWork&gt;();
///   _sut = Fixture.Create&lt;UserService&gt;();
/// </summary>
public abstract class UnitTestBase
{
    protected IFixture Fixture { get; }

    protected UnitTestBase()
    {
        Fixture = new Fixture().Customize(new AutoNSubstituteCustomization
        {
            ConfigureMembers = false
        });
    }

    protected static T Sub<T>() where T : class
        => Substitute.For<T>();
}
