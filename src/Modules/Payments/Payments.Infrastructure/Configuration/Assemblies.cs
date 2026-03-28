using System.Reflection;
using Payments.Application.Contracts;

namespace Payments.Infrastructure.Configuration;

public class Assemblies
{
    public static readonly Assembly Application = typeof(IPaymentsModule).Assembly;
}