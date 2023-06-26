namespace ConcordiaLocalServerConsole.Services.Modules.Abstract;

using System.Net.Sockets;

public interface IModule
{
    public string Name { get; }
    public string Command { get; }

    public void Start();

    public Task StartAsync(NetworkStream strem);
}