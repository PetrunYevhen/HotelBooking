using System.Reflection;
using Users.Application.Contracts;

namespace Users.Infrastructure.Configuration;

public class Assemblies
{
    public static readonly Assembly Application = typeof(IUsersModule).Assembly;
}