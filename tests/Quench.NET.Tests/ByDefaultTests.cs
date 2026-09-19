using Quench.Deems;

using Xunit;

namespace Quench.Tests;

public sealed class ByDefaultTests : QuenchTestBase
{
    [Fact]
    public void ApplicationException_must_be_rethrown()
    {
        Assert.True(CaughtException.MustBeRethrown(new ApplicationException()));
    }

    [Fact]
    public void OutOfMemoryException_must_be_rethrown()
    {
        Assert.True(CaughtException.MustBeRethrown(new OutOfMemoryException()));
    }

    [Fact]
    public void SystemException_must_be_rethrown()
    {
        Assert.True(CaughtException.MustBeRethrown(new SystemException()));
    }

    [Fact]
    public void MayBeQuenched_is_false_without_rules()
    {
        Assert.False(CaughtException.MayBeQuenched(new InvalidOperationException()));
    }

    [Fact]
    public void IsPreciselySpecified_is_false_without_rules()
    {
        Assert.False(Core.IsPreciselySpecified(typeof(ApplicationException), null));
    }
}
