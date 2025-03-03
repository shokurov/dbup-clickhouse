using DbUp.Tests.Common;

namespace DbUp.NewProvider.Tests;

public class NoPublicApiChanges : NoPublicApiChangesBase
{
    public NoPublicApiChanges()
        : base(typeof(NewProviderExtensions).Assembly)
    {
    }
}
