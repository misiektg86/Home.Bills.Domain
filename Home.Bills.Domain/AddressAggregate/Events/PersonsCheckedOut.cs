using System;
using MediatR;

namespace Home.Bills.Domain.AddressAggregate.Events
{
    public class PersonsCheckedOut : INotification
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