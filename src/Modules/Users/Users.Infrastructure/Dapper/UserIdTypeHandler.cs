using System.Data;
using Dapper;
using Users.Domain.Entities;

namespace Users.Infrastructure.Dapper;

public class UserIdTypeHandler  : SqlMapper.TypeHandler<UserId>
{
    public override void SetValue(IDbDataParameter parameter, UserId? value)
        => parameter.Value = value?.Value;

    public override UserId? Parse(object value)
        => new UserId((Guid)value);
}