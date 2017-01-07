using Autofac;

namespace Home.Bills.Client
{
    public class AutofacServiceClientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MartenServiceClient>().As<IServiceClient>().InstancePerLifetimeScope();
        }
    }
}