using System.Reflection;
using PaymantManagement.Application.Contracts;

namespace PaymantManagement.Infrastructure.Configuration;

public class Assemblies
{
    public static readonly Assembly Application = typeof(IPaymentManagementModule).Assembly;
}