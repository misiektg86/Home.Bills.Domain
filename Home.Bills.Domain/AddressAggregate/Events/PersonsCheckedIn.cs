using System;
using Home.Bills.Domain.Contracts.Messages;
using MediatR;

namespace Home.Bills.Domain.AddressAggregate.Events
{
    internal class PersonsCheckedIn : IPersonsCheckedIn
    {
        public Guid Id { get; }
        public int Persons { get; }

        public PersonsCheckedIn(Guid id, int persons)
        {
            Id = id;
            Persons = persons;
        }
    }
}