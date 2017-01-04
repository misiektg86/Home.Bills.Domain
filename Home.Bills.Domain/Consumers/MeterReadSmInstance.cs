using System;
using System.Collections.Generic;
using Automatonymous;

namespace Home.Bills.Domain.Consumers
{
    public class MeterReadSmInstance : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }

        public State CurrentState { get; set; }

        public List<Guid> UsagesToCalculate { get; set; }

        public Guid MeterReadId { get; set; }

        public Guid AddressId { get; set; }
    }
}