using Autofac;
using AutoMapper;
using Hotels.Application.Mapping;

namespace Hotels.Infastructure.Configuration.Mapping
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