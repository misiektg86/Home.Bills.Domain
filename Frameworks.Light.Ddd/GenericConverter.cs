using System;
using Newtonsoft.Json;

namespace Frameworks.Light.Ddd
{
    public class GenericConverter<T> : JsonConverter where T : class
    {
        private readonly Func<T> _factory;

        public GenericConverter(Func<T> factory)
        {
            _factory = factory;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, "");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return _factory?.Invoke();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T);
        }
    }
}