using System;
using Autofac;
using Frameworks.Light.Ddd;
using GreenPipes;
using GreenPipes.Policies;
using GreenPipes.Policies.ExceptionFilters;
using Home.Bills.Client;
using Home.Bills.Payments.Acl;
using Home.Bills.Payments.DataAccess;
using Home.Bills.Payments.DataAccess.Dtos;
using Home.Bills.Payments.Domain.Services;
using Marten;
using MassTransit;
using MassTransit.MartenIntegration;
using MassTransit.Saga;
using AddressAddedConsumer = Home.Bills.Payments.Domain.Consumers.AddressAddedConsumer;
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

            builder.RegisterType<Domain.AddressAggregate.AddressFactory>()
            .As<IAggregateFactory<Domain.AddressAggregate.Address, Domain.AddressAggregate.AddressFactoryInput, Guid>>()
            .InstancePerLifetimeScope();

            builder.RegisterType<Domain.PaymentAggregate.PaymentFactory>()
           .As<IAggregateFactory<Domain.PaymentAggregate.Payment, Domain.PaymentAggregate.PaymentFactoryInput, Guid>>()
           .InstancePerLifetimeScope();

            builder.RegisterType<Domain.TariffAggregate.TariffFactory>()
          .As<IAggregateFactory<Domain.TariffAggregate.Tariff, Domain.TariffAggregate.TariffFactoryInput, Guid>>()
          .InstancePerLifetimeScope();

            builder.RegisterType<Domain.RegistratorAgregate.RegistratorFactory>()
             .As<IAggregateFactory<Domain.RegistratorAgregate.Registrator, Domain.RegistratorAgregate.FactoryInput, Guid>>()
             .InstancePerLifetimeScope();

            #endregion

            #region DataProviders

            builder.RegisterType<PaymentsDataProvider>().As<IPaymentsDataProvider>().InstancePerLifetimeScope();

            builder.RegisterType<RegistratorDataProvider>().As<IRegistratorDataProvider>().InstancePerLifetimeScope();

            builder.RegisterModule<AutofacServiceClientModule>();

            builder.RegisterType<TariffDataProvider>().As<ITariffDataProvider>().InstancePerLifetimeScope();

            #endregion

            #region DomainServices

            builder.RegisterType<PaymentDomainService>().AsSelf().InstancePerLifetimeScope();

            #endregion

            #region Marten

            builder.Register(context => context.Resolve<IDocumentStore>().OpenSession())
                .As<IDocumentSession>()
                .InstancePerLifetimeScope();
            builder.Register(context => DocumentStoreFactory.Create(context.Resolve<IComponentContext>())).As<IDocumentStore>().SingleInstance();

            #endregion

            #region MassTransit

            builder.RegisterStateMachineSagas(typeof(AddressAddedConsumer).Assembly);
            builder.RegisterConsumers(typeof(AddressAddedConsumer).Assembly,
                typeof(MeterReadProcessFinishedConsumer).Assembly);
            builder.RegisterGeneric(typeof(MartenSagaRepository<>)).As(typeof(ISagaRepository<>));
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

                    configurator.UseRetry(new IncrementalRetryPolicy(new AllExceptionFilter(), 10, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(200)));

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