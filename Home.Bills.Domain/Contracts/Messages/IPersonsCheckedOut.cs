using System;
using MediatR;

namespace Home.Bills.Domain.Contracts.Messages
{
    public interface IPersonsCheckedOut : INotification
    {
        Guid Id { get; }
        int Persons { get; }
    }
}