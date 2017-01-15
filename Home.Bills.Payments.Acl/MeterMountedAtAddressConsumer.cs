using System.Threading.Tasks;
using Home.Bills.Domain.Messages;
using Home.Bills.Payments.Messages;
using MassTransit;

namespace Home.Bills.Payments.Acl
{
    public class MeterMountedAtAddressConsumer : IConsumer<IMeterMountedAtAddress>
    {
        public Task Consume(ConsumeContext<IMeterMountedAtAddress> context)
        {
           return context.Publish<IRegistratorAdded>(
                new
                {
                    context.Message.AddressId,
                    Description = context.Message.MeterSerialNumber,
                    RegistratorId = context.Message.MeterId
                });
        }
    }
}