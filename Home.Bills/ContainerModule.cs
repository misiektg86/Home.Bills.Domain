using System;
using System.Reflection;
using Autofac;
using Frameworks.Light.Ddd;
using GreenPipes.Policies;
using GreenPipes.Policies.ExceptionFilters;
using Home.Bills.DataAccess;
using Home.Bills.Domain.AddressAggregate;
using Home.Bills.Domain.AddressAggregate.DataProviders;
using Home.Bills.Domain.AddressAggregate.Events;
using Home.Bills.Domain.MeterAggregate;
using Home.Bills.Domain.MeterReadAggregate;
using Home.Bills.Payments.Acl;
using Home.Bills.Payments.Domain;
using Home.Bills.Payments.Domain.Handlers;
using Marten;
using Marten.Services;
using MassTransit;
using Newtonsoft.Json.Serialization;
using Module = Autofac.Module;

namespace Home.Bills
{
    public class ContainerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(GenericMartenRepository<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<AddressFactory>()
                .As<Frameworks.Light.Ddd.IAggregateFactory<Address, AddressFactoryInput, Guid>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MeterFactory>()
               .As<Frameworks.Light.Ddd.IAggregateFactory<Meter, MeterFactoryInput, Guid>>()
               .InstancePerLifetimeScope();

            builder.RegisterType<MeterReadFactory>()
             .As<Frameworks.Light.Ddd.IAggregateFactory<MeterRead, MeterReadFactoryInput, Guid>>()
             .InstancePerLifetimeScope();

            builder.RegisterType<AddressDataProvider>().As<IAddressDataProvider>().InstancePerLifetimeScope();

            builder.RegisterType<PaymentsDataProvider>().As<IPaymentsDataProvider>().InstancePerLifetimeScope();
            builder.RegisterType<MeterDataProvider>().As<IMeterDataProvider>().InstancePerLifetimeScope();


            builder.Register(context => context.Resolve<IDocumentStore>().OpenSession())
                .As<IDocumentSession>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UsageDataProvider>().As<IUsageDataProvider>().InstancePerLifetimeScope();

            builder.Register(context => DocumentStoreFactory.Create()).As<IDocumentStore>().SingleInstance();

            builder.RegisterAssemblyTypes(typeof(Payments.Domain.AddressAggregate.Address).GetTypeInfo().Assembly)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterConsumers(typeof(AddressCreatedConsumer).Assembly, typeof(CreateAddressHandler).Assembly, typeof(MeterMountedAtAddress).Assembly);

            builder.Register(context =>
                {
                    return Bus.Factory.CreateUsingInMemory(configurator =>
                    {
                        configurator.UseRetry(new IncrementalRetryPolicy(new AllExceptionFilter(), 10,TimeSpan.FromMilliseconds(100),TimeSpan.FromMilliseconds(100) ));
                        configurator.ReceiveEndpoint("Home.Bills", endpointConfigurator =>
                        {
                            endpointConfigurator.LoadFrom(context);
                        });
                    });
                }).SingleInstance()
                .As<IBusControl>()
                .As<IBus>();
        }
    }

    public class DocumentStoreFactory
    {
        public static IDocumentStore Create()
        {
            return DocumentStore
                .For(_ =>
                {
                    _.MappingFor(typeof(Payments.Domain.AddressAggregate.Address)).DatabaseSchemaName = "Bills_Payments";
                    _.MappingFor(typeof(Payment)).DatabaseSchemaName = "Bills_Payments";
                    _.Schema.For<Payments.Domain.AddressAggregate.Address>().DocumentAlias("Bills_Payments_Address");
                    _.Connection("host=dev-machine;database=home_bills;password=admin;username=postgres");

                    var serializer = new JsonNetSerializer();

                    var dcr = new DefaultContractResolver();

                    dcr.DefaultMembersSearchFlags |= BindingFlags.NonPublic | BindingFlags.Instance;

                    serializer.Customize(i =>
                    {
                        i.ContractResolver = dcr;
                    });
                    _.Serializer(serializer);
                });
        }
    }
}