namespace ConcordiaDBLibrary.Models.Abstract;

public abstract class Entity : IEquatable<Entity>
{
    public int? Id { get; set; } = null;

	public Entity(int? id = default)
	{
		Id = id;
	}

    public Entity()
     : this(null)
    { }

    public override string ToString()
    {
        return $"{nameof(Entity.Id)}:{Id}";
    }

    public virtual bool Equals(Entity? other)
    {
        if (other is null || !(other is Entity))
        {
            return false;
        }
        if (GetType() != other.GetType())
        {
            return false;
        }
        if (Id is null || other.Id is null)
        {
            return false;
        }
        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id is null ? base.GetHashCode() : Id.GetHashCode();
    }
}