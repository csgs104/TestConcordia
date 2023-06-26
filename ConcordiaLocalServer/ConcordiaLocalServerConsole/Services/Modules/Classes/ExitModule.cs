namespace ConcordiaLocalServerConsole.Services.Modules.Classes;

using System.Net.Sockets;
using Abstract;
using Exceptions;

public class ExitModule : IModule
{
    public string Name => "ExitMenu";
    public string Command => "Exit";

    public void Start()
    {
        throw new ExitException("Exit");
    }

    public Task StartAsync(NetworkStream stream)
    {
        throw new ExitException("Exit");
    }
}
