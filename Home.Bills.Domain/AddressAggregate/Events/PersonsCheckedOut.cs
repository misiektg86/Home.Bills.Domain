using System;
using Home.Bills.Domain.Messages;
using MediatR;

namespace Home.Bills.Domain.AddressAggregate.Events
{
    internal class PersonsCheckedOut : IPersonsCheckedOut, INotification
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