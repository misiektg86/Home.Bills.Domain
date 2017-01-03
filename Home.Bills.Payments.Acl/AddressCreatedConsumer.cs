using System.Threading.Tasks;
using Home.Bills.Domain.Messages;
using Home.Bills.Payments.Domain.Commands;
using MassTransit;

namespace Home.Bills.Payments.Acl
{
    public class AddressCreatedConsumer : IConsumer<IAddressCreated>
    {
        public Task Consume(ConsumeContext<IAddressCreated> context)
        {
            context.Publish(new CreateAddress() { Id = context.Message.Id, SquareMeters = context.Message.SquareMeters });

            return Task.FromResult(true);
        }
    }
}