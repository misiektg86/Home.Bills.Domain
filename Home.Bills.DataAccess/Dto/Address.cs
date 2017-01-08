using System;
using System.Collections.Generic;

namespace Home.Bills.DataAccess.Dto
{
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string StreetNumber { get; set; }
        public string HomeNumber { get; set; }
        public Guid AddressId { get; set; }
        public double SquareMeters { get; set; }
        public int CheckedInPersons { get; set; }
        public Guid? LastFinishedMeterReadProcess { get; set; }
        public Guid? MeterReadId { get; set; }
        public IEnumerable<Guid> Meters { get; set; }
    }
}