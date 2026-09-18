using Quench.Exceptions;

using Xunit;

namespace Quench.Tests;

public sealed class ByConfigBadSystemExceptionTests : QuenchTestBase
{
    [Fact]
    public void ConfigureFromXml_rejects_System_Exception_ofClass()
    {
        InvalidQuenchConfigurationValueException ex =
            Assert.Throws<InvalidQuenchConfigurationValueException>(
                () => Core.ConfigureFromXml(
                    """
                    <quench>
                      <program defaultQuenchAction="throw" />
                      <actions>
                        <forException ofClass="System.Exception" quenchAction="quench"/>
                      </actions>
                    </quench>
                    """));

        Assert.Equal("ofClass", ex.AttributeName);
        Assert.Equal("System.Exception", ex.InvalidValue);
        Assert.Contains("System.Exception", ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Configure_rejects_typeof_Exception()
    {
        InvalidQuenchConfigurationValueException ex =
            Assert.Throws<InvalidQuenchConfigurationValueException>(
                () => Core.Configure(
                    cfg => cfg.ForException(typeof(Exception), QuenchAction.Quench)));

        Assert.Equal("System.Exception", ex.InvalidValue);
    }
}
