//using System.Threading.Tasks;
//using Home.Bills.Domain.Messages;
//using Home.Bills.Payments.Domain.Commands;
//using MassTransit;
//using MediatR;

//namespace Home.Bills.Payments.Acl
//{
//    public class UsageCreatedConsumer : IConsumer<IUsageCreated>
//    {
//        private readonly IMediator _mediator;

//        public UsageCreatedConsumer(IMediator mediator)
//        {
//            _mediator = mediator;
//        }

//        public Task Consume(ConsumeContext<IUsageCreated> context)
//        {
//            return _mediator.PublishAsync(new RegisterUsage()
//            {
//                AddressId = context.Message.AddressId,
//                MeterSerialNumber = context.Message.MeterSerialNumber,
//                ReadDateTime = context.Message.ReadDateTime,
//                Value = context.Message.Value
//            });
//        }
//    }
//}