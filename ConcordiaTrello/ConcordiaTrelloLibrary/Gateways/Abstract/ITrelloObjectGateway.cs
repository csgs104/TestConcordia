namespace ConcordiaTrelloLibrary.Gateways.Abstract;

using Models.Abstract;

public interface ITrelloObjectGateway<TObject> where TObject : TrelloObject
{
    public Task<bool> Create();

    public Task<bool> Update();

    public Task<bool> Delete();
}