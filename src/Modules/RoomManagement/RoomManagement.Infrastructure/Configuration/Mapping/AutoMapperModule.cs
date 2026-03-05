using Autofac;
using AutoMapper;
using RoomManagement.Application.Mapping;
using Module = Autofac.Module;

namespace RoomManagement.Infrastructure.Configuration.Mapping;

public class AutoMapperModule : Module
{ 
    // private readonly Assembly[] _assemblies;
    //
    // public AutoMapperModule(params Assembly[] assemblies)
    // {
    //     _assemblies = assemblies;
    // }

    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(ctx =>
            {
                // Створюємо конфігурацію AutoMapper
                var config = new MapperConfiguration(cfg =>
                {
                    // Додаємо профіль HotelModule
                    cfg.AddProfile<RoomProfile>();
                });

                return config.CreateMapper();
            })
            .As<IMapper>()            // Реєструємо як IMapper
            .SingleInstance();        // Синглтон на модуль
    }
}