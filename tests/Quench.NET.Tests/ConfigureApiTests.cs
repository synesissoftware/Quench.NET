using Quench.Exceptions;

using Xunit;

namespace Quench.Tests;

public sealed class ConfigureApiTests : QuenchTestBase
{
    [Fact]
    public void Configure_null_callback_throws()
    {
        Assert.Throws<ArgumentNullException>(() => Core.Configure(null!));
    }

    [Fact]
    public void ConfigureFromXml_null_throws()
    {
        Assert.Throws<ArgumentNullException>(() => Core.ConfigureFromXml(null!));
    }

    [Fact]
    public void ConfigureFromXml_missing_program_throws()
    {
        Assert.Throws<InvalidQuenchConfigurationException>(
            () => Core.ConfigureFromXml("<quench></quench>"));
    }

    [Fact]
    public void ConfigureFromXml_invalid_action_throws()
    {
        InvalidQuenchConfigurationValueException ex =
            Assert.Throws<InvalidQuenchConfigurationValueException>(
                () => Core.ConfigureFromXml(
                    """
                    <quench>
                      <program defaultQuenchAction="swallow"/>
                    </quench>
                    """));

        Assert.Equal("swallow", ex.InvalidValue);
    }

    [Fact]
    public void ResetConfiguration_restores_default_rethrow()
    {
        Core.Configure(cfg => cfg.DefaultAction = QuenchAction.Quench);
        Assert.False(Core.MustBeRethrown(new InvalidOperationException()));

        Core.ResetConfiguration();
        Assert.True(Core.MustBeRethrown(new InvalidOperationException()));
    }

    [Fact]
    public void MustBeRethrown_null_exception_throws()
    {
        Assert.Throws<ArgumentNullException>(() => Core.MustBeRethrown(null!));
    }
}
