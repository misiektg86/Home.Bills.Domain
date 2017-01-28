using System;
using Automatonymous;
using Home.Bills.Notifications.Messages;

namespace Home.Bills.Notifications.Domain.Consumers.Saga
{
    public class PaymentNotificationStateMachine : MassTransitStateMachine<PaymentNotificationStateMachineInstance>
    {
        public State CreatingNotification { get; set; }

        public State SendingNotification { get; set; }

        public Event<IPaymentAccepted> PaymentAccepted { get; set; }

        public Event<IPaymentCreated> NotificationCreated { get; set; }

        public Event<INotificationSent> NotificationSent { get; set; }

        public PaymentNotificationStateMachine()
        {
            InstanceState(instance => instance.State);

            Event(() => PaymentAccepted,
                configurator =>
                    configurator.CorrelateById(context => context.Message.PaymentId)
                        .SelectId(context => context.Message.PaymentId));

            Event(() => NotificationCreated,
                configurator => configurator.CorrelateById(context => context.Message.PaymentId));
            Event(() => NotificationSent,
               configurator => configurator.CorrelateById(context => context.Message.NotificationId));

            Initially(
                When(PaymentAccepted)
                    .Send((instance, data) => new Uri("rabbitmq://dev-machine:5672/test/Home.Bills.Notifications"),
                        context =>
                            new CreatePaymentNotification()
                            {
                                PaymentId = context.Instance.CorrelationId
                            }).TransitionTo(CreatingNotification));
            During(CreatingNotification,
                When(NotificationCreated)
                    .Send((instance, data) => new Uri("rabbitmq://dev-machine:5672/test/Home.Bills.Notifications"),
                        context =>
                            new SendPaymentNotification()
                            {
                                PaymentId = context.Instance.CorrelationId
                            }).TransitionTo(SendingNotification));
            During(SendingNotification,When(NotificationSent).Send((instance, data) => new Uri("rabbitmq://dev-machine:5672/test/Home.Bills.Notifications"),
                        context =>
                            new MarkPaymentNotificationAsSent()
                            {
                                PaymentId = context.Instance.CorrelationId
                            }).Finalize());

            SetCompletedWhenFinalized();
        }
    }
}
