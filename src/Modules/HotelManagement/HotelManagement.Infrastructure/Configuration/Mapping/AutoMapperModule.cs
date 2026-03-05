using Autofac;
using AutoMapper;
using HotelManagement.Application.Mapping;

namespace HotelManagement.Infastructure.Configuration.Mapping
{
    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(ctx =>
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.AddMaps(typeof(HotelProfile).Assembly);
                    });

                    return config.CreateMapper();
                })
                .As<IMapper>()            
                .SingleInstance();        
        }
    }
}