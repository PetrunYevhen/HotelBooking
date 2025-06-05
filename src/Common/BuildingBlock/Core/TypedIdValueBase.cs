namespace BuildingBlock.Domain;

// Class for creating strongly typed identifiers based on GUIDs
public abstract class TypedIdValueBase : IEquatable<TypedIdValueBase>
{
    public Guid Value { get; }
    
    protected TypedIdValueBase(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Value cannot be an empty GUID.", nameof(value));
        Value = value;
    }
    
    public bool Equals(TypedIdValueBase? other)
    {
        return this.Value == other?.Value;
    }
    
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }
        return obj is TypedIdValueBase other && Equals(other);
    }
    
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
    
    public static bool operator ==(TypedIdValueBase? obj1, TypedIdValueBase? obj2)
    {
        if (Equals(obj1, null))
        {
            if (Equals(obj2, null))
            {
                return true;
            }
            return false;
        }
         return obj1.Equals(obj2);       
         
    }
    public static bool operator !=(TypedIdValueBase x, TypedIdValueBase y)
    {
        return !(x == y);
    }
}