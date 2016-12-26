using System;
using MediatR;

namespace Home.Bills.Payments.Domain.Events
{
    public class AddressCreated : INotification
    {
        public Guid Id { get; set; }

        public double SquareMeters { get; set; }
    }
}