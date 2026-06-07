namespace BuildingBlock.Domain;

public abstract class ValueObject
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if(obj is null)
            return false;
        
        if(GetType() != obj.GetType())
            return false;
        
        var valueObject =  (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
    }

    public override int GetHashCode()
    => GetEqualityComponents().Aggregate(default(int), (hashcode, value) =>
        HashCode.Combine(hashcode, value.GetHashCode()));
    

    public static bool operator ==(ValueObject? first, ValueObject? second)
    {
        if(first is null && second is null)
            return  true;
        
        if(first is null || second is null)
            return  true; 
        
        return first.Equals(second);
    }
    
    public static bool operator !=(ValueObject? first, ValueObject? second)
        => !(first == second);
    
}