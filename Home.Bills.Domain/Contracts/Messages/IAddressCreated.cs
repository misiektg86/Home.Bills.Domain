using System;
using MediatR;

namespace Home.Bills.Domain.Contracts.Messages
{
    public interface IAddressCreated : INotification
    {
        Guid Id { get; }
    }
}