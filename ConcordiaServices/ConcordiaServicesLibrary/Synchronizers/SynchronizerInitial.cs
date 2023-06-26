namespace ConcordiaServicesLibrary.Synchronizers;

using ConcordiaDBLibrary.Models.Classes;
using ConcordiaDBLibrary.Gateways.Abstract;

using ConcordiaTrelloLibrary;
using ConcordiaTrelloLibrary.Models.Classes;
using ConcordiaTrelloLibrary.Models.Extensions;
using ConcordiaTrelloLibrary.Gateways.Abstract.ITrelloGateways;
using ConcordiaTrelloLibrary.Gateways.Classes;
using ConcordiaTrelloLibrary.Networks;

using ConcordiaDBLibrary;
using ConcordiaDBLibrary.Data;
using ConcordiaDBLibrary.Gateways.Classes;

public partial class Synchronizer
{
    public virtual async Task InitialSynchronizationAsync()
    {
        // Database
        var tDatabaseGway = new TrelloDatabaseGateway(_network);
        await tDatabaseGway.InitTrelloObjectAsync(TrelloSettings.GetBoardCode());
        // Priorities
        var tPrioritiesAll = await tDatabaseGway.GetTrelloPrioritiesAsync();
        var tPriorities = tPrioritiesAll.Where(t => TrelloSettings.GetTrelloPrioritiesNames().Any(s => s.Equals(t.Name, StringComparison.OrdinalIgnoreCase)));
        if (tPriorities is not null && tPriorities.Any())
        {
            foreach (var tPriority in tPriorities)
            {
                if (_prioritiesGway.GetByCode(tPriority.Code) is null)
                {
                    var priority = _prioritiesGway.InsertByCode(new Priority(null, tPriority.Code, tPriority.Name, tPriority.Color));
                    if (priority is null || priority.Id is null) throw new Exception("Null priority.");
                }
            }
        }
        // Scientists
        var tScientists = await tDatabaseGway.GetTrelloScientistsAsync();
        if (tScientists is not null && tScientists.Any())
        {
            foreach (var tScientist in tScientists)
            {
                if (_scientistsGway.GetByCode(tScientist.Code) is null)
                {
                    var scientist = _scientistsGway.InsertByCode(new Scientist(null, tScientist.Code, tScientist.FullName));
                    if (scientist is null || scientist.Id is null) throw new Exception("Null scientist.");
                }
            }
        }
        // States
        var tStatesAll = await tDatabaseGway.GetTrelloStatesAsync();
        var tStates = tStatesAll.Where(t => TrelloSettings.GetTrelloStatesNames().Any(s => s.Equals(t.Name, StringComparison.OrdinalIgnoreCase)));
        if (tStates is not null && tStates.Any())
        {
            foreach (var tState in tStates)
            {
                if (_statesGway.GetByCode(tState.Code) is null)
                {
                    var state = _statesGway.InsertByCode(new State(null, tState.Code, tState.Name));
                    if (state is null || state.Id is null) throw new Exception("Null state.");
                }
            }
        }
        // Experiments
        if (tStates is not null && tStates.Any())
        {
            foreach (var tState in tStates)
            {
                var tStateGway = new TrelloStateGateway(_network, tState);
                var tExperiments = await tStateGway.GetTrelloExperimentsAsync();
                if (tExperiments is not null && tExperiments.Any())
                {
                    foreach (var tExperiment in tExperiments)
                    {
                        // Experiment
                        if (_experimentsGway.GetByCode(tExperiment.Code) is null)
                        {
                            var priority = _prioritiesGway.GetByCode(tExperiment.TrelloPriority.Code);
                            if (priority is null || priority.Id is null) throw new Exception("Null priority.");
                            var state = _statesGway.GetByCode(tExperiment.TrelloState.Code);
                            if (state is null || state.Id is null) throw new Exception("Null state.");
                            var experiment = _experimentsGway.InsertByCode(new Experiment(null, tExperiment.Code, tExperiment.Name, tExperiment.Description, true,
                                                                                          tExperiment.StartDate, tExperiment.DueDate, (int)priority!.Id!, (int)state!.Id!));
                            if (experiment is null || experiment.Id is null) throw new Exception("Null experiment.");
                            // Remarks
                            if (tExperiment.TrelloRemarks is not null && tExperiment.TrelloRemarks.Any())
                            {
                                foreach (var tRemark in tExperiment.TrelloRemarks)
                                {
                                    var author = _scientistsGway.GetByCode(tRemark.TrelloScientist.Code);
                                    if (author is null || author.Id is null) throw new Exception("Null author.");
                                    var remark = _remarksGway.InsertByCode(new Remark(null, tRemark.Code, tRemark.Text, tRemark.Date, (int)experiment!.Id!, (int)author!.Id!));
                                    if (remark is null || remark.Id is null) throw new Exception("Null remark.");
                                }
                            }
                            // Participants
                            if (tExperiment.TrelloScientists is not null && tExperiment.TrelloScientists.Any())
                            {
                                foreach (var tScientist in tExperiment.TrelloScientists)
                                {
                                    var scientist = _scientistsGway.GetByCode(tScientist.Code);
                                    if (scientist is null || scientist.Id is null) throw new Exception("Null scientist.");
                                    var partecipant = _participantsGway.Insert(new Participant(null, (int)experiment!.Id!, (int)scientist!.Id!));
                                    if (partecipant is null || partecipant.Id is null) throw new Exception("Null participant.");
                                }
                            }
                        }
                    }
                }
            }
        }
        times++;
    }

