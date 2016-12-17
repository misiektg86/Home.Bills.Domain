using System;

namespace Home.Bills.Domain.Messages
{
    public interface IPersonsCheckedIn
    {
        Guid Id { get; }
        int Persons { get; }
    }
}