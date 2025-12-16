using System.Reflection;
using Autofac;
using AutoMapper;
using Module = Autofac.Module;

namespace RoomManagement.Infrastructure.Configuration.Mapping;

public class AutoMapperModule : Module
{ 
    private readonly Assembly[] _assemblies;

    public AutoMapperModule(params Assembly[] assemblies)
    {
        _assemblies = assemblies;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(context =>
            {
                var config = new MapperConfiguration(config =>
                {
                    config.AddMaps(_assemblies);
                });

                config.AssertConfigurationIsValid();

                return config.CreateMapper(context.Resolve);
            })
            .As<IMapper>()
            .SingleInstance();
    }
}