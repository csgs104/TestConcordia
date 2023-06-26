using System.Text;
using System.Net.Sockets;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ConcordiaLocalClientConsole;

// NOT FINISHED

TcpClient client = null;
try
{
    var builder = Host.CreateDefaultBuilder(args);

    // Configuring Services
    builder.ConfigureServices((context, services) =>
    {
        // IP and Port of Local Server
        var ip = ClientSettings.GetLocalServerIp();
        var port = int.Parse(ClientSettings.GetLocalServerPort());

        // Start Local Server
        services.AddSingleton(_ => new TcpClient(ip, port));
    });

    // Starting 
    var host = builder.Build() ?? throw new Exception("Host is not Ready.");

    Console.WriteLine("Local Client Started.");
    Console.WriteLine("Local Client is waiting a connection to LocalServer...");

    // Connection to Local Server
    client = host.Services.GetService<TcpClient>() ?? throw new Exception("Local Server not Found.");
    Console.WriteLine("Connected to Local Server.");

    // Connection Stream
    NetworkStream stream = client.GetStream();
    var buffer = new byte[0];
    var bytesRead = 0;
    var message = string.Empty;

    while (true)
    {
        buffer = new byte[4096];
        bytesRead = stream.Read(buffer, 0, buffer.Length);
        message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        Console.Write(message);

        if (message.EndsWith("Waiting..."))
        {
            Console.Write(" Input: ");
            var request = Console.ReadLine();
            byte[] inputBuffer = Encoding.ASCII.GetBytes(request);
            stream.Write(inputBuffer, 0, inputBuffer.Length);
            if (request == "Exit") break;
        }
    }
    client.Close();
    Console.WriteLine("Disconnected from Local Server.");
}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}