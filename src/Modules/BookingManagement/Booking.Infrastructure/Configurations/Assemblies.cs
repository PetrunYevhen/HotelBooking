using System.Reflection;
using BookingManagement.Application.Contracts;

namespace BookingManagement.Infrastructure.Configurations;

public class Assemblies
{
    public static readonly Assembly Application = typeof(IBookingManagementModule).Assembly;
}