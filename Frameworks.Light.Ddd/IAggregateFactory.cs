﻿namespace Frameworks.Light.Ddd
{
    public interface IAggregateFactory<out TAggregateRoot, in TCreateInput, TEntityId> where TAggregateRoot : AggregateRoot<TEntityId>
    {
        TAggregateRoot Create(TCreateInput input);
    }
}