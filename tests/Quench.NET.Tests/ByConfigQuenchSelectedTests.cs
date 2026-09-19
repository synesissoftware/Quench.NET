using Quench.Deems;

using Xunit;

namespace Quench.Tests;

public sealed class ByConfigQuenchSelectedTests : QuenchTestBase
{
    public ByConfigQuenchSelectedTests()
    {
        Core.Configure(cfg =>
        {
            cfg.DefaultAction = QuenchAction.Throw;
            cfg.ForException(typeof(OutOfMemoryException), QuenchAction.Quench);
        });
    }

    [Fact]
    public void ApplicationException_must_be_rethrown()
    {
        Assert.True(CaughtException.MustBeRethrown(new ApplicationException()));
    }

    [Fact]
    public void OutOfMemoryException_may_be_quenched()
    {
        Assert.False(CaughtException.MustBeRethrown(new OutOfMemoryException()));
    }

    [Fact]
    public void SystemException_must_be_rethrown()
    {
        Assert.True(CaughtException.MustBeRethrown(new SystemException()));
    }

    [Fact]
    public void OutOfMemoryException_is_precisely_specified()
    {
        Assert.True(Core.IsPreciselySpecified(typeof(OutOfMemoryException), null));
        Assert.False(Core.IsPreciselySpecified(typeof(ApplicationException), null));
    }
}
