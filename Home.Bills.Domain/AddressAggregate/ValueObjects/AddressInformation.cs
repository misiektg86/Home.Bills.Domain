namespace Home.Bills.Domain.AddressAggregate.ValueObjects
{
    public class AddressInformation
    {
        public AddressInformation(string street, string city, string streetNumber, string homeNumber)
        {
            Street = street;
            City = city;
            StreetNumber = streetNumber;
            HomeNumber = homeNumber;
        }

        public string Street { get; }
        public string City { get; }
        public string StreetNumber { get; }
        public string HomeNumber { get; }

        internal AddressInformation Clone() => MemberwiseClone() as AddressInformation;
    }
}