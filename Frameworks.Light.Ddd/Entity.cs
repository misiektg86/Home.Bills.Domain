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

        public IPublishRecorder Recorder { get; set; }

        protected void Await(Task task)
        {
            TaskUtil.Await(async () => await task);
        }

        protected void Publish<T>(T message) where T : class
        {
            Recorder.Record(t =>
            {
                var task = MessageBus != null ? MessageBus.Publish(t) : StaticBus.Publish(t);
                Await(task);
            }, message);
        }
    }
}