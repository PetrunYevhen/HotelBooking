using System.Data;
using Bookings.Domain.Entities;
using Dapper;

namespace Bookings.Infrastructure.Dapper;

public class BookingIdTypeHandler : SqlMapper.TypeHandler<BookingId>
{
    public override void SetValue(IDbDataParameter parameter, BookingId? value) 
        => parameter.Value = value?.Value;

    public override BookingId? Parse(object value)
    => new BookingId((Guid)value);
}