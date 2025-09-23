using System.Reflection;
using RoomManagment.Application.Contracts;

namespace RoomManagment.Infrastructure.Configuration;

public class Assemblies
{
    public static readonly Assembly Application = typeof(IRoomManagementModule).Assembly;
}