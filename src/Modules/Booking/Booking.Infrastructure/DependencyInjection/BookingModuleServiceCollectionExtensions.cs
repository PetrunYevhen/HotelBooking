using Microsoft.Extensions.DependencyInjection;

namespace Booking.Infrastructure.DependencyInjection;

public static class BookingModuleServiceCollectionExtensions 
{
    public static IServiceCollection AddBookingModule(this IServiceCollection services)
    {
        // Register repositories
        services.AddScoped<Domain.RepositoryContracts.IReservationRepository, Infrastructure.Repositories.ReservationRepository>();

        // Register application commands and queries
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Application.Command.AddReservationCommand>());

        return services;
    }
}