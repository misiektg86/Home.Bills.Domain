using System;
using Frameworks.Light.Ddd;
using MassTransit;
using Newtonsoft.Json;

namespace Home.Bills.Payments.Domain.AddressAggregate
{
    public class Address : AggregateRoot<Guid>
    {
        internal Address() { }

        internal Address(IBus messageBus)
        {
            MessageBus = messageBus;
        }

        internal Address(Guid id, double squareMeters, IBus messageBus) : this(messageBus)
        {
            SquareMeters = squareMeters;

            Id = id;
        }

        [JsonIgnore]
        public Guid RentId { get; private set; }

        [JsonIgnore]
        public int Persons { get; private set; }

        [JsonIgnore]
        public double SquareMeters { get; }

        public void ApplyRent(Guid rentId)
        {
            RentId = rentId;

            Publish(new RentAppliedForAddress { AddressId = Id, RentId = RentId });
        }

        public void UpdatePersons(int persons)
        {
            Persons = persons;

            Publish(new PersonsUpdatedAtAddress { AddressId = Id, Persons = Persons });
        }
    }
}