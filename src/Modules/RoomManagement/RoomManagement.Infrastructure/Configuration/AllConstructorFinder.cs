using System.Collections.Concurrent;
using System.Reflection;
using Autofac.Core.Activators.Reflection;

namespace RoomManagement.Infrastructure.Configuration;

public class AllConstructorFinder : IConstructorFinder
{
    private static readonly ConcurrentDictionary<Type, ConstructorInfo[]> Cache = new();

    public ConstructorInfo[] FindConstructors(Type targetType)
    {
        var result = Cache.GetOrAdd(
            targetType,
            t => t.GetTypeInfo().DeclaredConstructors.ToArray());

        return result.Length > 0 ? result : throw new NoConstructorsFoundException(targetType, this);
    }
}