using Microsoft.EntityFrameworkCore;

using ConcordiaTrelloLibrary;

using ConcordiaDBLibrary;
using ConcordiaDBLibrary.Data;
using ConcordiaDBLibrary.Gateways.Abstract;
using ConcordiaDBLibrary.Gateways.Classes;
using ConcordiaDBLibrary.Models.Classes;

using ConcordiaServicesLibrary;

using ConcordiaMVC;

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
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ConcordiaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(DBSettings.GetConnectionString())));

builder.Services.AddScoped<ITrelloEntityGateway<Experiment>, ExperimentsGateway>();
builder.Services.AddScoped<ITrelloEntityGateway<Priority>, PrioritiesGateway>();
builder.Services.AddScoped<ITrelloEntityGateway<Remark>, RemarksGateway>();
builder.Services.AddScoped<ITrelloEntityGateway<Scientist>, ScientistsGateway>();
builder.Services.AddScoped<ITrelloEntityGateway<State>, StatesGateway>();
builder.Services.AddScoped<IEntityGateway<Participant>, ParticipantsGateway>();

builder.Services.AddSingleton(_ => new BackgroundSynchronizer());
builder.Services.AddHostedService(provider => provider.GetService<BackgroundSynchronizer>());

// build services
var app = builder.Build();

await app.MigrateAsync();

// configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // the default HSTS value is 30 days
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();