using System;
using Home.Bills.Domain.Contracts.Messages;
using MediatR;

namespace Home.Bills.Domain.AddressAggregate.Events
{
    internal class PersonsCheckedOut : IPersonsCheckedOut
    {
        public Guid Id { get; }
        public int Persons { get; }

        public PersonsCheckedOut(Guid id, int persons)
        {
            Id = id;
            Persons = persons;
        }
    }
}