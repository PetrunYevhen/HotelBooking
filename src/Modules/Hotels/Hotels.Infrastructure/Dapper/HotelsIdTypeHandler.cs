using System.Data;
using Dapper;
using Hotels.Domain.Entities;

namespace Hotels.Infastructure.Dapper;

public class HotelsIdTypeHandler : SqlMapper.TypeHandler<HotelId>
{
    public override void SetValue(IDbDataParameter parameter, HotelId? value)
        => parameter.Value = value?.Value;

    public override HotelId? Parse(object value)
        => new HotelId((Guid)value);
}