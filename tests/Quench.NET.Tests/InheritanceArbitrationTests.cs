using System.IO;

using Quench.Deems;

using Xunit;

namespace Quench.Tests;

public sealed class InheritanceArbitrationTests : QuenchTestBase
{
    private class GrandParent
    {
    }

    private class Parent : GrandParent
    {
    }

    private class Child : Parent
    {
    }

    public InheritanceArbitrationTests()
    {
        Core.Configure(cfg =>
        {
            cfg.DefaultAction = QuenchAction.Quench;
            cfg.ForException(typeof(IOException), QuenchAction.Throw)
                .ExceptWhen(typeof(Parent), QuenchAction.Quench);
            cfg.ForException(typeof(FileNotFoundException))
                .ExceptWhen(typeof(Child), QuenchAction.Quench);
        });
    }

    [Fact]
    public void FileNotFound_in_Child_uses_precise_exceptWhen()
    {
        Assert.False(
            CaughtException.MustBeRethrown(new FileNotFoundException(), typeof(Child)));
    }

    [Fact]
    public void FileNotFound_in_Parent_inherits_IOException_exceptWhen()
    {
        // 3.3: parent exception type (IOException) with specific catching
        // type (Parent). Hautacam 0.1.1 skipped this match.
        Assert.False(
            CaughtException.MustBeRethrown(new FileNotFoundException(), typeof(Parent)));
    }

    [Fact]
    public void FileNotFound_unqualified_inherits_IOException_throw()
    {
        Assert.True(CaughtException.MustBeRethrown(new FileNotFoundException()));
    }

    [Fact]
    public void IOException_in_Parent_is_quenched()
    {
        Assert.False(CaughtException.MustBeRethrown(new IOException(), typeof(Parent)));
    }

    [Fact]
    public void IOException_unqualified_must_be_rethrown()
    {
        Assert.True(CaughtException.MustBeRethrown(new IOException()));
    }

    [Fact]
    public void FileNotFound_in_GrandParent_falls_back_to_IOException_throw()
    {
        Assert.True(
            CaughtException.MustBeRethrown(
                new FileNotFoundException(),
                typeof(GrandParent)));
    }
}
