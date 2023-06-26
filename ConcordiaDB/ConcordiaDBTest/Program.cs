using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using ConcordiaDBInitializer;

using ConcordiaDBLibrary;
using ConcordiaDBLibrary.Data;
using ConcordiaDBLibrary.Gateways.Classes;

using ConcordiaDBTest;

InizializerSettings.SetConnectionString("Server=127.0.0.1,1433; Database=Master; User Id=SA; Password=R00t.r00T; TrustServerCertificate=true;");
DBSettings.SetConnectionString("Server=127.0.0.1,1433; Database=Concordia; User Id=SA; Password=R00t.r00T; TrustServerCertificate=true;");

var inizializer = false;
var library = true;

if (inizializer) 
{
    var cs = InizializerSettings.GetConnectionString();
    Console.WriteLine("INITIALIZER START.");
    Console.WriteLine("...");
    Initializer.Create(cs);
    Console.WriteLine("...");
    Initializer.Insert(cs);
    Console.WriteLine("...");
    Initializer.Delete(cs);
    Console.WriteLine("...");
    Console.WriteLine("INITIALIZER FINISH.");
}

if (library)
{
    var builder = Host.CreateDefaultBuilder(args);

    builder.ConfigureServices((context, services) =>
    {
        var cs = DBSettings.GetConnectionString();
        services.AddDbContext<ConcordiaContext>(options => options.UseSqlServer(cs));
        services.AddSingleton<ExperimentsGateway>();
        services.AddSingleton<ParticipantsGateway>();
        services.AddSingleton<PrioritiesGateway>();
        services.AddSingleton<RemarksGateway>();
        services.AddSingleton<StatesGateway>();
        services.AddSingleton<ScientistsGateway>();
    });

    var host = builder.Build();

    await Bootstrapper.MigrateAsync(host);
}