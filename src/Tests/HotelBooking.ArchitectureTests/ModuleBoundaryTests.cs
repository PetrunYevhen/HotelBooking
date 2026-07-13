using System.Reflection;
using Xunit;

namespace HotelBooking.ArchitectureTests;

public sealed class ModuleBoundaryTests
{
    private static readonly Assembly[] DomainAssemblies =
    [
        typeof(Accommodations.Domain.Entities.Hotels.Hotel).Assembly,
        typeof(Bookings.Domain.Entities.Booking).Assembly,
        typeof(Payments.Domain.Entities.Payment).Assembly,
        typeof(Users.Domain.Entities.User).Assembly,
        typeof(Reviews.Domain.Entities.Reviews.Review).Assembly,
        typeof(Notifications.Domain.Entities.Notification).Assembly
    ];

    [Fact]
    public void DomainLayers_DoNotDependOnApplicationOrInfrastructureLayers()
    {
        var violations = DomainAssemblies
            .SelectMany(assembly => assembly.GetReferencedAssemblies()
                .Where(reference => reference.Name?.EndsWith(".Application") == true ||
                                    reference.Name?.EndsWith(".Infrastructure") == true)
                .Select(reference => $"{assembly.GetName().Name} -> {reference.Name}"))
            .ToArray();

        Assert.True(violations.Length == 0,
            $"Domain dependency violations:{Environment.NewLine}{string.Join(Environment.NewLine, violations)}");
    }

    [Fact]
    public void DomainLayers_DoNotDependOnAnotherBusinessModule()
    {
        var moduleNames = new[]
        {
            "Accommodations", "Bookings", "Payments", "Users", "Reviews", "Notifications"
        };

        var violations = DomainAssemblies.SelectMany(assembly =>
        {
            var ownModule = assembly.GetName().Name!.Split('.')[0];
            return assembly.GetReferencedAssemblies()
                .Where(reference => moduleNames.Any(module => module != ownModule &&
                    reference.Name?.StartsWith(module + ".", StringComparison.Ordinal) == true))
                .Select(reference => $"{assembly.GetName().Name} -> {reference.Name}");
        }).ToArray();

        Assert.True(violations.Length == 0,
            $"Cross-module domain dependencies:{Environment.NewLine}{string.Join(Environment.NewLine, violations)}");
    }
}
