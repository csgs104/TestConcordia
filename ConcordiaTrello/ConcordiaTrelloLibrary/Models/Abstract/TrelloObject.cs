namespace ConcordiaTrelloLibrary.Models.Abstract;

public abstract class TrelloObject : IEquatable<TrelloObject>
{
    public string? Code { get; set; } = null;

    public TrelloObject(string? code = default)
    {
        Code = code;
    }

    public override string ToString()
    {
        return $"{nameof(TrelloObject.Code)}:{Code}";
    }

    public virtual bool Equals(TrelloObject? other)
    {
        if (other is null || !(other is TrelloObject))
        {
            return false;
        }
        if (GetType() != other.GetType())
        {
            return false;
        }
        if (Code is null || other.Code is null)
        {
            return false;
        }
        return Code == other.Code;
    }

    public override int GetHashCode()
    {
        return Code is null ? base.GetHashCode() : Code.GetHashCode();
    }
}