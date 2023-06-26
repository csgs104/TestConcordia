using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using ConcordiaLocalServerConsole;
using ConcordiaLocalServerConsole.Services;
using ConcordiaLocalServerConsole.Services.Modules;
using ConcordiaLocalServerConsole.Services.Modules.Abstract;
using ConcordiaLocalServerConsole.Services.Modules.Classes;

using ConcordiaDBLibrary;
using ConcordiaTrelloLibrary;
using ConcordiaServicesLibrary;
using ConcordiaServicesLibrary.Synchronizers;
using ConcordiaDBLibrary.Gateways.Abstract;
using ConcordiaDBLibrary.Gateways.Classes;
using ConcordiaDBLibrary.Models.Classes;
using ConcordiaDBLibrary.Data;

// NOT FINISHED

// FIRST: set the names for priorities and states (these names must be the same ones you used in your board)
SynchronizerSettings.SynchronizePrioritiesNames(new List<string>() { "HIGH", "MEDIUM", "LOW" });
SynchronizerSettings.SynchronizeStatesNames(new List<string>() { "Start", "Working", "Finish" });

// SECOND: set the connection string
DBSettings.SetConnectionString("Server=127.0.0.1,1433; Database=Concordia; User Id=SA; Password=R00t.r00T; TrustServerCertificate=true;");

// THIRD: set the board and the administrator
TrelloSettings.SetBoardCode("6495adfbc6ed836ca45adcf8");
TrelloSettings.SetBoardURL("https://trello.com/b/9M3kIsvR/testconcordia");
TrelloSettings.SetKeyAD("");
TrelloSettings.SetTokenAD("");

// FOURTH: set the base directory and reports directory
PathSettings.SetBaseDirectory("Concordia");
ReporterSettings.SetReportsDirectory("ConcordiaReports");

// FIFTH: set the email to send the report and the eamil to receive the report 
SenderSettings.SetFromEmail("...@outlook.com");
SenderSettings.SetFromPassword("...");
SenderSettings.SetToEmail("...@gmail.com");
SenderSettings.SetHost("smtp-mail.outlook.com");
SenderSettings.SetPort(587);

// SIXTH: set time for synchronization
SynchronizerSettings.SetSynchronizationTime(1);

// SEVENTH: the program can start...
TcpListener server = null;
bool isConcordiaTrelloAccessible = false;

try
{
    var builder = Host.CreateDefaultBuilder(args);

    // Configuring Services
    builder.ConfigureServices((context, services) =>
    {
        // IP and Port of Local Server
        var ip = IPAddress.Parse(ServerSettings.GetLocalServerIp());
        var port = int.Parse(ServerSettings.GetLocalServerPort());

        // Start Local Server
        services.AddSingleton(_ => new TcpListener(ip, port));

        services.AddScoped<ConcordiaContext>();
        services.AddScoped<ITrelloEntityGateway<Experiment>, ExperimentsGateway>();
        services.AddScoped<ITrelloEntityGateway<Priority>, PrioritiesGateway>();
        services.AddScoped<ITrelloEntityGateway<Remark>, RemarksGateway>();
        services.AddScoped<ITrelloEntityGateway<Scientist>, ScientistsGateway>();
        services.AddScoped<ITrelloEntityGateway<State>, StatesGateway>();
        services.AddScoped<IEntityGateway<Participant>, ParticipantsGateway>();
        services.AddSingleton(_ => new Synchronizer());

        // var ...Repo = new ...Repository(cn);
        // var ...Repo = new ...Repository(cn);

        var localClientMod = new LocalClientModule();
        var localClientTestMod = new LocalClientTestModule();
        // var ...Mod = new ...Module(...Repo);
        // var ...Mod = new ...Module(...Repo);

        var listMods = new List<IModule>();
        listMods.Add(localClientMod);
        listMods.Add(localClientTestMod);
        // listMods.Add(...)
        // listMods.Add(...)
        var menu = new Menu(listMods);

        services.AddTransient<IMenu>(_ => menu);
        // ... 
        // ...
    });

    // Starting 
    var host = builder.Build() ?? throw new Exception("Host is not Ready.");

    // Local Server Ready
    server = host.Services.GetService<TcpListener>() ?? throw new Exception("Local Server is not Ready.");
    server.Start();

    Console.WriteLine("Local Server Started.");
    Console.WriteLine("Local Server is waiting for connections...");

    // Semaphore Logic
    int maxClients = 10;
    var semaphore = new SemaphoreSlim(maxClients, maxClients);

    // Monitor internet connection in a separate thread
    _ = Task.Run(async () =>
    {
        while (true)
        {
            isConcordiaTrelloAccessible = await TrelloSettings.IsBoardAccessibleAsync();
            if (!isConcordiaTrelloAccessible)
            {
                // No Internet connection
                Console.WriteLine("No Internet connection. Server is, it will serve clients.");
            }
            else
            {
                // Internet connection
                Console.WriteLine("Internet connection. Server is , it will not serve clients.");
            }

            await Task.Delay(TimeSpan.FromMinutes(1)); // Check for internet connection every 10 minutes
        }
    });

    while (true)
    {
        var menu = host.Services.GetService<IMenu>() ?? throw new Exception("Menu is not Ready.");

        if (!isConcordiaTrelloAccessible) // isConcordiaTrelloAccessible = false
        {
            // Server is not under maintenance: waiting and accepting clients
            Console.WriteLine("Server is currently available.");
            var client = await server.AcceptTcpClientAsync();
            Console.WriteLine("Client connected.");

            await semaphore.WaitAsync(); // Waits until a slot is available

            _ = Task.Run(async () =>
            {
                await menu.HandleClientAsync(client);
                semaphore.Release(); // Release the slot when done
            });
        }
        else // isConcordiaTrelloAccessible = true
        {
            // Server is under maintenance: synchronization of DB and Trello
            Console.WriteLine("Server is currently unavailable.");

            // Synchronization LOGIC ()

            await Task.Delay(TimeSpan.FromMinutes(2)); // Check for changes in behavior every 12 minutes
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}
finally
{
    server?.Stop();
}