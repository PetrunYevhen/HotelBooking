using System.Reflection;
using Bookings.Application.Contracts;

namespace Bookings.Infrastructure.Configurations;

public class Assemblies
{
    public static readonly Assembly Application = typeof(IBookingsModule).Assembly;
}