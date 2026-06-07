using Autofac;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.TypedIdConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;

namespace Users.Infrastructure.Configuration.DataAccess;

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
                var dbContextOptionBuilder = new DbContextOptionsBuilder<UsersDbContext>();
                
                dbContextOptionBuilder
                    // .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>()
                    .UseNpgsql(_databaseConnectionString);

                dbContextOptionBuilder
                    .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();

                return new UsersDbContext(dbContextOptionBuilder.Options, _loggerFactory);
            })
            .AsSelf()
            .As<DbContext>()
            .InstancePerLifetimeScope();
        
        var infrastructureAssembly = typeof(UsersDbContext).Assembly;

        builder.RegisterAssemblyTypes(infrastructureAssembly)
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .FindConstructorsWith(new AllConstructorFinder());
    }
    
}
