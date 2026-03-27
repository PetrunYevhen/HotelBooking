using System.Reflection;
using PaymentManagement.Application.Contracts;

namespace PaymentManagement.Infrastructure.Configuration;

public class Assemblies
{
    public static readonly Assembly Application = typeof(IPaymentManagementModule).Assembly;
}