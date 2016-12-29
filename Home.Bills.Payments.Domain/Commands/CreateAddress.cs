using System;
using MediatR;

namespace Home.Bills.Payments.Domain.Commands
{
    public class CreateAddress : INotification
    {
        public Guid Id { get; set; }

        public double SquareMeters { get; set; }
    }
}