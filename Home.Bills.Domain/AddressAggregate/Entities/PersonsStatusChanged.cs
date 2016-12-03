using System;
using MediatR;

namespace Home.Bills.Domain.AddressAggregate.Entities
{
    public class PersonsStatusChanged : INotification
    {
        public Guid Id { get; set; }
        public int CheckedInPersons { get; set; }

        public PersonsStatusChanged(Guid id, int checkedInPersons)
        {
            Id = id;
            CheckedInPersons = checkedInPersons;
        }
    }
}