using ConcordiaTrelloLibrary;
using ConcordiaTrelloLibrary.Gateways.Classes;

var network = TrelloSettings.GetConcordiaAD();
var tDatabaseGway = new TrelloDatabaseGateway(network);
await tDatabaseGway.InitTrelloObjectAsync(TrelloSettings.GetBoardCodeOfConcordia());

var test1 = true;
var test2 = true;
var test3 = true;

if (test1) // TEST 1
{
    // Priorities
    var tPrioritiesAll = await tDatabaseGway.GetTrelloPrioritiesAsync();
    Console.WriteLine("PrioritiesAll: ");
    foreach (var tPriority in tPrioritiesAll)
    {
        Console.WriteLine($"{tPriority};");
    }
    Console.WriteLine("#### #### #### #### #### #### #### #### #### #### #### ####");

    var tPriorities = tPrioritiesAll.Where(t => t.Name != string.Empty);
    Console.WriteLine("Priorities: ");
    foreach (var tPriority in tPriorities)
    {
        Console.WriteLine($"{tPriority};");
    }
    Console.WriteLine("#### #### #### #### #### #### #### #### #### #### #### ####");

    // Scientists
    var tScientists = await tDatabaseGway.GetTrelloScientistsAsync();
    foreach (var tScientist in tScientists)
    {
        Console.WriteLine($"{tScientist};");
    }
    Console.WriteLine("#### #### #### #### #### #### #### #### #### #### #### ####");

    var tScientistsSmart = tDatabaseGway.GetTrelloScientistsSmart();
    foreach (var tScientist in tScientistsSmart)
    {
        Console.WriteLine($"{tScientist};");
    }
    Console.WriteLine("#### #### #### #### #### #### #### #### #### #### #### ####");
}

if (test2) // TEST 2
{
    // States
    var tStates = await tDatabaseGway.GetTrelloStatesAsync();
    foreach (var tState in tStates)
    {
        Console.WriteLine($"{tState};");
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
            Console.WriteLine($"{tExperiment};");
            // Remarks
            Console.WriteLine("Remarks: ");
            if (tExperiment.TrelloRemarks is not null && tExperiment.TrelloRemarks.Any())
            {
                foreach (var tRemark in tExperiment.TrelloRemarks)
                {
                    Console.WriteLine($"{tRemark}; ");
                }
            }
            // Participants
            Console.WriteLine("Partecipants: ");
            if (tExperiment.TrelloScientists is not null && tExperiment.TrelloScientists.Any())
            {
                foreach (var tScientist in tExperiment.TrelloScientists)
                {
                    Console.WriteLine($"{tScientist}; ");
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
        Console.WriteLine($"{tState};");
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
            Console.WriteLine($"{tExperiment};");
            // Remarks
            Console.WriteLine("Remarks: ");
            if (tExperiment.TrelloRemarks is not null && tExperiment.TrelloRemarks.Any())
            {
                foreach (var tRemark in tExperiment.TrelloRemarks)
                {
                    Console.WriteLine($"{tRemark}; ");
                }
            }
            // Participants
            Console.WriteLine("Partecipants: ");
            if (tExperiment.TrelloScientists is not null && tExperiment.TrelloScientists.Any())
            {
                foreach (var tScientist in tExperiment.TrelloScientists)
                {
                    Console.WriteLine($"{tScientist}; ");
                }
            }
            Console.WriteLine("#### #### #### #### #### #### #### #### #### #### #### ####");
        }
    }
}