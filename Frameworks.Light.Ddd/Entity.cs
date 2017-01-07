using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Util;
using Newtonsoft.Json;

namespace Frameworks.Light.Ddd
{
    public class Entity<TEntityId>
    {
        [JsonIgnore]
        public static IBus StaticBus { get; set; }  // TODO to remove

        public TEntityId Id { get; protected set; }

        public IBus MessageBus { get; set; }

        protected void Await(Task task)
        {
            TaskUtil.Await(async () => await task);
        }

        protected void Publish<T>(T message) where T : class
        {
            var task = MessageBus != null ? MessageBus.Publish(message) : StaticBus.Publish(message);
            Await(task);
        }
    }
    public class MessageBusConverter : JsonConverter
    {
        private readonly Func<IBus> _busFactory;

        public MessageBusConverter(Func<IBus> busFactory)
        {
            _busFactory = busFactory;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, "");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return _busFactory?.Invoke();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IBus);
        }
    }
}