using System.Net.Sockets;

namespace ConcordiaLocalServerConsole.Services;

public interface IMenu
{
    public Task HandleClientAsync(TcpClient client);
}