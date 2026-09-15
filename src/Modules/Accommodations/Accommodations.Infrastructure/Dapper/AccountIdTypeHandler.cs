using System.Data;
using Dapper;
using SharedKernel.Contracts;

namespace Accommodations.Infrastructure.Dapper;

public sealed class AccountIdTypeHandler : SqlMapper.TypeHandler<AccountId>
{
    public override void SetValue(IDbDataParameter parameter, AccountId? value) => parameter.Value = value?.Value;
    public override AccountId Parse(object value) => new((Guid)value);
}
