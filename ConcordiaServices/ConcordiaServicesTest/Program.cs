using ConcordiaDBLibrary.Data;
using ConcordiaDBLibrary.Models.Classes;
using ConcordiaDBLibrary.Gateways.Classes;

using ConcordiaTrelloLibrary;
using ConcordiaTrelloLibrary.Models.Classes;
using ConcordiaTrelloLibrary.Gateways.Classes;

using ConcordiaServicesLibrary;
using ConcordiaServicesLibrary.Reporters;
using ConcordiaServicesLibrary.Senders;
using ConcordiaServicesLibrary.Synchronizers;

SynchronizerSettings.SynchronizeStatesNames(new List<string>() { "Start", "Working", "Finish" });
SynchronizerSettings.SynchronizePrioritiesNames(new List<string>() { "HIGH", "MEDIUM", "LOW" });

TrelloSettings.SetBoardCode("6495adfbc6ed836ca45adcf8");
TrelloSettings.SetBoardURL("https://trello.com/b/9M3kIsvR/testconcordia");
TrelloSettings.SetKeyAD("b7d0c40e7ccff5aefd87584240998c77");
TrelloSettings.SetTokenAD("ATTA9b0f53db2127dff365b1879dbbf7becd6f706c0908c5392a3be158c4964c3d3142475A3C");

var network = TrelloSettings.GetBoardAD();
var tDatabaseGway = new TrelloDatabaseGateway(network);
await tDatabaseGway.InitTrelloObjectAsync(TrelloSettings.GetBoardCode());

var test1 = false;
var test2 = false;
var test3 = false;
var test4 = false;
var test5 = false;
var test6 = false;
var test7 = true;
var test8 = false;
var test9 = false;

if (test1) // TEST 1
{
    // Priorities
    var tPrioritiesAll = await tDatabaseGway.GetTrelloPrioritiesAsync();
    Console.WriteLine("PrioritiesAll: ");
    foreach (var tPriority in tPrioritiesAll)
    {

        var priority = new Priority(null, tPriority.Code, tPriority.Name, tPriority.Color);
        Console.WriteLine($"{tPriority};");
        Console.WriteLine($"{priority};");
    }
    Console.WriteLine("#### #### #### #### #### #### #### #### #### #### #### ####");

    var tPriorities = tPrioritiesAll.Where(t => t.Name != string.Empty);
    Console.WriteLine("Priorities: ");
    foreach (var tPriority in tPriorities)
    {
        var priority = new Priority(null, tPriority.Code, tPriority.Name, tPriority.Color);
        Console.WriteLine($"{tPriority};");
        Console.WriteLine($"{priority};");
    }
    Console.WriteLine("#### #### #### #### #### #### #### #### #### #### #### ####");

    // Scientists
    var tScientists = await tDatabaseGway.GetTrelloScientistsAsync();
    foreach (var tScientist in tScientists)
    {
        var scientist = new Scientist(null, tScientist.Code, tScientist.FullName);
        Console.WriteLine($"{tScientist};");
        Console.WriteLine($"{scientist};");
    }
    Console.WriteLine("#### #### #### #### #### #### #### #### #### #### #### ####");

    var tScientistsSmart = tDatabaseGway.GetTrelloScientistsSmart();
    foreach (var tScientist in tScientistsSmart)
    {
        var scientist = new Scientist(null, tScientist.Code, tScientist.FullName);
        Console.WriteLine($"{tScientist};");
        Console.WriteLine($"{scientist};");
    }
    Console.WriteLine("#### #### #### #### #### #### #### #### #### #### #### ####");
}

if (test2) // TEST 2
{
    // States
    var tStates = await tDatabaseGway.GetTrelloStatesAsync();
    foreach (var tState in tStates)
    {
        var state = new State(null, tState.Code, tState.Name);
        Console.WriteLine($"{tState};");
        Console.WriteLine($"{state};");
    }
    Console.WriteLine("#### #### #### #### #### #### #### #### #### #### #### ####");

    // Experiments
    Console.WriteLine("Experiments: ");
    foreach (var tState in tStates)
    {
        var tStateGway = new TrelloStateGateway(network, tState);
        var tExperiments = await tStateGway.GetTrelloExperimentsAsync();
        foreach (var tExperiment in tExperiments)
        {
            var experiment = new Experiment(null, tExperiment.Code, tExperiment.Name, tExperiment.Description,
                                            true, tExperiment.StartDate, tExperiment.DueDate, 0, 0);
            Console.WriteLine($"{tExperiment};");
            Console.WriteLine($"{experiment};");
            // Remarks
            Console.WriteLine("Remarks: ");
            if (tExperiment.TrelloRemarks is not null && tExperiment.TrelloRemarks.Any())
            {
                foreach (var tRemark in tExperiment.TrelloRemarks)
                {
                    var remark = new Remark(null, tRemark.Code, tRemark.Text, tRemark.Date, 0, 0);
                    Console.WriteLine($"{tRemark}; ");
                    Console.WriteLine($"{remark}; ");
                }
            }
            // Participants
            Console.WriteLine("Partecipants: ");
            if (tExperiment.TrelloScientists is not null && tExperiment.TrelloScientists.Any())
            {
                foreach (var tScientist in tExperiment.TrelloScientists)
                {
                    var participant = new Participant(null, 0, 0);
                    Console.WriteLine($"{tScientist}; ");
                    Console.WriteLine($"{participant}; ");
                }
            }
            Console.WriteLine("#### #### #### #### #### #### #### #### #### #### #### ####");
        }
    }
}

