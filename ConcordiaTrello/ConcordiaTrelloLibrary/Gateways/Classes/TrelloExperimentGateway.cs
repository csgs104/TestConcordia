namespace ConcordiaTrelloLibrary.Gateways.Classes;

using TrelloDotNet.Model;
using Abstract;
using Abstract.ITrelloGateways;
using Models.Classes;
using Models.Extensions;
using Networks;

public class TrelloExperimentGateway : ITrelloExperimentGateway
{
    public TrelloNetwork TrelloNetwork { get; set; }
    public TrelloExperiment? TrelloExperiment { get; set; }

    public TrelloExperimentGateway(TrelloNetwork network)
    {
        TrelloNetwork = network;
        TrelloExperiment = null;
    }

    public TrelloExperimentGateway(TrelloNetwork network, TrelloExperiment experiment)
    {
        TrelloNetwork = network;
        TrelloExperiment = experiment;
    }

    public async Task InitTrelloObjectAsync(TrelloState state, string code)
    {
        var card = await TrelloNetwork.GetCardAsync(code);
        var scientists = new List<TrelloScientist>();
        var members = await TrelloNetwork.GetMembersOfCardAsync(code);
        foreach (var member in members)
        {
            scientists.Add(new TrelloScientist(member.Id, member.FullName));
        }
        var remarks = new List<TrelloRemark>();
        var comments = await TrelloNetwork.GetAllCommentsOnCardAsync(code);
        foreach (var comment in comments)
        {
            remarks.Add(new TrelloRemark(comment.Id, comment.Data.Text, comment.Date,
                        new TrelloScientist(comment.MemberCreatorId, comment.MemberCreator.FullName),
                        state.TrelloDatabase));
        }
        var label = card.Labels[0];
        var priority = new TrelloPriority(label.Id, label.Name, label.Color);
        TrelloExperiment = new TrelloExperiment(card.Id, card.Name, card.Description, card.Start,
                                                card.Due, priority, state, scientists, remarks);
    }

    public async Task InitTrelloObjectSmartAsync(TrelloState state, string code)
    {
        var card = await TrelloNetwork.GetCardAsync(code);
        var scientists = new List<TrelloScientist>();
        var remarks = new List<TrelloRemark>();
        var comments = await TrelloNetwork.GetAllCommentsOnCardAsync(code);
        foreach (var comment in comments)
        {
            var remark = new TrelloRemark(comment.Id, comment.Data.Text, comment.Date,
                                          null, state.TrelloDatabase);
            remark.SetScientistFromText();
            remarks.Add(remark);
        }
        var label = card.Labels[0];
        var priority = new TrelloPriority(label.Id, label.Name, label.Color);
        var experiment = new TrelloExperiment(card.Id, card.Name, card.Description, card.Start,
                                              card.Due, priority, state, scientists, remarks);
        experiment.SetScientistsFromDescription();
        TrelloExperiment = experiment;
    }

    public async Task AddTrelloRemarkAsync(TrelloRemark remark)
    {
        if (TrelloExperiment is null) throw new Exception("TrelloExperiment not initialized.");
        var comment = new Comment(remark.Text);
        await TrelloNetwork.AddCommentAsync(TrelloExperiment.Code, comment);
      
    }

    public async Task AddTrelloRemarkSmartAsync(TrelloRemark remark)
    {
        if (TrelloExperiment is null) throw new Exception("TrelloExperiment not initialized.");
        var comment = new Comment($"{remark.Text}{Environment.NewLine}{TrelloSmartSettings.GetRemarkDivisor()} {remark.TrelloScientist.FullName}.");
        await TrelloNetwork.AddCommentAsync(TrelloExperiment.Code, comment);
    }

    public async Task MoveToTrelloStateAsync(TrelloState state)
    {
        if (TrelloExperiment is null) throw new Exception("TrelloExperiment not initialized.");
        TrelloExperiment.TrelloState = state;
        var card = await TrelloNetwork.GetCardAsync(TrelloExperiment.Code);
        await TrelloNetwork.MoveCardToListAsync(card.Id, state.Code);
    }

    public async Task<TrelloRemark?> GetLastTrelloRemarkAsync()
    {
        if (TrelloExperiment is null) throw new Exception("TrelloExperiment not initialized.");
        var comments = await TrelloNetwork.GetAllCommentsOnCardAsync(TrelloExperiment.Code);
        var comment = comments.Where(a => a.Type == "commentCard").OrderByDescending(c => c.Date);
        if (comments.Count > 0)
        {
            var last = comments.First();
            var scientist = new TrelloScientist(last.MemberCreatorId, last.MemberCreator.FullName);
            return new TrelloRemark(last.Id, last.Data.Text, last.Date, scientist, TrelloExperiment.TrelloState.TrelloDatabase);
        }
        return null;
    }

    public async Task<TrelloRemark?> GetLastTrelloRemarkSmartAsync()
    {
        if (TrelloExperiment is null) throw new Exception("TrelloExperiment not initialized.");
        var comments = await TrelloNetwork.GetAllCommentsOnCardAsync(TrelloExperiment.Code);
        var comment = comments.Where(a => a.Type == "commentCard").OrderByDescending(c => c.Date);
        if (comments.Count > 0)
        {
            var last = comments.First();
            var remark = new TrelloRemark(last.Id, last.Data.Text, last.Date, null, TrelloExperiment.TrelloState.TrelloDatabase);
            remark.SetScientistFromText();
            return remark;
        }
        return null;
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