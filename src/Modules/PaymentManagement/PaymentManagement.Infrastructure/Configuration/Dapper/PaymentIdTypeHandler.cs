using System.Data;
using Dapper;
using PaymentManagement.Domain.Entities;

namespace PaymantManagement.Infrastructure.Configuration.Dapper;

public class PaymentIdTypeHandler  : SqlMapper.TypeHandler<PaymentId>
{
    public override void SetValue(IDbDataParameter parameter, PaymentId? value)
        => parameter.Value = value?.Value;

    public override PaymentId? Parse(object value)
        => new PaymentId((Guid)value);
}