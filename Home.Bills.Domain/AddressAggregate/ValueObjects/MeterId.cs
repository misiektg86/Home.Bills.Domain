namespace Home.Bills.Domain.AddressAggregate.ValueObjects
{
    public class MeterId
    {
        public MeterId(string serialNumber)
        {
            SerialNumber = serialNumber;
        }

        public string SerialNumber { get; }

        public static implicit operator string(MeterId source)
        {
            return source.SerialNumber;
        }
    }
}