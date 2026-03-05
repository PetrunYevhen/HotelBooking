using System.Data;
using BookingManagement.Domain.Entities;
using Dapper;

namespace BookingManagement.Infrastructure.Dapper;

public class BookingIdTypeHandler : SqlMapper.TypeHandler<BookingId>
{
    public override void SetValue(IDbDataParameter parameter, BookingId? value) 
        => parameter.Value = value?.Value;

    public override BookingId? Parse(object value)
    => new BookingId((Guid)value);
}