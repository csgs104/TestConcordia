namespace ConcordiaTrelloLibrary.Gateways.Abstract.ITrelloGateways;

using Models.Classes;
using Gateways.Abstract;

public interface ITrelloExperimentGateway : ITrelloObjectGateway<TrelloExperiment>
{
    public Task InitTrelloObjectAsync(TrelloState state, string code);

    public Task InitTrelloObjectSmartAsync(TrelloState state, string code);

    public Task AddTrelloRemarkAsync(TrelloRemark remark);

    public Task AddTrelloRemarkSmartAsync(TrelloRemark remark);

    public Task MoveToTrelloStateAsync(TrelloState state);

    public Task<TrelloRemark?> GetLastTrelloRemarkAsync();

    public Task<TrelloRemark?> GetLastTrelloRemarkSmartAsync();
}