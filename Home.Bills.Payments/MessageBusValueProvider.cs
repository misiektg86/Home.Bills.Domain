using System;
using Frameworks.Light.Ddd;
using MassTransit;
using Newtonsoft.Json.Serialization;

namespace Home.Bills.Payments
{
    public class MessageBusValueProvider : IValueProvider
    {
        private readonly Func<IBus> _busFactory;
        public MessageBusValueProvider(Func<IBus> busFactory)
        {
            _busFactory = busFactory;
        }

        public void SetValue(object target, object value)
        {
            ((Entity<Guid>)target).MessageBus = _busFactory?.Invoke();
        }

        public object GetValue(object target)
        {
            return "";
        }
    }
}