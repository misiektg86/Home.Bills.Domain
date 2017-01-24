using System;
using System.Collections.Generic;
using Automatonymous;

namespace Home.Bills.Notifications.Domain.Consumers.Saga
{
    public class PaymentNotificationStateMachineInstance: SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }

        public string State { get; set; }
    }
}