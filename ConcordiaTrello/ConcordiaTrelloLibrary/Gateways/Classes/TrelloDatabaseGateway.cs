namespace ConcordiaTrelloLibrary.Gateways.Classes;

using TrelloDotNet.Model;
using Abstract;
using Abstract.ITrelloGateways;
using Models.Classes;
using Models.Extensions;
using Networks;


public class TrelloDatabaseGateway : ITrelloDatabaseGateway
{
    public TrelloNetwork TrelloNetwork { get; set; }
    public TrelloDatabase? TrelloDatabase { get; set; }

    public TrelloDatabaseGateway(TrelloNetwork network) 
    {
        TrelloNetwork = network;
        TrelloDatabase = null;
    }

    public TrelloDatabaseGateway(TrelloNetwork network, TrelloDatabase database)
    {
        TrelloNetwork = network;
        TrelloDatabase = database;
    }

    public async Task InitTrelloObjectAsync(string code)
    {
        var board = await TrelloNetwork.GetBoardAsync(code);
        TrelloDatabase = new TrelloDatabase(board.Id, board.Name, board.Description);
    }

    public async Task<IEnumerable<TrelloState>> GetTrelloStatesAsync()
    {
        if (TrelloDatabase is null) throw new Exception("TrelloDatabase not initialized.");
        var lists = await TrelloNetwork.GetListsOnBoardAsync(TrelloDatabase.Code);
        var states = new List<TrelloState>();
        foreach (var list in lists) states.Add(new TrelloState(list.Id, list.Name, TrelloDatabase));
        return states;
    }

    public async Task<IEnumerable<TrelloPriority>> GetTrelloPrioritiesAsync()
    {
        if (TrelloDatabase is null) throw new Exception("TrelloDatabase not initialized.");
        var lables = await TrelloNetwork.GetLabelsOfBoardAsync(TrelloDatabase.Code);
        var priorities = new List<TrelloPriority>();
        foreach (var label in lables) priorities.Add(new TrelloPriority(label.Id, label.Name, label.Color));
        return priorities;
    }

    public async Task<IEnumerable<TrelloScientist>> GetTrelloScientistsAsync()
    {
        if (TrelloDatabase is null) throw new Exception("TrelloDatabase not initialized.");
        var members = await TrelloNetwork.GetMembersOfBoardAsync(TrelloDatabase.Code);
        var scientists = new List<TrelloScientist>();
        foreach (var member in members) scientists.Add(new TrelloScientist(member.Id, member.FullName));
        return scientists;
    }

    public IEnumerable<TrelloScientist> GetTrelloScientistsSmart()
    {
        if (TrelloDatabase is null) throw new Exception("TrelloDatabase not initialized.");
        return TrelloDatabase.GetScientistsFromDescription();
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