using BuildingBlock.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.TypedIdConverters;

public class TypedIdValueConverter<TTypedIdValue> : ValueConverter<TTypedIdValue, Guid>
    where TTypedIdValue : TypedIdValueBase
{
    public TypedIdValueConverter(ConverterMappingHints? mappingHints = null)
        : base(id => id.Value, value => Create(value), mappingHints)
    {
    }

    private static TTypedIdValue Create(Guid id)
    {
        var result = Activator.CreateInstance(typeof(TTypedIdValue), id);
        
        if (result is not TTypedIdValue typedIdValue)
            throw new InvalidOperationException($"Cannot create instance of {typeof(TTypedIdValue)} from Guid.");
        
        return typedIdValue;
    }
}