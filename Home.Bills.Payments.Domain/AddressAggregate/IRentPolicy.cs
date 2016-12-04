namespace Home.Bills.Payments.Domain.AddressAggregate
{
    internal interface IRentPolicy
    {
        decimal Calculate(double squareMeters, int persons);
    }
}