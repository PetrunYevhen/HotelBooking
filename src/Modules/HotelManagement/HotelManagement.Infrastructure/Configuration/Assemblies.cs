using System.Reflection;
using HotelManagement.Application.Contracts;

namespace HotelManagement.Infastructure.Configuration;

public class Assemblies
{
    public static readonly Assembly Application = typeof(IHotelManagementModule).Assembly;
}