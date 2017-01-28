using System;
using Autofac;
using Frameworks.Light.Ddd;
using GreenPipes;
using GreenPipes.Policies;
using GreenPipes.Policies.ExceptionFilters;
using Home.Bills.Notification.Infrastructure;
using Home.Bills.Notifications.Acl;
using Home.Bills.Notifications.Domain.AddressAggregate;
using Home.Bills.Notifications.Domain.Consumers.Saga;
using Home.Bills.Notifications.Domain.Services;
using Home.Bills.Payments.Client;
using Marten;
using MassTransit;
using MassTransit.MartenIntegration;
using MassTransit.Saga;
using Module = Autofac.Module;

namespace Home.Bills.Notifications
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

            #endregion

            #region DataProviders

            builder.RegisterModule<AutofacServiceClientModule>();

            builder.RegisterModule<Client.AutofacServiceClientModule>();

            #endregion

            #region DomainServices

            builder.RegisterType<PaymentNotificationDomainService>().AsSelf().InstancePerLifetimeScope();

            #endregion

            #region Marten

            builder.Register(context => context.Resolve<IDocumentStore>().OpenSession(DocumentTracking.None))
                .As<IDocumentSession>()
                .InstancePerLifetimeScope();
            builder.Register(context => DocumentStoreFactory.Create(context.Resolve<IComponentContext>())).As<IDocumentStore>().SingleInstance();
            #endregion

            #region MassTransit

            builder.RegisterStateMachineSagas(typeof(PaymentNotificationStateMachine).Assembly);
            builder.RegisterConsumers(typeof(PaymentAcceptedConsumer).Assembly, typeof(Address).Assembly, typeof(SmtpMailSenderConsumer).Assembly);
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

                    configurator.ReceiveEndpoint(host, "Home.Bills.Notifications", endpointConfigurator =>
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

            #region Infrastructure

            builder.RegisterType<SmtpAccountDataAccess>().AsSelf().InstancePerLifetimeScope();

            #endregion
        }
    }
}