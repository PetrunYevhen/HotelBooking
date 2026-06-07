using System.Reflection;
using Hotels.Application.Contracts;

namespace Hotels.Infastructure.Configuration;

public class Assemblies
{
    public static readonly Assembly Application = typeof(IHotelsModule).Assembly;
}