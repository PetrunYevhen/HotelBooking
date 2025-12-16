using Autofac;
using HotelManagement.Application.CachingContract;
using Microsoft.Extensions.Caching.Memory;

namespace HotelManagement.Infastructure.Configuration.Caching;

public class CachingModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register<IMemoryCache>(_ => new MemoryCache(new MemoryCacheOptions()))
            .SingleInstance();
        
        builder.RegisterType<HotelRoomsCache>()
            .As<IHotelRoomsCache>()
            .InstancePerLifetimeScope();
    }
}