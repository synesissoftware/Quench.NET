using Quench.Deems;

using Xunit;

namespace Quench.Tests;

public sealed class ByConfigQuenchAllTests : QuenchTestBase
{
    public ByConfigQuenchAllTests()
    {
        Core.ConfigureFromXml(
            """
            <quench>
              <program defaultQuenchAction="quench"/>
            </quench>
            """);
    }

    [Fact]
    public void ApplicationException_may_be_quenched()
    {
        Assert.False(CaughtException.MustBeRethrown(new ApplicationException()));
    }

    [Fact]
    public void OutOfMemoryException_may_be_quenched()
    {
        Assert.False(CaughtException.MustBeRethrown(new OutOfMemoryException()));
    }

    [Fact]
    public void SystemException_may_be_quenched()
    {
        Assert.False(CaughtException.MustBeRethrown(new SystemException()));
    }
}
