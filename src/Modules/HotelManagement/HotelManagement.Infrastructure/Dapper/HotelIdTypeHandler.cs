using System.Data;
using Dapper;
using HotelManagement.Domain.Entities;

namespace HotelManagement.Infastructure.Dapper;

public class HotelIdTypeHandler : SqlMapper.TypeHandler<HotelId>
{
    public override void SetValue(IDbDataParameter parameter, HotelId? value)
        => parameter.Value = value?.Value;

    public override HotelId? Parse(object value)
        => new HotelId((Guid)value);
}