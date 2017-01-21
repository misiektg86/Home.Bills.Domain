using System;
using Autofac;
using Autofac.Core;
using Autofac.Core.Lifetime;
using Automatonymous;
using Frameworks.Light.Ddd;
using GreenPipes;
using GreenPipes.Policies;
using GreenPipes.Policies.ExceptionFilters;
using Home.Bills.DataAccess;
using Home.Bills.Domain.AddressAggregate;
using Home.Bills.Domain.AddressAggregate.Events;
using Home.Bills.Domain.MeterAggregate;
using Home.Bills.Domain.MeterReadAggregate;
using Home.Bills.Domain.Services;
using Marten;
using MassTransit;
using MassTransit.MartenIntegration;
using MassTransit.Saga;
using Module = Autofac.Module;

namespace Home.Bills
{
    public class ContainerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region Repositories

            builder.RegisterGeneric(typeof(GenericMartenRepository<>))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

            #endregion

            #region Factories

            builder.RegisterType<AddressFactory>()
                .As<Frameworks.Light.Ddd.IAggregateFactory<Address, AddressFactoryInput, Guid>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MeterFactory>()
               .As<Frameworks.Light.Ddd.IAggregateFactory<Meter, MeterFactoryInput, Guid>>()
               .InstancePerLifetimeScope();

            builder.RegisterType<MeterReadFactory>()
             .As<Frameworks.Light.Ddd.IAggregateFactory<MeterRead, MeterReadFactoryInput, Guid>>()
             .InstancePerLifetimeScope();

            #endregion

            #region DataProviders

            builder.RegisterType<AddressDataProvider>().As<IAddressDataProvider>().InstancePerLifetimeScope();
            builder.RegisterType<MeterDataProvider>().As<IMeterDataProvider>().InstancePerLifetimeScope();
            builder.RegisterType<UsageDataProvider>().As<IUsageDataProvider>().InstancePerLifetimeScope();
            builder.RegisterType<MeterReadDataProvider>().As<IMeterReadDataProvider>().InstancePerLifetimeScope();

            #endregion

            #region DomainServices

            builder.RegisterType<UsageDomainService>().AsSelf().InstancePerLifetimeScope();

            #endregion

            #region Marten

            builder.Register(context => context.Resolve<IDocumentStore>().OpenSession(DocumentTracking.None))
                .As<IDocumentSession>()
                .InstancePerLifetimeScope();
            builder.Register(context => DocumentStoreFactory.Create(context.Resolve<IComponentContext>())).As<IDocumentStore>().SingleInstance();
            #endregion

            #region MassTransit

            builder.RegisterStateMachineSagas(typeof(MeterMountedAtAddress).Assembly);
            builder.RegisterConsumers(typeof(MeterMountedAtAddress).Assembly);
            builder.RegisterGeneric(typeof(MartenSagaRepository<>)).As(typeof(ISagaRepository<>));
            builder.Register(context =>
            {
                return Bus.Factory.CreateUsingRabbitMq(configurator =>
                {
                    var host = configurator.Host(new Uri("rabbitmq://dev-machine:5672/test"),
                        hostConfigurator =>
                        {
                            hostConfigurator.Username("home");
                            hostConfigurator.Password("bills");
                        });

                    configurator.UseRetry(new IncrementalRetryPolicy(new AllExceptionFilter(), 10, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(200)));

                    configurator.ReceiveEndpoint(host, "Home.Bills", endpointConfigurator =>
                     {
                         endpointConfigurator.LoadFrom(context);
                         endpointConfigurator.LoadStateMachineSagas(context);
                     });
                });
            }).SingleInstance()
                .As<IBusControl>()
                .As<IBus>();

            #endregion

            #region Framework
           
            builder.RegisterType<AsyncUnitOfWork<Guid>>().As<IAsyncUnitOfWork>().InstancePerLifetimeScope();

            #endregion
        }
    }
}