using Autofac;
using Infrastructure.Client;

namespace Rooms.Infrastructure.Configuration.Client;

public class ClientModule : Module
{
    private readonly IClient _client;
    public ClientModule(IClient client)
    {
        _client = client;
    }
    
    protected override void Load(ContainerBuilder builder)
    {
        if (_client != null)
        {
            builder.RegisterInstance(_client);
        }
        
        builder.RegisterType<SubscriberManager>()
            .AsSelf()
            .As<IStartable>()
            .AutoActivate()
            .SingleInstance();
        
        builder.RegisterAssemblyTypes(ThisAssembly)
            .AsImplementedInterfaces()
            .Where(type => type.IsAssignableTo<ISubscription>())
            .SingleInstance();
    }
}