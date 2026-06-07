using System.Reflection;
using Accommodations.Application.Contracts;

namespace Accommodations.Infrastructure.Configuration;

public class Assemblies
{
    public static readonly Assembly Application = typeof(IAccommodationsModule).Assembly;
}