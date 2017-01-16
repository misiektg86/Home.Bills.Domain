using System;
using System.Collections.Generic;
using MassTransit;

namespace Frameworks.Light.Ddd
{
    public class PublishRecorder : IPublishRecorder
    {
        private readonly IBus _messageBus;

        public PublishRecorder(IBus messageBus)
        {
            _messageBus = messageBus;
        }

        private List<Action> _actionList = new List<Action>();

        public void Record<T>(Action<T> action, T message) where T : class
        {
            _actionList.Add(() => action(message));
        }

        public void Play()
        {
            foreach (var action in _actionList)
            {
                action();
            }
        }
    }
}