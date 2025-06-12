using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoomManagment.Application.Command.AddRoom;
using RoomManagment.Application.Mapping;
using RoomManagment.Domain.RepositoryContract;
using RoomManagment.Infrastructure.Dapper;
using RoomManagment.Infrastructure.Data;
using RoomManagment.Infrastructure.Repositories;

namespace RoomManagment.Infrastructure.DI;

public static class RoomModuleServiceCollectionExtension
{
    public static IServiceCollection AddRoomModule(this IServiceCollection? services, IConfiguration configuration)
    {
        // Register repositories
        services.AddScoped<IRoomManagmentReadRepository, RoomManagmentReadRepository>();
        services.AddScoped<IRoomManagmentWriteRepository, RoomManagmentWriteRepository>();
        
        // Register MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AddRoomCommand>());
        
        // Register Dapper Type Handlers
        SqlMapper.AddTypeHandler(new RoomIdTypeHandler());
        
        //Register DbContext
        services.AddDbContext<RoomDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        // Register Automapper
        services.AddAutoMapper(typeof(RoomProfile).Assembly);
        
        return services;
    }
}