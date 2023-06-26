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
    public virtual async Task PeriodicSynchronizationAsync()
    {
        // Database
        var tDatabaseGway = new TrelloDatabaseGateway(_network);
        await tDatabaseGway.InitTrelloObjectAsync(TrelloSettings.GetBoardCode());
        // Experiments and Remarks from DB
        var experimentsAll = _experimentsGway.GetAll().ToList();
        var remarksAll = _remarksGway.GetAll().ToList();
        var remarksNull = remarksAll.Where(e => e.Code is null).ToList();
        var remarksNotNull = remarksAll.Where(e => e.Code is not null).ToList();
        // Scientists
        var tScientists = await tDatabaseGway.GetTrelloScientistsAsync();
        if (tScientists is null || !tScientists.Any()) throw new Exception("Null scientists.");
        // States
        var tStatesAll = await tDatabaseGway.GetTrelloStatesAsync();
        var tStates = tStatesAll.Where(t => TrelloSettings.GetTrelloStatesNames().Any(s => s.Equals(t.Name, StringComparison.OrdinalIgnoreCase)));
        if (tStates is null || !tStates.Any()) throw new Exception("Null states.");
        // From DB to Trello
        foreach (var tState in tStates)
        {
            var tStateGway = new TrelloStateGateway(_network, tState);
            var tExperiments = await tStateGway.GetTrelloExperimentsAsync();
            if (tExperiments is not null && tExperiments.Any())
            {
                foreach (var tExperiment in tExperiments)
                {
                    // Experiment
                    var experiment = _experimentsGway.GetByCode(tExperiment!.Code!);
                    if (experiment is not null)
                    {
                        var tExperimentGway = new TrelloExperimentGateway(_network);
                        await tExperimentGway.InitTrelloObjectAsync(tState, tExperiment!.Code!);
                        // Updating TrelloExperiment
                        if (experiment.State!.Code! != tExperiment.TrelloState!.Code!)
                        {
                            await tExperimentGway.MoveToTrelloStateAsync(tStates.SingleOrDefault(t => t.Code! == experiment.State!.Code!)!);
                            _experimentsGway.Update(new Experiment(experiment.Id, experiment.Code, experiment.Name, experiment.Description, true,
                                                                  experiment.StartDate, experiment.DueDate, experiment.PriorityId, experiment.StateId));
                        }
                        // Adding Remarks to TrelloExperiment
                        if (remarksNull is not null && remarksNull.Any())
                        {
                            var remarks = remarksNull.Where(e => e.ExperimentId == experiment.Id).ToList();
                            foreach (var remark in remarks)
                            {
                                // Scientist
                                var scientist = _scientistsGway.GetById(remark.ScientistId);
                                if (scientist is null || scientist.Id is null) throw new Exception("Null scientist.");
                                // Adding Remark to TrelloExperiment
                                var tAuthor = tScientists.SingleOrDefault(t => t.Code! == scientist.Code!);
                                await tExperimentGway.AddTrelloRemarkAsync(new TrelloRemark(null, remark.Text, null, tAuthor, tDatabaseGway.TrelloDatabase));
                                var tRemark = await tExperimentGway.GetLastTrelloRemarkAsync();
                                // Updating Remark
                                var author = _scientistsGway.GetByCode(tRemark.TrelloScientist.Code);
                                if (author is null || author.Id is null) throw new Exception("Null author.");
                                var remarkUpdated = _remarksGway.Update(new Remark(remark.Id, tRemark.Code, remark.Text, tRemark.Date, remark.ExperimentId, (int)author!.Id!));
                                if (remarkUpdated is null || remarkUpdated.Id is null) throw new Exception("Null remark.");
                            }
                        }
                    }
                }
            }
        }
        // From Trello to DB
        foreach (var tState in tStates)
        {
            var tStateGway = new TrelloStateGateway(_network, tState);
            var tExperiments = await tStateGway.GetTrelloExperimentsAsync();
            if (tExperiments is not null && tExperiments.Any())
            {
                foreach (var tExperiment in tExperiments)
                {
                    if (!experimentsAll.Any(e => e.Code == tExperiment.Code))
                    {
                        // Experiment
                        var priority = _prioritiesGway.GetByCode(tExperiment.TrelloPriority.Code);
                        if (priority is null || priority.Id is null) throw new Exception("Null priority.");
                        var state = _statesGway.GetByCode(tExperiment.TrelloState.Code);
                        if (state is null || state.Id is null) throw new Exception("Null state.");
                        var experiment = _experimentsGway.InsertByCode(new Experiment(null, tExperiment.Code, tExperiment.Name, tExperiment.Description, true,
                                                                                      tExperiment.StartDate, tExperiment.DueDate, (int)priority!.Id!, (int)state!.Id!));
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
                    else
                    {
                        // Experiment
                        var experiment = _experimentsGway.GetByCode(tExperiment!.Code!);
                        if (experiment is not null)
                        {
                            // Remarks
                            if (tExperiment.TrelloRemarks is not null && tExperiment.TrelloRemarks.Any())
                            {
                                foreach (var tRemark in tExperiment.TrelloRemarks)
                                {
                                    if (!remarksNotNull.Any(e => e.Code == tRemark.Code))
                                    {
                                        var author = _scientistsGway.GetByCode(tRemark.TrelloScientist.Code);
                                        if (author is null || author.Id is null) throw new Exception("Null author.");
                                        var remark = _remarksGway.InsertByCode(new Remark(null, tRemark.Code, tRemark.Text, tRemark.Date, (int)experiment!.Id!, (int)author!.Id!));
                                        if (remark is null || remark.Id is null) throw new Exception("Null remark.");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        times++;
    }

    public virtual async Task PeriodicSynchronizationSmartAsync()
    {
        // Database
        var tDatabaseGway = new TrelloDatabaseGateway(_network);
        await tDatabaseGway.InitTrelloObjectAsync(TrelloSettings.GetBoardCode());
        // Experiments and Remarks from DB
        var experimentsAll = _experimentsGway.GetAll().ToList();
        var remarksAll = _remarksGway.GetAll().ToList();
        var remarksNull = remarksAll.Where(e => e.Code is null).ToList();
        var remarksNotNull = remarksAll.Where(e => e.Code is not null).ToList();
        // Scientists
        var tScientists = tDatabaseGway.GetTrelloScientistsSmart();
        if (tScientists is null || !tScientists.Any()) throw new Exception("Null scientists.");
        // States
        var tStatesAll = await tDatabaseGway.GetTrelloStatesAsync();
        var tStates = tStatesAll.Where(t => TrelloSettings.GetTrelloStatesNames().Any(s => s.Equals(t.Name, StringComparison.OrdinalIgnoreCase)));
        if (tStates is null || !tStates.Any()) throw new Exception("Null states.");
        // From DB to Trello
        foreach (var tState in tStates)
        {
            var tStateGway = new TrelloStateGateway(_network, tState);
            var tExperiments = await tStateGway.GetTrelloExperimentsSmartAsync();
            if (tExperiments is not null && tExperiments.Any())
            {
                foreach (var tExperiment in tExperiments)
                {
                    // Experiment
                    var experiment = _experimentsGway.GetByCode(tExperiment!.Code!);
                    if (experiment is not null)
                    {
                        var tExperimentGway = new TrelloExperimentGateway(_network);
                        await tExperimentGway.InitTrelloObjectSmartAsync(tState, tExperiment!.Code!);
                        // Updating TrelloExperiment
                        if (experiment.State!.Code! != tExperiment.TrelloState!.Code!)
                        {
                            await tExperimentGway.MoveToTrelloStateAsync(tStates.SingleOrDefault(t => t.Code! == experiment.State!.Code!)!);
                            _experimentsGway.Update(new Experiment(experiment.Id, experiment.Code, experiment.Name, experiment.Description, true,
                                                                  experiment.StartDate, experiment.DueDate, experiment.PriorityId, experiment.StateId));
                        }
                        if (remarksNull is not null && remarksNull.Any())
                        {
                            var remarks = remarksNull.Where(e => e.ExperimentId == experiment.Id);
                            foreach (var remark in remarks)
                            {
                                // Scientist
                                var scientist = _scientistsGway.GetById(remark.ScientistId);
                                if (scientist is null || scientist.Id is null) throw new Exception("Null scientist.");
                                // Adding Remark to TrelloExperiment
                                var tAuthor = tScientists.SingleOrDefault(t => t.Code! == scientist.Code!);
                                await tExperimentGway.AddTrelloRemarkSmartAsync(new TrelloRemark(null, remark.Text, null, tAuthor, tDatabaseGway.TrelloDatabase));
                                var tRemark = await tExperimentGway.GetLastTrelloRemarkAsync();
                                // Updating Remark
                                var remarkUpdated = _remarksGway.Update(new Remark(remark.Id, tRemark.Code, remark.Text, tRemark.Date, remark.ExperimentId, (int)scientist.Id));
                                if (remarkUpdated is null || remarkUpdated.Id is null) throw new Exception("Null remark.");
                            }
                        }
                    }
                }
            }
        }
        // From Trello to DB
        foreach (var tState in tStates)
        {
            var tStateGway = new TrelloStateGateway(_network, tState);
            var tExperiments = await tStateGway.GetTrelloExperimentsSmartAsync();
            if (tExperiments is not null && tExperiments.Any())
            {
                foreach (var tExperiment in tExperiments)
                {
                    if (!experimentsAll.Any(e => e.Code == tExperiment.Code))
                    {
                        // Experiment
                        var priority = _prioritiesGway.GetByCode(tExperiment.TrelloPriority.Code);
                        if (priority is null || priority.Id is null) throw new Exception("Null priority.");
                        var state = _statesGway.GetByCode(tExperiment.TrelloState.Code);
                        if (state is null || state.Id is null) throw new Exception("Null state.");
                        var experiment = _experimentsGway.InsertByCode(new Experiment(null, tExperiment.Code, tExperiment.Name, tExperiment.Description, true,
                                                                                      tExperiment.StartDate, tExperiment.DueDate, (int)priority!.Id!, (int)state!.Id!));
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
                    else
                    {
                        // Experiment
                        var experiment = _experimentsGway.GetByCode(tExperiment!.Code!);
                        if (experiment is not null)
                        {
                            // Remarks
                            if (tExperiment.TrelloRemarks is not null && tExperiment.TrelloRemarks.Any())
                            {
                                foreach (var tRemark in tExperiment.TrelloRemarks)
                                {
                                    if (!remarksNotNull.Any(e => e.Code == tRemark.Code))
                                    {
                                        var author = _scientistsGway.GetByCode(tRemark.TrelloScientist.Code);
                                        if (author is null || author.Id is null) throw new Exception("Null author.");
                                        var remark = _remarksGway.InsertByCode(new Remark(null, tRemark.Code, tRemark.Text, tRemark.Date, (int)experiment!.Id!, (int)author!.Id!));
                                        if (remark is null || remark.Id is null) throw new Exception("Null remark.");
                                    }
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