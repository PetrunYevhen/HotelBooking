using System.Data;
using Dapper;
using Accommodations.Domain.Entities.HotelierApplications;

namespace Accommodations.Infrastructure.Dapper;

public sealed class HotelierApplicationIdTypeHandler : SqlMapper.TypeHandler<HotelierApplicationId>
{
    public override void SetValue(IDbDataParameter parameter, HotelierApplicationId? value) => parameter.Value = value?.Value;
    public override HotelierApplicationId? Parse(object value) => new HotelierApplicationId((Guid)value);
}
