using System;
using System.Reflection;
using Autofac;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate.DataProviders;
using Home.Bills.Domain.AddressAggregate.Entities;
using Home.Bills.Infrastructure;
using Home.Bills.Payments.Domain;
using Marten;
using Marten.Services;
using Newtonsoft.Json.Serialization;
using Module = Autofac.Module;

namespace Home.Bills
{
    public class ContainerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GenericMartenRepository<Address>>()
                .As<IRepository<Address, Guid>>().InstancePerLifetimeScope();
            builder.RegisterType<AddressDataProvider>().As<IAddressDataProvider>().InstancePerLifetimeScope();

            builder.RegisterType<PaymentsDataProvider>().As<IPaymentsDataProvider>().InstancePerLifetimeScope();

            builder.Register(context =>
                {
                    return context.Resolve<IDocumentStore>().OpenSession();
                })
                .As<IDocumentSession>()
                .InstancePerLifetimeScope();

            builder.Register(context => DocumentStoreFactory.Create()).As<IDocumentStore>().SingleInstance();
        }
    }

    public class DocumentStoreFactory
    {
        public static IDocumentStore Create()
        {
            return DocumentStore
                .For(_ =>
                {
                    _.Connection("host=dev-machine;database=home_bills;password=admin;username=postgres");

                    var serializer = new JsonNetSerializer();

                    var dcr = new DefaultContractResolver();

                    dcr.DefaultMembersSearchFlags |= BindingFlags.NonPublic;

                    serializer.Customize(i =>
                    {
                        i.ContractResolver = dcr;
                    });
                    _.Serializer(serializer);
                });
        }
    }
}