using System;
using System.Collections.Generic;
using System.Diagnostics;

using UnityEngine;
using UnityEngine.Profiling;

using Debug = UnityEngine.Debug;

namespace Unity.Simulation
{
    /// <summary>
    /// Continuous Events class for creating and updating events that measure some metric at a frequency and aggregate over a period.
    /// </summary>
    public static class ContinuousEvents
    {
        // Public Members

        /// <summary>
        /// Dispatch delegate for consuming aggregated event data.
        /// </summary>
        /// <param name="event">The event whose aggregated data you wish to consume.</param>
        public delegate void EventDispatchDelegate(Event e);

        /// <summary>
        /// Delegate for collecting some metric.
        /// </summary>
        /// <returns>the metric represented by a double.</returns>
        public delegate double EventCollectionDelegate();

        /// <summary>
        /// Create a continuous event at a particular frequency, aggregated over a period.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <param name="interval">The interval in which to collect the metric, in seconds.</param>
        /// <param name="period">The aggregation period in seconds.</param>
        /// <param name="collector">A delegate to collect the metric.</param>
        /// <returns>A newly constructed Event instance.</returns>
        public static Event Create(string name, float interval, float period, EventCollectionDelegate collector)
        {
            var e = new Event(name, interval, period, collector);
            AddEvent(e);
            return e;
        }

        /// <summary>
        /// Add an event to be measure according to its frequency.
        /// </summary>
        /// <param name="event">The event to add.</param>
        public static void AddEvent(Event e)
        {
            if (!_events.Contains(e))
                _events.Add(e);
        }

        /// <summary>
        /// Removes an event from being collected.
        /// </summary>
        /// <param name="event">The event to remove.</param>
        public static void RemoveEvent(Event e)
        {
            _events.Remove(e);
        }

        /// <summary>
        /// The default dispatcher, which just logs to the console.
        /// </summary>
        /// <param name="event">The event to log.</param>
        /// <returns></returns>
        public static void DefaultDispatchDelegate(Event e)
        {
            Debug.Log($"Event {e.name} min {e.min} max {e.max} mean {e.mean} variance {e.variance}");
        }

        /// <summary>
        /// Event class which contains the aggregated data.
        /// </summary>
        public class Event
        {
            // Public Members

            /// <summary>
            /// The name of the event.
            /// </summary>
            public string name { get; protected set; }

            /// <summary>
            /// The min value collected over the aggregation period.
            /// </summary>
            public double min { get; protected set; }

            /// <summary>
            /// The max value collected over the aggregation period.
            /// </summary>
            public double max { get; protected set; }

            /// <summary>
            /// The mean value collected over the aggregation period.
            /// </summary>
            public double mean { get; protected set; }

            /// <summary>
            /// The variance of the values collected over the aggregation period.
            /// </summary>
            public double variance { get; protected set; }

            /// <summary>
            /// The delegate to use for dispatching events.
            /// </summary>
            public EventDispatchDelegate dispatchDelegate { get; set; }

            /// <summary>
            /// The delegate to use for collecting a metric to aggregate.
            /// </summary>
            public EventCollectionDelegate collector { get; set; }

            /// <summary>
            /// Constucts an Event that collects a metric each interval and aggregates over a period.
            /// </summary>
            /// <param name="name">The name of the event.</param>
            /// <param name="interval">The interval in which to collect the metric, in seconds.</param>
            /// <param name="period">The aggregation period in seconds.</param>
            /// <param name="collector">A delegate to collect the metric.</param>
            /// <returns>A newly constructed Event instance.</returns>
            public Event(string name, float interval, float period, EventCollectionDelegate collector)
            {
                this.name = name;
                _interval = interval;
                _period   = period;

                this.collector = collector;
                this.dispatchDelegate = DefaultDispatchDelegate;
            }

            // Non Public Members

            void Reset()
            {
                min      = double.MinValue;
                max      = double.MaxValue;
                mean     = 0;
                variance = 0;
                _count   = 0;
            }

            void IngestValue(double value)
            {
                min = value < min ? value : min;
                max = value > max ? value : max;

                ++_count;

                double delta, delta2;

                if (value > mean)
                {
                    delta  = value - mean;
                    mean += delta / _count;
                    delta2 = value - mean;
                }
                else
                {
                    delta  = mean - value;
                    mean -= delta / _count;
                    delta2 = mean - value;
                }

                variance += delta * delta2;
            }

            internal void Update(float deltaTime)
            {
                _elapsedInterval += deltaTime;
                _elapsedPeriod   += deltaTime;

                if (_elapsedInterval >= _interval)
                {
                    IngestValue(collector.Invoke());
                    _elapsedInterval -= _interval;
                }

                if (_elapsedPeriod >= _period)
                {
                    dispatchDelegate?.Invoke(this);
                    _elapsedPeriod -= _period;
                    Reset();
                }
            }

            int _count;

            float _elapsedInterval;
            float _elapsedPeriod;

            float _interval;
            float _period;
        }

        [RuntimeInitializeOnLoadMethod]
        static void Initialize()
        {
            Manager.Instance.Tick += (float dt) =>
            {
                foreach (var e in _events)
                    e.Update(dt);
            };
        }

        // Non Public Members

        static List<Event> _events = new List<Event>();
    }
}