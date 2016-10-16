using Frameworks.Light.Ddd;
using Home.Bills.Domain.AddressAggregate.ValueObjects;

namespace Home.Bills.Domain.AddressAggregate.Entities
{
    public class Meter : Entity<MeterId>
    {
        public Meter(MeterId id)
        {
            Id = id;
        }

        public double State { get; set; }

        internal Meter Clone() => MemberwiseClone() as Meter;
    }
}