using System.Reflection;
using RoomManagement.Application.Contracts;

namespace RoomManagement.Infrastructure.Configuration;

public class Assemblies
{
    public static readonly Assembly Application = typeof(IRoomManagementModule).Assembly;
}