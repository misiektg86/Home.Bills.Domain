using System;
using System.Collections.Generic;
using Automatonymous;
using Marten.Schema;


namespace Home.Bills.Notifications.Domain.Consumers.Saga
{
    public class PaymentNotificationStateMachineInstance: SagaStateMachineInstance
    {
        [Identity]
        public Guid CorrelationId { get; set; }

        public string State { get; set; }
    }
}