if (test3) // TEST 3
{
    // States
    var tStates = await tDatabaseGway.GetTrelloStatesAsync();
    foreach (var tState in tStates)
    {
        var state = new State(null, tState.Code, tState.Name);
        Console.WriteLine($"{tState};");
        Console.WriteLine($"{state};");
    }
    Console.WriteLine("#### #### #### #### #### #### #### #### #### #### #### ####");

    // Experiments
    Console.WriteLine("Experiments: ");
    foreach (var tState in tStates)
    {
        var tStateGway = new TrelloStateGateway(network, tState);
        var tExperiments = await tStateGway.GetTrelloExperimentsSmartAsync();
        foreach (var tExperiment in tExperiments)
        {
            var experiment = new Experiment(null, tExperiment.Code, tExperiment.Name, tExperiment.Description,
                                            true, tExperiment.StartDate, tExperiment.DueDate, 0, 0);
            Console.WriteLine($"{tExperiment};");
            Console.WriteLine($"{experiment};");
            // Remarks
            Console.WriteLine("Remarks: ");
            if (tExperiment.TrelloRemarks is not null && tExperiment.TrelloRemarks.Any())
            {
                foreach (var tRemark in tExperiment.TrelloRemarks)
                {
                    var remark = new Remark(null, tRemark.Code, tRemark.Text, tRemark.Date, 0, 0);
                    Console.WriteLine($"{tRemark}; ");
                    Console.WriteLine($"{remark}; ");
                }
            }
            // Participants
            Console.WriteLine("Partecipants: ");
            if (tExperiment.TrelloScientists is not null && tExperiment.TrelloScientists.Any())
            {
                foreach (var tScientist in tExperiment.TrelloScientists)
                {
                    var participant = new Participant(null, 0, 0);
                    Console.WriteLine($"{tScientist}; ");
                    Console.WriteLine($"{participant}; ");
                }
            }
            Console.WriteLine("#### #### #### #### #### #### #### #### #### #### #### ####");
        }
    }
}

if (test4)
{
    var context = new ConcordiaContext();
    var experimentsGway = new ExperimentsGateway(context);
    var priorityGway = new PrioritiesGateway(context);
    var remarksGway = new RemarksGateway(context);
    var scientistsGway = new ScientistsGateway(context);
    var statesGway = new StatesGateway(context);
    var participantsGway = new ParticipantsGateway(context);

    var synchronizer = new Synchronizer(experimentsGway, priorityGway, remarksGway,
                                        scientistsGway, statesGway, participantsGway);

    await synchronizer.InitialSynchronizationAsync();
}

if (test5)
{
    var context = new ConcordiaContext();
    var experimentsGway = new ExperimentsGateway(context);
    var priorityGway = new PrioritiesGateway(context);
    var remarksGway = new RemarksGateway(context);
    var scientistsGway = new ScientistsGateway(context);
    var statesGway = new StatesGateway(context);
    var participantsGway = new ParticipantsGateway(context);

    var synchronizer = new Synchronizer(experimentsGway, priorityGway, remarksGway,
                                        scientistsGway, statesGway, participantsGway);

    await synchronizer.PeriodicSynchronizationAsync();
}

if (test6)
{
    var context = new ConcordiaContext();
    var experimentsGway = new ExperimentsGateway(context);
    var priorityGway = new PrioritiesGateway(context);
    var remarksGway = new RemarksGateway(context);
    var scientistsGway = new ScientistsGateway(context);
    var statesGway = new StatesGateway(context);
    var participantsGway = new ParticipantsGateway(context);

    var synchronizer = new Synchronizer(experimentsGway, priorityGway, remarksGway,
                                        scientistsGway, statesGway, participantsGway);

    await synchronizer.InitialSynchronizationSmartAsync();
}

if (test7)
{
    var context = new ConcordiaContext();
    var experimentsGway = new ExperimentsGateway(context);
    var priorityGway = new PrioritiesGateway(context);
    var remarksGway = new RemarksGateway(context);
    var scientistsGway = new ScientistsGateway(context);
    var statesGway = new StatesGateway(context);
    var participantsGway = new ParticipantsGateway(context);

    var synchronizer = new Synchronizer(experimentsGway, priorityGway, remarksGway,
                                        scientistsGway, statesGway, participantsGway);

    await synchronizer.PeriodicSynchronizationSmartAsync();
}

if (test8)
{
    SynchronizerSettings.SetSynchronizationTime(60);
    var delay = SynchronizerSettings.SynchronizationTimeDelay();
    Console.WriteLine($"DELAY: {delay}.");
}

if (test9)
{
    var context = new ConcordiaContext();
    var experimentsGway = new ExperimentsGateway(context);
    var scientistsGway = new ScientistsGateway(context);
    var statesGway = new StatesGateway(context);
    var participantsGway = new ParticipantsGateway(context);

    var reporter = new Reporter(experimentsGway, scientistsGway, statesGway, participantsGway);
    var report = reporter.Report();

    var sender = SenderSettings.GetSender();
    sender.SendMail("CONCORDIA REPORT", "Here's today report.", report);
}