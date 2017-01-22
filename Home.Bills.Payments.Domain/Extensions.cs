using System;

namespace Home.Bills.Payments.Domain
{
    public static class Extensions
    {
        public static bool HasExpired(this DateTime? source) => source.HasValue && source < DateTime.Now;
    }
}