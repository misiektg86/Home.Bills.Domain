using System;
using Autofac;
using Frameworks.Light.Ddd;
using GreenPipes.Policies;
using GreenPipes.Policies.ExceptionFilters;
using Home.Bills.Client;
using Home.Bills.Payments.DataAccess;
using Home.Bills.Payments.DataAccess.Dtos;
using Home.Bills.Payments.Domain;
using Home.Bills.Payments.Domain.Handlers;
using Home.Bills.Payments.Domain.Services;
using Marten;
using MassTransit;
using MassTransit.MartenIntegration;
using MassTransit.Saga;
using Module = Autofac.Module;

namespace Home.Bills.Payments
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

            builder.RegisterType<Payments.Domain.AddressAggregate.AddressFactory>()
            .As<IAggregateFactory<Payments.Domain.AddressAggregate.Address, Payments.Domain.AddressAggregate.AddressFactoryInput, Guid>>()
            .InstancePerLifetimeScope();

            builder.RegisterType<Payments.Domain.PaymentAggregate.PaymentFactory>()
           .As<IAggregateFactory<Payments.Domain.PaymentAggregate.Payment, Payments.Domain.PaymentAggregate.PaymentFactoryInput, Guid>>()
           .InstancePerLifetimeScope();

            builder.RegisterType<Payments.Domain.TariffAggregate.TariffFactory>()
          .As<IAggregateFactory<Payments.Domain.TariffAggregate.Tariff, Payments.Domain.TariffAggregate.TariffFactoryInput, Guid>>()
          .InstancePerLifetimeScope();

            #endregion

            #region DataProviders

            builder.RegisterType<PaymentsDataProvider>().As<IPaymentsDataProvider>().InstancePerLifetimeScope();

            builder.RegisterModule<AutofacServiceClientModule>();

            #endregion

            #region DomainServices

            builder.RegisterType<PaymentDomainService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<TariffDataProvider>().As<ITariffDataProvider>().InstancePerLifetimeScope();

            #endregion

            #region Marten

            builder.Register(context => context.Resolve<IDocumentStore>().OpenSession())
                .As<IDocumentSession>()
                .InstancePerLifetimeScope();
            builder.Register(DocumentStoreFactory.Create).As<IDocumentStore>().SingleInstance();

            #endregion

            #region MassTransit

            builder.RegisterStateMachineSagas(typeof(CreateAddressHandler).Assembly).InstancePerLifetimeScope();
            builder.RegisterConsumers(typeof(CreateAddressHandler).Assembly).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(MartenSagaRepository<>)).As(typeof(ISagaRepository<>)).InstancePerLifetimeScope();
            builder.Register(context =>
            {
                return Bus.Factory.CreateUsingRabbitMq(configurator =>
                {
                    var host = configurator.Host(new Uri("rabbitmq://dev-machine:5672/home_bills"),
                        hostConfigurator =>
                        {
                            hostConfigurator.Username("home");
                            hostConfigurator.Password("bills");
                        });

                    configurator.UseRetry(new IncrementalRetryPolicy(new AllExceptionFilter(), 10, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(100)));

                    configurator.ReceiveEndpoint(host, "Home.Bills.Payments", endpointConfigurator =>
                     {
                         endpointConfigurator.LoadFrom(context);
                         endpointConfigurator.LoadStateMachineSagas(context);
                     });
                });
            }).SingleInstance()
                .As<IBusControl>()
                .As<IBus>();

            #endregion
        }
    }
}