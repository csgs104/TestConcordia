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

using ConcordiaServicesLibrary.Reporters;
using ConcordiaServicesLibrary.Senders;
using ConcordiaServicesLibrary;

public partial class Synchronizer
{
    private static int times = 0;

    private readonly ITrelloEntityGateway<Experiment> _experimentsGway;
    private readonly ITrelloEntityGateway<Priority> _prioritiesGway;
    private readonly ITrelloEntityGateway<Remark> _remarksGway;
    private readonly ITrelloEntityGateway<Scientist> _scientistsGway;
    private readonly ITrelloEntityGateway<State> _statesGway;
    private readonly IEntityGateway<Participant> _participantsGway;

    private readonly TrelloNetwork _network;

    private readonly Sender _sender;
    private readonly Reporter _reporter;

    public Synchronizer(ITrelloEntityGateway<Experiment> experimentsGway,
                        ITrelloEntityGateway<Priority> prioritiesGway,
                        ITrelloEntityGateway<Remark> remarksGway,
                        ITrelloEntityGateway<Scientist> scientistsGway,
                        ITrelloEntityGateway<State> statesGway,
                        IEntityGateway<Participant> participantsGway,
                        TrelloNetwork network) 
    {
        _experimentsGway = experimentsGway;
        _prioritiesGway = prioritiesGway;
        _remarksGway = remarksGway;
        _scientistsGway = scientistsGway;
        _statesGway = statesGway;
        _participantsGway = participantsGway;
        _network = network;
        _sender = SenderSettings.GetSender();
        _reporter = new Reporter(experimentsGway, scientistsGway, statesGway, participantsGway);
    }

    public Synchronizer(ITrelloEntityGateway<Experiment> experimentsGway,
                        ITrelloEntityGateway<Priority> prioritiesGway,
                        ITrelloEntityGateway<Remark> remarksGway,
                        ITrelloEntityGateway<Scientist> scientistsGway,
                        ITrelloEntityGateway<State> statesGway,
                        IEntityGateway<Participant> participantsGway)
     : this(experimentsGway, prioritiesGway, remarksGway, scientistsGway, 
	        statesGway, participantsGway, TrelloSettings.GetBoardAD())
    { }

    public Synchronizer(ConcordiaContext context)
	 : this(new ExperimentsGateway(context), new PrioritiesGateway(context),
            new RemarksGateway(context), new ScientistsGateway(context), 
	        new StatesGateway(context), new ParticipantsGateway(context))
    { }

    public Synchronizer() 
	 : this(new ConcordiaContext())
    { }

    public virtual async Task RunAsync()
    {
        if (times == 0) await InitialSynchronizationAsync();
        else await PeriodicSynchronizationAsync();
        var attachment = _reporter.Report();
        _sender.SendMail("CONCORDIA REPORT", "Hello, this is today report", attachment);
    }

    public virtual async Task RunSmartAsync()
    {
        if (times == 0) await InitialSynchronizationSmartAsync();
        else await PeriodicSynchronizationSmartAsync();
        var attachment = _reporter.Report();
        _sender.SendMail("CONCORDIA REPORT", "Hello, this is today report", attachment);
    }

    public virtual async Task RunTestAsync()
    {
        if (times == 0) await Task.Delay(TimeSpan.FromSeconds(30));
        else await Task.Delay(TimeSpan.FromSeconds(30));
        var attachment = _reporter.Report();
        _sender.SendMail("CONCORDIA REPORT", "Hello, this is today report", attachment);
    }
}