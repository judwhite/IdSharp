using System;
using System.Collections.Generic;

namespace IdSharp.Tagging.Harness.Wpf.Events
{
    /// <summary>
    /// Event dispatcher.
    /// </summary>
    public static class EventDispatcher
    {
        private static readonly Dictionary<EventType, List<Action<object>>> _events = new Dictionary<EventType, List<Action<object>>>();

        /// <summary>
        /// Publishes the specified event type.
        /// </summary>
        /// <param name="eventType">The event type.</param>
        public static void Publish(EventType eventType)
        {
            List<Action<object>> eventHandlers;
            if (_events.TryGetValue(eventType, out eventHandlers))
            {
                foreach (var eventHandler in eventHandlers)
                {
                    eventHandler(null);
                }
            }
        }

        /// <summary>
        /// Subscribes a handler to the specified event type.
        /// </summary>
        /// <param name="eventType">The event type.</param>
        /// <param name="eventHandler">The event handler.</param>
        public static void Subscribe(EventType eventType, Action eventHandler)
        {
            if (eventHandler == null)
                throw new ArgumentNullException("eventHandler");

            List<Action<object>> eventHandlers;
            if (!_events.TryGetValue(eventType, out eventHandlers))
            {
                eventHandlers = new List<Action<object>>();
                _events.Add(eventType, eventHandlers);
            }

            eventHandlers.Add(o => eventHandler());
        }

        /// <summary>
        /// Publishes the specified event type.
        /// </summary>
        /// <param name="eventType">The event type.</param>
        /// <param name="parameter">The parameter.</param>
        public static void Publish(EventType eventType, object parameter)
        {
            List<Action<object>> eventHandlers;
            if (_events.TryGetValue(eventType, out eventHandlers))
            {
                foreach (var eventHandler in eventHandlers)
                {
                    eventHandler(parameter);
                }
            }
        }

        /// <summary>
        /// Subscribes a handler to the specified event type.
        /// </summary>
        /// <param name="eventType">The event type.</param>
        /// <param name="eventHandler">The event handler.</param>
        public static void Subscribe<T>(EventType eventType, Action<T> eventHandler)
        {
            if (eventHandler == null)
                throw new ArgumentNullException("eventHandler");

            List<Action<object>> eventHandlers;
            if (!_events.TryGetValue(eventType, out eventHandlers))
            {
                eventHandlers = new List<Action<object>>();
                _events.Add(eventType, eventHandlers);
            }

            eventHandlers.Add(o => eventHandler((T)o));
        }
    }
}
