using System;
using System.Collections.Generic;
using Automatonymous;
using Marten.Schema;

namespace Home.Bills.Domain.Consumers
{
    public class MeterReadSmInstance : SagaStateMachineInstance
    {
        [Identity]
        public Guid CorrelationId { get; set; }

        public string CurrentState { get; set; }

        public List<Guid> UsagesToCalculate { get; set; }

        public Guid MeterReadId { get; set; }

        public Guid AddressId { get; set; }
    }
}