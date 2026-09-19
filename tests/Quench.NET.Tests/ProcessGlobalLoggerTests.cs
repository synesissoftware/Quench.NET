using Quench.Diagnostics;

using Xunit;

namespace Quench.Tests;

public sealed class ProcessGlobalLoggerTests : QuenchTestBase
{
    [Fact]
    public void Original_logger_is_not_null()
    {
        ISimpleLogger newLogger = new NullSimpleLogger();
        ISimpleLogger oldLogger = Core.SetProcessGlobalLogger(newLogger);

        try
        {
            Assert.NotNull(oldLogger);
        }
        finally
        {
            Core.SetProcessGlobalLogger(oldLogger);
        }
    }

    [Fact]
    public void Original_logger_is_DebugSimpleLogger()
    {
        ISimpleLogger newLogger = new NullSimpleLogger();
        ISimpleLogger oldLogger = Core.SetProcessGlobalLogger(newLogger);

        try
        {
            Assert.IsType<DebugSimpleLogger>(oldLogger);
        }
        finally
        {
            Core.SetProcessGlobalLogger(oldLogger);
        }
    }

    [Fact]
    public void Setting_twice_yields_original()
    {
        ISimpleLogger newLogger = new NullSimpleLogger();
        ISimpleLogger oldLogger = Core.SetProcessGlobalLogger(newLogger);
        ISimpleLogger otherLogger = Core.SetProcessGlobalLogger(oldLogger);

        Assert.Same(newLogger, otherLogger);
    }

    [Fact]
    public void Setting_null_logger_throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => Core.SetProcessGlobalLogger(null!));
    }
}
