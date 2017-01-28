using System;

namespace Home.Bills.Notifications.Models
{
    public class CreateAddress
    {
        public Guid AddressId { get; set; }

        public string FullAddress { get; set; }
    }
}