    public virtual async Task InitialSynchronizationSmartAsync()
    {
        // Database
        var tDatabaseGway = new TrelloDatabaseGateway(_network);
        await tDatabaseGway.InitTrelloObjectAsync(TrelloSettings.GetBoardCode());
        // Priorities
        var tPrioritiesAll = await tDatabaseGway.GetTrelloPrioritiesAsync();
        var tPriorities = tPrioritiesAll.Where(t => TrelloSettings.GetTrelloPrioritiesNames().Any(s => s.Equals(t.Name, StringComparison.OrdinalIgnoreCase)));
        foreach (var tPriority in tPriorities)
        {
            if (_prioritiesGway.GetByCode(tPriority.Code) is null)
            {
                var priority = _prioritiesGway.InsertByCode(new Priority(null, tPriority.Code, tPriority.Name, tPriority.Color));
                if (priority is null || priority.Id is null) throw new Exception("Null priority.");
            }
        }
        // Scientists
        var tScientistsSmart = tDatabaseGway.GetTrelloScientistsSmart();
        if (tScientistsSmart is not null && tScientistsSmart.Any())
        {
            foreach (var tScientist in tScientistsSmart)
            {
                if (_scientistsGway.GetByCode(tScientist.Code) is null)
                {
                    var scientist = _scientistsGway.InsertByCode(new Scientist(null, tScientist.Code, tScientist.FullName));
                    if (scientist is null || scientist.Id is null) throw new Exception("Null scientist.");
                }
            }
        }
        // States
        var tStatesAll = await tDatabaseGway.GetTrelloStatesAsync();
        var tStates = tStatesAll.Where(t => TrelloSettings.GetTrelloStatesNames().Any(s => s.Equals(t.Name, StringComparison.OrdinalIgnoreCase)));
        if (tStates is not null && tStates.Any())
        {
            foreach (var tState in tStates)
            {
                if (_statesGway.GetByCode(tState.Code) is null)
                {
                    var state = _statesGway.InsertByCode(new State(null, tState.Code, tState.Name));
                    if (state is null || state.Id is null) throw new Exception("Null state.");
                }
            }
        }
        // Experiments
        if (tStates is not null && tStates.Any())
        {
            foreach (var tState in tStates)
            {
                var tStateGway = new TrelloStateGateway(_network, tState);
                var tExperimentsSmart = await tStateGway.GetTrelloExperimentsSmartAsync();
                if (tExperimentsSmart is not null && tExperimentsSmart.Any())
                {
                    foreach (var tExperiment in tExperimentsSmart)
                    {
                        // Experiment
                        if (_experimentsGway.GetByCode(tExperiment.Code) is null)
                        {
                            var priority = _prioritiesGway.GetByCode(tExperiment.TrelloPriority.Code);
                            if (priority is null || priority.Id is null) throw new Exception("Null priority.");
                            var state = _statesGway.GetByCode(tExperiment.TrelloState.Code);
                            if (state is null || state.Id is null) throw new Exception("Null state.");
                            var experiment = _experimentsGway.InsertByCode(new Experiment(null, tExperiment.Code, tExperiment.Name, tExperiment.Description, true,
                                                                                          tExperiment.StartDate, tExperiment.DueDate, (int)priority!.Id!, (int)state!.Id!));
                            if (experiment is null || experiment.Id is null) throw new Exception("Null experiment.");
                            // Remarks
                            if (tExperiment.TrelloRemarks is not null && tExperiment.TrelloRemarks.Any())
                            {
                                foreach (var tRemark in tExperiment.TrelloRemarks)
                                {
                                    var author = _scientistsGway.GetByCode(tRemark.TrelloScientist.Code);
                                    if (author is null || author.Id is null) throw new Exception("Null author.");
                                    var remark = _remarksGway.InsertByCode(new Remark(null, tRemark.Code, tRemark.Text, tRemark.Date, (int)experiment!.Id!, (int)author!.Id!));
                                    if (remark is null || remark.Id is null) throw new Exception("Null remark.");
                                }
                            }
                            // Participants
                            if (tExperiment.TrelloScientists is not null && tExperiment.TrelloScientists.Any())
                            {
                                foreach (var tScientist in tExperiment.TrelloScientists)
                                {
                                    var scientist = _scientistsGway.GetByCode(tScientist.Code);
                                    if (scientist is null || scientist.Id is null) throw new Exception("Null scientist.");
                                    var partecipant = _participantsGway.Insert(new Participant(null, (int)experiment!.Id!, (int)scientist!.Id!));
                                    if (partecipant is null || partecipant.Id is null) throw new Exception("Null participant.");
                                }
                            }
                        }
                    }
                }
            }
        }
        times++;
    }
}