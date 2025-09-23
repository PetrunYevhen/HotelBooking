using System.Data;
using Dapper;
using RoomManagment.Domain.Entities;

namespace RoomManagment.Infrastructure.Dapper;

public class RoomIdTypeHandler : SqlMapper.TypeHandler<RoomId>
{
    public override void SetValue(IDbDataParameter parameter, RoomId? value)
    => parameter.Value = value?.Value;

    public override RoomId? Parse(object value)
    => new RoomId((Guid)value);
}