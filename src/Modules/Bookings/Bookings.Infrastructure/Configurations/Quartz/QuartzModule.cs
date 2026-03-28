using Autofac;
using Quartz;

namespace Bookings.Infrastructure.Configurations.Quartz;

public class QuartzModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(ThisAssembly)
            .Where(x => typeof(IJob).IsAssignableFrom(x))
            .InstancePerDependency();
    }
}