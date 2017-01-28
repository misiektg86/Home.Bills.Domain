using System;
using Frameworks.Light.Ddd;
using Newtonsoft.Json;

namespace Home.Bills.Payments.Domain.AddressAggregate
{
    public class Address : AggregateRoot<Guid>
    {
        internal Address() { }

        internal Address(Guid id, double squareMeters)
        {
            SquareMeters = squareMeters;

            Id = id;
        }

        public Guid? RentId { get; private set; }

        [JsonIgnore]
        public int Persons { get; private set; }

        [JsonIgnore]
        public double SquareMeters { get; }

        public void ApplyRent(Guid rentId)
        {
            RentId = rentId;

            Publish(new RentAppliedForAddress { AddressId = Id, RentId = rentId });
        }

        public void UpdatePersons(int persons)
        {
            Persons = persons;

            Publish(new PersonsUpdatedAtAddress { AddressId = Id, Persons = Persons });
        }
    }
}