using Autofac;
using Quartz;

namespace PaymentManagement.Infrastructure.Configuration.Quartz;

public class QuartzModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(ThisAssembly)
            .Where(type => typeof(IJob).IsAssignableFrom(type))
            .InstancePerDependency();
    }
}