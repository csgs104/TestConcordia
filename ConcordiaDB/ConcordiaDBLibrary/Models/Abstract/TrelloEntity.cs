namespace ConcordiaDBLibrary.Models.Abstract;

public abstract class TrelloEntity : Entity
{
    public string? Code { get; set; } = null;

    public TrelloEntity(int? id = default, string? code = default) : base(id)
    {
        Code = code;
    }

    public TrelloEntity()
     : this(null, null)
    { }

    public override string ToString()
    {
        return $"{base.ToString()}, {nameof(TrelloEntity.Code)}:{Code}";
    }
}