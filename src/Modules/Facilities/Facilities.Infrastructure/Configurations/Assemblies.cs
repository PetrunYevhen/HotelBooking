using System.Reflection;
using Facilities.Application.Contracts;

namespace Facilities.Infrastructure.Configurations;

public class Assemblies
{
    public static readonly Assembly Application = typeof(IFacilityModule).Assembly;
}