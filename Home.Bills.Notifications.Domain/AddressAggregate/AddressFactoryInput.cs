using System;

namespace Home.Bills.Notifications.Domain.AddressAggregate
{
    public class AddressFactoryInput
    {
        public Guid AddressId { get; set; }

        public string FullAddressName { get; set; }
    }
}