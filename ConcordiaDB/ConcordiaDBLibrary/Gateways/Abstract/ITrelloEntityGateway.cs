namespace ConcordiaDBLibrary.Gateways.Abstract;

using Models.Abstract;

public interface ITrelloEntityGateway<TTrelloEntity> : IEntityGateway<TTrelloEntity> where TTrelloEntity : TrelloEntity
{
    public TTrelloEntity? GetByCode(string? code);
    public IEnumerable<TTrelloEntity>? GetByCodeMulti(IEnumerable<string>? codes);

    public TTrelloEntity InsertByCode(TTrelloEntity tentity);
    public IEnumerable<TTrelloEntity>? InsertMultiByCode(IEnumerable<TTrelloEntity>? tentities);

    public TTrelloEntity UpdateByCode(TTrelloEntity tentity);
    public IEnumerable<TTrelloEntity>? UpdateMultiByCode(IEnumerable<TTrelloEntity>? tentities);

    public TTrelloEntity DeleteByCode(string? code);
    public IEnumerable<TTrelloEntity>? DeleteMulti(IEnumerable<string>? codes);
}