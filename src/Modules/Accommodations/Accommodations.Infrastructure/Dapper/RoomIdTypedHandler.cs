using System.Data;
using Accommodations.Domain.Entities.Rooms;
using Dapper;

namespace Accommodations.Infrastructure.Dapper;

public class RoomIdTypedHandler : SqlMapper.TypeHandler<RoomId>
{
    public override void SetValue(IDbDataParameter parameter, RoomId? value)
        => parameter.Value = value?.Value;

    public override RoomId? Parse(object value)
        => new RoomId((Guid)value);
}