using System.Threading.Tasks;
using Home.Bills.Domain.Messages;
using Home.Bills.Payments.Domain.Commands;
using MassTransit;
using MediatR;

namespace Home.Bills.Payments.Acl
{
    public class AddressCreatedConsumer : IConsumer<IAddressCreated>
    {
        private readonly IMediator _mediator;

        public AddressCreatedConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Consume(ConsumeContext<IAddressCreated> context)
        {
            _mediator.Publish(new CreateAddress() { Id = context.Message.Id, SquareMeters = context.Message.SquareMeters });

            return Task.FromResult(true);
        }
    }
}