using Quench;
using Quench.Deems;

Console.WriteLine($"Quench.NET {LibraryVersion.VersionString}");

try
{
    throw new InvalidOperationException("demo");
}
catch (Exception x)
{
    if (CaughtException.MustBeRethrown(x))
    {
        Console.WriteLine("default: must rethrow (no rules configured)");
    }
}

Core.Configure(cfg =>
{
    cfg.DefaultAction = QuenchAction.Throw;
    cfg.ForException<InvalidOperationException>(QuenchAction.Quench);
});

try
{
    throw new InvalidOperationException("demo");
}
catch (Exception x)
{
    if (CaughtException.MayBeQuenched(x))
    {
        Console.WriteLine("configured: InvalidOperationException may be quenched");
    }
}
