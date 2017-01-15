using System;

namespace Home.Bills.Payments.Messages
{
    public interface IRegistratorAdded
    {
        Guid RegistratorId { get; set; }

        string Description { get; set; }

        Guid AddressId { get; set; }
    }
}
