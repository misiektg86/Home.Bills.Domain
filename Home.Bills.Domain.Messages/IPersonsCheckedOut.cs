using System;

namespace Home.Bills.Domain.Messages
{
    public interface IPersonsCheckedOut
    {
        Guid Id { get; }
        int Persons { get; }
    }
}