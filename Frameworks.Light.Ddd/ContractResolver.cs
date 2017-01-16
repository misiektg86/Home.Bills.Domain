using System;
using System.Reflection;
using MassTransit;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Frameworks.Light.Ddd
{
    public class ContractResolver : DefaultContractResolver
    {
        private readonly Func<IBus> _busFactory;
        private readonly Func<IPublishRecorder> _recorderFactory;

        public ContractResolver(Func<IBus> busFactory, Func<IPublishRecorder> recorderFactory)
        {
            _busFactory = busFactory;
            _recorderFactory = recorderFactory;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (property.PropertyType == typeof(IBus) && property.PropertyName == "MessageBus")
            {
                property.Writable = true;
                property.Readable = true;

                property.ShouldSerialize = value => true;
                property.Converter = new GenericConverter<IBus>(_busFactory);
                property.ValueProvider = new MessageBusValueProvider(_busFactory);
            }
            else if (property.PropertyType == typeof(IPublishRecorder) && property.PropertyName == "Recorder")
            {
                property.Writable = true;
                property.Readable = true;

                property.ShouldSerialize = value => true;
                property.Converter = new GenericConverter<IPublishRecorder>(_recorderFactory);
                property.ValueProvider = new PublishRecorderValueProvider(_recorderFactory);
            }

            return property;
        }
    }
}