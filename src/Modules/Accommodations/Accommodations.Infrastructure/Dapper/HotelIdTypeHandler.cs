using System.Data;
using Accommodations.Domain.Entities.Hotels;
using Dapper;

namespace Accommodations.Infrastructure.Dapper;

public class HotelIdTypeHandler : SqlMapper.TypeHandler<HotelId>
{
    public override void SetValue(IDbDataParameter parameter, HotelId? value)
        => parameter.Value = value?.Value;

    public override HotelId? Parse(object value)
        => new HotelId((Guid)value);
}