namespace ConcordiaTrelloLibrary.Gateways.Abstract.ITrelloGateways;

using Models.Classes;
using Gateways.Abstract;

public interface ITrelloDatabaseGateway : ITrelloObjectGateway<TrelloDatabase>
{
    public Task InitTrelloObjectAsync(string code);

    public Task<IEnumerable<TrelloState>> GetTrelloStatesAsync();

    public Task<IEnumerable<TrelloPriority>> GetTrelloPrioritiesAsync();

    public Task<IEnumerable<TrelloScientist>> GetTrelloScientistsAsync();

    public IEnumerable<TrelloScientist> GetTrelloScientistsSmart();
}