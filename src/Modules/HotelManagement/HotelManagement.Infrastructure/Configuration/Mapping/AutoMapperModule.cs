using Autofac;
using AutoMapper;
using HotelManagement.Application.Mapping;

namespace HotelManagement.Infastructure.Configuration.Mapping;

public class AutoMapperModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(ctx => new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(HotelProfile).Assembly);
            }))
            .AsSelf()
            .SingleInstance();
        
        builder.Register(ctx =>
        {
            var config = ctx.Resolve<MapperConfiguration>();
            config.AssertConfigurationIsValid(); // 🔥 тепер помилка буде ЧІТКА
            return config.CreateMapper();
        })
        .As<IMapper>()
        .InstancePerLifetimeScope();
    }
}