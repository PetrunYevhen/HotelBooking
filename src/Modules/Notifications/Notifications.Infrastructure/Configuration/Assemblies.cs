using System.Reflection;
using Notifications.Application.Contracts;

namespace Notifications.Infrastructure.Configuration;

public class Assemblies
{
    public static readonly Assembly Application = typeof(INotificationsModule).Assembly;
}
