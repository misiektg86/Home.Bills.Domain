using System;

namespace Frameworks.Light.Ddd
{
    public interface IPublishRecorder
    {
        void Record<T>(Action<T> action, T message) where T : class;

        void Play();
    }
}