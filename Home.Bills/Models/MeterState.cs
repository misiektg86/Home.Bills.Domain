using System;

namespace Home.Bills.Models
{
    public class MeterState
    {
        public Guid MeterId { get; set; }
        
        public double State { get; set; }
    }
}