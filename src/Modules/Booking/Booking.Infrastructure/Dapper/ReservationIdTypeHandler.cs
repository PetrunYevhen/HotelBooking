using System.Data;
using Booking.Domain.Entities;
using Dapper;

namespace Booking.Infrastructure.Dapper;

public class ReservationIdTypeHandler : SqlMapper.TypeHandler<ReservationId>
{
    public override void SetValue(IDbDataParameter parameter, ReservationId? value) 
        => parameter.Value = value?.Value;

    public override ReservationId? Parse(object value)
    => new ReservationId((Guid)value);
}