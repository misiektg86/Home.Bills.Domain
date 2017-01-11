using System;
using Autofac;
using Frameworks.Light.Ddd;
using Home.Bills.Payments.Acl;
using Home.Bills.Payments.DataAccess;
using Home.Bills.Payments.Domain.AddressAggregate;
using Home.Bills.Payments.Domain.Handlers;
using Marten;
using MassTransit;

namespace Home.Bills.Payments.Domain.Tests
{
    public class InfrastructureFixture : IDisposable
    {
        internal MartenDatabaseFixture MartenDatabaseFixture { get; }

        internal IContainer AutofacContainer { get; }

        internal IBusControl Bus { get; }

        public InfrastructureFixture()
        {
            MartenDatabaseFixture = new MartenDatabaseFixture();

            var builder = new ContainerBuilder();

            builder.Register(context => MartenDatabaseFixture.DocumentStore).As<IDocumentStore>().SingleInstance();

            builder.Register(context => context.Resolve<IDocumentStore>().OpenSession())
                .As<IDocumentSession>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AddressFactory>()
                .As<Frameworks.Light.Ddd.IAggregateFactory<Address, AddressFactoryInput, Guid>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GenericMartenRepository<Address>>()
                .As<IRepository<Address, Guid>>().InstancePerLifetimeScope();

            builder.RegisterType<PaymentsDataProvider>().As<IPaymentsDataProvider>().InstancePerLifetimeScope();

            builder.RegisterConsumers(typeof(AddressCreatedConsumer).Assembly,typeof(CreateAddressHandler).Assembly);
            builder.Register(context =>
                {
                    return MassTransit.Bus.Factory.CreateUsingInMemory(configurator =>
                    {
                        configurator.ReceiveEndpoint("Home.Bills.Payments", endpointConfigurator =>
                        {
                            endpointConfigurator.LoadFrom(context);
                        });
                    });
                }).SingleInstance()
                .As<IBusControl>()
                .As<IBus>();

            AutofacContainer = builder.Build();

            Bus = AutofacContainer.Resolve<IBusControl>();

            Bus.Start();
        }

        public void Dispose()
        {
            Bus?.Stop();
            AutofacContainer?.Dispose();
            MartenDatabaseFixture?.Dispose();
        }
    }
}