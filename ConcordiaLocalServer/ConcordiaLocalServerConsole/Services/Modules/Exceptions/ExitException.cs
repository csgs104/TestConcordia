namespace ConcordiaLocalServerConsole.Services.Modules.Exceptions;

public class ExitException : Exception
{
    public ExitException(string message)
    : base(message)
    { }
}