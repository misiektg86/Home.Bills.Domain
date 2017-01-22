using Autofac;
using Autofac.Core;
using Home.Bills.Payments.DataAccess;
using Home.Bills.Payments.DataAccess.Dtos;
using Marten;

namespace Home.Bills.Payments.Client
{
    public class AutofacServiceClientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MartenServiceClient>().WithParameter(ResolvedParameter.ForNamed<IDocumentSession>("bills.payments.client")).As<IServiceClient>().InstancePerLifetimeScope();
            builder.RegisterType<PaymentsDataProvider>().WithParameter(ResolvedParameter.ForNamed<IDocumentSession>("bills.payments.client")).As<IPaymentsDataProvider>().InstancePerLifetimeScope();
            builder.Register(DocumentStoreFactory.Create).Named<IDocumentStore>("bills.payments.client").SingleInstance();
            builder.Register(context => context.ResolveNamed<IDocumentStore>("bills.payments.client").OpenSession()).Named<IDocumentSession>("bills.payments.client").InstancePerLifetimeScope();
        }
    }
}