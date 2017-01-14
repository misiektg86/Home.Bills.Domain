using System;

namespace Home.Bills.Payments.DataAccess.Dtos
{
    public class Tariff
    {
        public DateTime Created { get; set; }
        public DateTime? ValidTo { get; set; }
        public bool Revoked { get; set; }
        public string Description { get; set; }
    }
}