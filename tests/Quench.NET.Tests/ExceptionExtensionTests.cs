using Quench.Extensions;

using Xunit;

namespace Quench.Tests;

public sealed class ExceptionExtensionTests : QuenchTestBase
{
    [Fact]
    public void MustBeRethrown_extension_matches_Core_by_default()
    {
        InvalidOperationException x = new("demo");

        Assert.True(x.MustBeRethrown());
        Assert.False(x.MayBeQuenched());
    }

    [Fact]
    public void MustBeRethrown_extension_honours_configured_rule()
    {
        Core.Configure(cfg =>
        {
            cfg.DefaultAction = QuenchAction.Throw;
            cfg.ForException<InvalidOperationException>(QuenchAction.Quench);
        });

        InvalidOperationException x = new("demo");

        Assert.False(x.MustBeRethrown());
        Assert.True(x.MayBeQuenched(typeof(ExceptionExtensionTests)));
    }
}
