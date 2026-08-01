using System.Data;
using Accommodations.Domain.Entities.HotelAddOns;
using Dapper;

namespace Accommodations.Infrastructure.Dapper;

public sealed class HotelAddOnIdTypeHandler : SqlMapper.TypeHandler<HotelAddOnId>
{
    public override void SetValue(IDbDataParameter parameter, HotelAddOnId? value) => parameter.Value = value?.Value;
    public override HotelAddOnId? Parse(object value) => new((Guid)value);
}
