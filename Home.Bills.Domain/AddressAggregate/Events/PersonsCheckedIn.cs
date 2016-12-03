using System;
using MediatR;

namespace Home.Bills.Domain.AddressAggregate.Events
{
    public class PersonsCheckedIn : INotification
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