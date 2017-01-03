using System;

namespace Home.Bills.Domain.AddressAggregate.Events
{
    public class PersonsStatusChanged
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