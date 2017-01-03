using System;

namespace Home.Bills.Payments.Domain.Commands
{
    public class CreateAddress
    {
        public Guid Id { get; set; }

        public double SquareMeters { get; set; }
    }
}