namespace Home.Bills.Payments.Domain.AddressAggregate
{
    internal class StandardRentForApartmentPolicy : IRentPolicy
    {
        public decimal Calculate(double squareMeters, int persons)
        {
            return 463.04m; // TODO to replace with detailed rent.
        }
    }
}