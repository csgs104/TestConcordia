namespace ConcordiaTrelloLibrary.Gateways.Abstract.ITrelloGateways;

using Models.Classes;
using Gateways.Abstract;

public interface ITrelloStateGateway : ITrelloObjectGateway<TrelloState>
{
    public Task InitTrelloObjectAsync(TrelloDatabase database, string code);

    public Task<IEnumerable<TrelloExperiment>> GetTrelloExperimentsAsync();

    public Task<IEnumerable<TrelloExperiment>> GetTrelloExperimentsSmartAsync();
}