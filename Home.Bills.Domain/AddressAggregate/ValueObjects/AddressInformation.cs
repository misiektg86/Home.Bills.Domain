using System;

namespace Home.Bills.Domain.AddressAggregate.ValueObjects
{
    public class AddressInformation
    {
        public AddressInformation(string street, string city, string streetNumber, string homeNumber, Guid addressId, double squareMeters)
        {
            Street = street;
            City = city;
            StreetNumber = streetNumber;
            HomeNumber = homeNumber;
            AddressId = addressId;
            SquareMeters = squareMeters;
        }

        internal AddressInformation() { }

        public string Street { get; }
        public string City { get; }
        public string StreetNumber { get; }
        public string HomeNumber { get; }
        public Guid AddressId { get; }
        public double SquareMeters { get; }

        internal AddressInformation Clone() => MemberwiseClone() as AddressInformation;
    }
}