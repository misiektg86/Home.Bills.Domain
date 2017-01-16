using System;
using Newtonsoft.Json.Serialization;

namespace Frameworks.Light.Ddd
{
    public class PublishRecorderValueProvider : IValueProvider
    {
        private readonly Func<IPublishRecorder> _factory;

        public PublishRecorderValueProvider(Func<IPublishRecorder> factory)
        {
            _factory = factory;
        }

        public void SetValue(object target, object value)
        {
            ((Entity<Guid>)target).Recorder = _factory?.Invoke();
        }

        public object GetValue(object target)
        {
            return "";
        }
    }
}