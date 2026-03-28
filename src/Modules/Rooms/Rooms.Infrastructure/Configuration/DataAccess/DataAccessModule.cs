using Autofac;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.TypedIdConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;

namespace Rooms.Infrastructure.Configuration.DataAccess;

public class DataAccessModule : Module
{
    private readonly string _databaseConnectionString;
    private readonly ILoggerFactory _loggerFactory;


    public DataAccessModule(string databaseConnectionString, ILoggerFactory loggerFactory)
    {
        _databaseConnectionString = databaseConnectionString;
        _loggerFactory = loggerFactory;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<NpgsqlConnectionFactory>()
            .As<INpgsqlConnectionFactory>()
            .WithParameter("connectionString", _databaseConnectionString)
            .InstancePerLifetimeScope();

        builder.Register(c =>
        {
            var dbContextOptionBuilder = new DbContextOptionsBuilder<RoomsDbContext>();
            dbContextOptionBuilder.UseNpgsql(_databaseConnectionString);

            dbContextOptionBuilder
                .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();

            return new RoomsDbContext(dbContextOptionBuilder.Options, _loggerFactory);
        })
        .AsSelf()
        .As<DbContext>()
        .InstancePerLifetimeScope();
        
        var infrastructureAssembly = typeof(RoomsDbContext).Assembly;
        builder.RegisterAssemblyTypes(infrastructureAssembly)
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .FindConstructorsWith(new AllConstructorFinder());
    }
}