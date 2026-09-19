using Xunit;

namespace Quench.Tests;

[CollectionDefinition(nameof(QuenchProcessStateCollection), DisableParallelization = true)]
public sealed class QuenchProcessStateCollection
{
}

[Collection(nameof(QuenchProcessStateCollection))]
public abstract class QuenchTestBase : IDisposable
{
    protected QuenchTestBase()
    {
        Core.ResetConfiguration();
    }

    public void Dispose()
    {
        Core.ResetConfiguration();
        GC.SuppressFinalize(this);
    }
}
