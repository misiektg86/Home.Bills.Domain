﻿using System;

namespace Home.Bills.Models
{
    public class Meter
    {
        public Guid AddressId { get; set; }

        public Guid MeterId { get; set; }

        public string MeterSerialNumber { get; set; }

        public double State { get; set; }
    }
}