﻿using System;

namespace Home.Bills.Domain.Messages
{
    public interface IUsageCreated
    {
        DateTime ReadDateTime { get; }
        Guid AddressId { get; }
        double Value { get; }
    }
}