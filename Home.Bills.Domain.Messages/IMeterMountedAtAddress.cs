using System;

namespace Home.Bills.Domain.Messages
{
    public interface IMeterMountedAtAddress
    {
        Guid MeterId { get; set; }
        string MeterSerialNumber { get; set; }
        Guid AddressId { get; set; }
    }
}