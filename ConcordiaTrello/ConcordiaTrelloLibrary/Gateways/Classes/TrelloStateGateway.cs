namespace ConcordiaTrelloLibrary.Gateways.Classes;

using TrelloDotNet.Model;
using Abstract;
using Abstract.ITrelloGateways;
using Models.Classes;
using Models.Extensions;
using Networks;

public class TrelloStateGateway : ITrelloStateGateway
{
    public TrelloNetwork TrelloNetwork { get; set; }
    public TrelloState? TrelloState { get; set; }

    public TrelloStateGateway(TrelloNetwork network)
    {
        TrelloNetwork = network;
        TrelloState = null;
    }

    public TrelloStateGateway(TrelloNetwork network, TrelloState state)
    {
        TrelloNetwork = network;
        TrelloState = state;
    }

    public async Task InitTrelloObjectAsync(TrelloDatabase database, string code)
    {
        var list = await TrelloNetwork.GetListAsync(code);
        TrelloState = new TrelloState(list.Id, list.Name, database);
    }

    // The right way, an assigned scientist can be only a member added on a card 
    public async Task<IEnumerable<TrelloExperiment>> GetTrelloExperimentsAsync()
    {
        if (TrelloState is null) throw new Exception("TrelloState not initialized.");
        var cards = await TrelloNetwork.GetCardsInListAsync(TrelloState.Code);
        var experiments = new List<TrelloExperiment>();
        foreach (var card in cards)
        {
            var scientists = new List<TrelloScientist>();
            var members = await TrelloNetwork.GetMembersOfCardAsync(card.Id);
            foreach (var member in members)
            {
                scientists.Add(new TrelloScientist(member.Id, member.FullName));
            }
            var remarks = new List<TrelloRemark>();
            var comments = await TrelloNetwork.GetAllCommentsOnCardAsync(card.Id);
            foreach (var comment in comments)
            {
                remarks.Add(new TrelloRemark(comment.Id, comment.Data.Text, comment.Date,
                            new TrelloScientist(comment.MemberCreatorId, comment.MemberCreator.FullName),
		                    TrelloState.TrelloDatabase));
            }
            var labels = card.Labels;
            var priority = new TrelloPriority(labels[0].Id, labels[0].Name, labels[0].Color);
            experiments.Add(new TrelloExperiment(card.Id, card.Name, card.Description, card.Start,
                                                 card.Due, priority, TrelloState, scientists, remarks));
        }
        return experiments;
    }

    // The smart way, an assigned scientist can be a member added on ... (must be put at the end)
    public async Task<IEnumerable<TrelloExperiment>> GetTrelloExperimentsSmartAsync()
    {
        if (TrelloState is null) throw new Exception("TrelloState not initialized.");
        var cards = await TrelloNetwork.GetCardsInListAsync(TrelloState.Code);
        var experiments = new List<TrelloExperiment>();
        foreach (var card in cards)
        {
            var scientists = new List<TrelloScientist>();
            var remarks = new List<TrelloRemark>();
            var comments = await TrelloNetwork.GetAllCommentsOnCardAsync(card.Id);
            foreach (var comment in comments)
            {
                var remark = new TrelloRemark(comment.Id, comment.Data.Text, comment.Date, null, TrelloState.TrelloDatabase);
                remark.SetScientistFromText();
                remarks.Add(remark);
            }
            var labels = card.Labels;
            var priority = new TrelloPriority(labels[0].Id, labels[0].Name, labels[0].Color);
            var experiment = new TrelloExperiment(card.Id, card.Name, card.Description, card.Start,
                                                  card.Due, priority, TrelloState, scientists, remarks);
            experiment.SetScientistsFromDescription();
            experiments.Add(experiment);
        }
        return experiments;
    }

    public Task<bool> Create()
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update()
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete()
    {
        throw new NotImplementedException();
    }
}