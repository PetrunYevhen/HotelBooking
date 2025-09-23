using System.Data;
using Dapper;
using Facilities.Domain.Entities;

namespace Facilities.Infrastructure.Dapper;

public class FacilityIdTypeHandler : SqlMapper.TypeHandler<FacilityTreeId>
{
    public override void SetValue(IDbDataParameter parameter, FacilityTreeId? value)
        => parameter.Value = value?.Value;

    public override FacilityTreeId? Parse(object value)
        => new FacilityTreeId((Guid)value);
}