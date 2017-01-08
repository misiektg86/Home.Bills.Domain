using System;
using System.Reflection;
using Frameworks.Light.Ddd;
using MassTransit;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Home.Bills.Payments
{
    public class ContractResolver : DefaultContractResolver
    {
        private readonly Func<IBus> _busFactory;
        public ContractResolver(Func<IBus> busFactory)
        {
            _busFactory = busFactory;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (property.PropertyType == typeof(IBus) && property.PropertyName == "MessageBus")
            {
                property.Writable = true;
                property.Readable = true;

                property.ShouldSerialize = value => true;
                property.Converter = new MessageBusConverter(_busFactory);
                property.ValueProvider = new MessageBusValueProvider(_busFactory);
            }

            return property;
        }
    }
}