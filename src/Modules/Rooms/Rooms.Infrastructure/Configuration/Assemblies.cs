using System.Reflection;
using Rooms.Application.Contracts;

namespace Rooms.Infrastructure.Configuration;

public class Assemblies
{
    public static readonly Assembly Application = typeof(IRoomsModule).Assembly;
}