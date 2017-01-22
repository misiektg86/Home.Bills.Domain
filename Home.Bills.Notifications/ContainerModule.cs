using System;
using Autofac;
using Frameworks.Light.Ddd;
using GreenPipes;
using GreenPipes.Policies;
using GreenPipes.Policies.ExceptionFilters;
using Home.Bills.Notifications.Acl;
using Home.Bills.Notifications.Domain.AddressAggregate;
using Home.Bills.Payments.Client;
using Marten;
using MassTransit;
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
                .As<Frameworks.Light.Ddd.IAggregateFactory<Address, Guid, Guid>>()
                .InstancePerLifetimeScope();

            #endregion

            #region DataProviders

            builder.RegisterModule<AutofacServiceClientModule>();

            #endregion

            #region DomainServices

            //builder.RegisterType<UsageDomainService>().AsSelf().InstancePerLifetimeScope();

            #endregion

            #region Marten

            builder.Register(context => context.Resolve<IDocumentStore>().OpenSession(DocumentTracking.None))
                .As<IDocumentSession>()
                .InstancePerLifetimeScope();
            builder.Register(context => DocumentStoreFactory.Create(context.Resolve<IComponentContext>())).As<IDocumentStore>().SingleInstance();
            #endregion

            #region MassTransit

            //builder.RegisterStateMachineSagas(typeof(MeterMountedAtAddress).Assembly);
            builder.RegisterConsumers(typeof(PaymentAcceptedConsumer).Assembly, typeof(Address).Assembly);
           // builder.RegisterGeneric(typeof(MartenSagaRepository<>)).As(typeof(ISagaRepository<>));
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