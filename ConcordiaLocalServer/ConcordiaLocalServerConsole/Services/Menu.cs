namespace ConcordiaLocalServerConsole.Services;

using System.Linq;
using System.Net.Sockets;
using System.Text;

using Modules;
using Modules.Abstract;
using Services.Modules.Classes;
using Services.Modules.Exceptions;

public class Menu : IMenu
{
    private NetworkStream? _stream;

    private readonly IList<IModule> _modules;
    public IList<IModule> Modules { get => _modules; }

    public Menu(IList<IModule> modules)
    {
        _stream = null;
        _modules = modules;
        _modules.Add(new ExitModule());
    }

    public async Task HandleClientAsync(TcpClient client)
    {
        try
        {
            _stream = client.GetStream();

            var buffer = new byte[0];
            var bytesRead = 0;

            var initString = new StringBuilder();
            initString.Append($"################################");
            initString.Append(Environment.NewLine);
            initString.Append($"Local Server");
            initString.Append(Environment.NewLine);

            var initBuffer = Encoding.ASCII.GetBytes(initString.ToString());
            await _stream.WriteAsync(initBuffer, 0, initBuffer.Length);

            while (true)
            {
                var menuString = new StringBuilder();
                menuString.Append($"MENU:");
                menuString.Append(Environment.NewLine);
                foreach (var mod in Modules)
                {
                    menuString.Append($"[{mod.Command}]:\t{mod.Name}{Environment.NewLine}");
                }
                var menuBuffer = Encoding.ASCII.GetBytes(menuString.ToString());
                await _stream.WriteAsync(menuBuffer, 0, menuBuffer.Length);

                var inputBuffer = Encoding.ASCII.GetBytes("Waiting...");
                await _stream.WriteAsync(inputBuffer, 0, inputBuffer.Length);

                // problem: if the client is stupid and writes too much (more than 4096 bytes) 
                // he is stupid and should not use the server at all... 
                buffer = new byte[4096];
                bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                var input = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                var module = Modules.SingleOrDefault(x => x.Command.Equals(input.Trim(), StringComparison.OrdinalIgnoreCase));
                if (module is null)
                {
                    var invalidString = new StringBuilder();
                    invalidString.Append($"Input Not Valid: {input}.");
                    invalidString.Append(Environment.NewLine);
                    var invalidBuffer = Encoding.ASCII.GetBytes(invalidString.ToString());
                    await _stream.WriteAsync(invalidBuffer, 0, invalidBuffer.Length);
                }
                else
                {
                    var runString = new StringBuilder();
                    runString.Append("RUN: ");

                    try
                    {
                        runString.Append($"{module.Name}");
                        runString.Append(Environment.NewLine);
                        var runBuffer = Encoding.ASCII.GetBytes(runString.ToString());
                        await _stream.WriteAsync(runBuffer, 0, runBuffer.Length);
                        await module.StartAsync(_stream);
                    }
                    catch (ExitException e)
                    {
                        runString.Append(e.Message);
                        runString.Append(Environment.NewLine);
                        initString.Append($"################################");
                        var runBuffer = Encoding.ASCII.GetBytes(runString.ToString());
                        await _stream.WriteAsync(runBuffer, 0, runBuffer.Length);
                        break;
                    }
                }
            }
            client.Close();
            Console.WriteLine("Client disconnected.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}