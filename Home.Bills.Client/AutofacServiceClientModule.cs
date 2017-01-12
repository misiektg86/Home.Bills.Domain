using Autofac;
using Home.Bills.DataAccess;

namespace Home.Bills.Client
{
    public class AutofacServiceClientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MartenServiceClient>().As<IServiceClient>().InstancePerLifetimeScope();
            builder.RegisterType<AddressDataProvider>().As<IAddressDataProvider>().InstancePerLifetimeScope();
            builder.RegisterType<MeterDataProvider>().As<IMeterDataProvider>().InstancePerLifetimeScope();
            builder.RegisterType<UsageDataProvider>().As<IUsageDataProvider>().InstancePerLifetimeScope();
            builder.RegisterType<MeterReadDataProvider>().As<IMeterReadDataProvider>().InstancePerLifetimeScope();
        }
    }
}