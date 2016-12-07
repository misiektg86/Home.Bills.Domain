using System;
using MediatR;

namespace Home.Bills.Domain.Contracts.Messages
{
    public interface IUsageCreated : INotification
    {
        string MeterSerialNumber { get; }
        DateTime ReadDateTime { get; }
        Guid AddressId { get; }
        double Value { get; }
    }
}