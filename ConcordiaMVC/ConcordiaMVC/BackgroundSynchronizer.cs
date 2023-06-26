namespace ConcordiaMVC;

using System;
using System.Threading;
using System.Threading.Tasks;

using ConcordiaTrelloLibrary;
using ConcordiaTrelloLibrary.Networks;

using ConcordiaDBLibrary.Data;
using ConcordiaDBLibrary.Gateways.Abstract;
using ConcordiaDBLibrary.Models.Classes;

using ConcordiaServicesLibrary;
using ConcordiaServicesLibrary.Synchronizers;

public class BackgroundSynchronizer : BackgroundService
{
    private readonly Synchronizer _synchronizer;

    public bool Synchronized { get; set; }

    public BackgroundSynchronizer(Synchronizer synchronizer) 
	 : base()
    {
        _synchronizer = synchronizer;
    }

    public BackgroundSynchronizer(ITrelloEntityGateway<Experiment> experimentsGway,
                        ITrelloEntityGateway<Priority> prioritiesGway,
                        ITrelloEntityGateway<Remark> remarksGway,
                        ITrelloEntityGateway<Scientist> scientistsGway,
                        ITrelloEntityGateway<State> statesGway,
                        IEntityGateway<Participant> participantsGway,
                        TrelloNetwork network)
     : base()
    {
        _synchronizer = new Synchronizer(experimentsGway, prioritiesGway, remarksGway, 
	                                     scientistsGway, statesGway, participantsGway, 
					                     network);
    }

    public BackgroundSynchronizer(ITrelloEntityGateway<Experiment> experimentsGway,
                    ITrelloEntityGateway<Priority> prioritiesGway,
                    ITrelloEntityGateway<Remark> remarksGway,
                    ITrelloEntityGateway<Scientist> scientistsGway,
                    ITrelloEntityGateway<State> statesGway,
                    IEntityGateway<Participant> participantsGway)
     : base()
    {
        _synchronizer = new Synchronizer(experimentsGway, prioritiesGway, remarksGway,
                                         scientistsGway, statesGway, participantsGway);
    }

    public BackgroundSynchronizer(ConcordiaContext context)
     : base()
    {
        _synchronizer = new Synchronizer(context);
    }

    public BackgroundSynchronizer()
     : base()
    {
        _synchronizer = new Synchronizer();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (await TrelloSettings.IsBoardAccessibleAsync())
            {
                Synchronized = true;
                // await _synchronizer.RunTestAsync();
                // await _synchronizer.RunAsync();
                await _synchronizer.RunSmartAsync();
                Synchronized = false;
                // delaying any synchronization.
                await Task.Delay(SynchronizerSettings.SynchronizationTimeDelay(), stoppingToken);
            }
            await Task.Delay(TimeSpan.FromMinutes(4), stoppingToken);
        }
    }
}