using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Frameworks.Light.Ddd;
using Home.Bills.Domain.Messages;
using Home.Bills.Infrastructure;
using Home.Bills.Payments.Acl;
using Home.Bills.Payments.Domain.AddressAggregate;
using Marten;
using MassTransit;
using MediatR;
using Xunit;

namespace Home.Bills.Payments.Domain.Tests
{
    public class AclTests : IClassFixture<InfrastructureFixture>
    {
        private readonly InfrastructureFixture _infrastructureFixture;

        public AclTests(InfrastructureFixture infrastructureFixture)
        {
            _infrastructureFixture = infrastructureFixture;
        }

        [Fact]
        public async Task ShouldIAddressCreatedMessageCreateAddress()
        {
            var addressId = Guid.NewGuid();

            await _infrastructureFixture.Bus.Publish<IAddressCreated>(new { Id = addressId, SquareMeters = 50.00 });

            await Task.Delay(500);

            var addressRepository = _infrastructureFixture.AutofacContainer.Resolve<IRepository<Address, Guid>>();

            var address = await addressRepository.Get(addressId);

            Assert.NotNull(address);
        }
    }

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

            builder.RegisterConsumers(typeof(AddressCreatedConsumer).Assembly);

            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(Address).GetTypeInfo().Assembly).AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.Register<SingleInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.Register<MultiInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
            });

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