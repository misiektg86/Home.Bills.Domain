using System.Threading.Tasks;
using Home.Bills.Domain.Messages;
using Home.Bills.Payments.Messages;
using MassTransit;

namespace Home.Bills.Payments.Acl
{
    public class AddressAddedConsumer : IConsumer<IAddressCreated>
    {
        public Task Consume(ConsumeContext<IAddressCreated> context)
        {
            context.Publish<IAddressAdded>(new {context.Message.Id, context.Message.SquareMeters });

            return Task.FromResult(true);
        }
    }
}