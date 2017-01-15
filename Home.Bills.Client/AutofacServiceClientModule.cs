using Autofac;
using Autofac.Core;
using Home.Bills.DataAccess;
using Marten;

namespace Home.Bills.Client
{
    public class AutofacServiceClientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MartenServiceClient>().WithParameter(ResolvedParameter.ForNamed<IDocumentSession>("bills.client")).As<IServiceClient>().InstancePerLifetimeScope();
            builder.RegisterType<AddressDataProvider>().WithParameter(ResolvedParameter.ForNamed<IDocumentSession>("bills.client")).As<IAddressDataProvider>().InstancePerLifetimeScope();
            builder.RegisterType<MeterDataProvider>().WithParameter(ResolvedParameter.ForNamed<IDocumentSession>("bills.client")).As<IMeterDataProvider>().InstancePerLifetimeScope();
            builder.RegisterType<UsageDataProvider>().WithParameter(ResolvedParameter.ForNamed<IDocumentSession>("bills.client")).As<IUsageDataProvider>().InstancePerLifetimeScope();
            builder.RegisterType<MeterReadDataProvider>().WithParameter(ResolvedParameter.ForNamed<IDocumentSession>("bills.client")).As<IMeterReadDataProvider>().InstancePerLifetimeScope();
            builder.Register(DocumentStoreFactory.Create).Named<IDocumentStore>("bills.client").SingleInstance();
            builder.Register(context => context.ResolveNamed<IDocumentStore>("bills.client").OpenSession()).Named<IDocumentSession>("bills.client").InstancePerLifetimeScope();
        }
    }
}