using System;
using System.Collections.Generic;

namespace Sopra.ECS
{
    /// <summary>
    /// Basic implementation of a publish / subscribe event bus.
    ///
    /// usage:
    /// class TestEvent : IGameEvent {}
    ///
    /// somewhere else:
    /// Events.Instance.Subscribe(OnTestEvent)
    ///
    /// void OnTestEvent(TestEvent testEvent) {}
    /// </summary>
    /// <author>Michael Fleig</author>
    public sealed class Events
    {
        private static Events sInstance;

        public static Events Instance => sInstance ?? (sInstance = new Events());

        public delegate void EventDelegate<in T>(T e) where T : IGameEvent;

        private delegate void EventDelegate(IGameEvent e);

        private readonly Dictionary<Type, EventDelegate> mDelegates = new Dictionary<Type, EventDelegate>();

        private readonly Dictionary<Delegate, EventDelegate> mDelegateLookup =
            new Dictionary<Delegate, EventDelegate>();

        /// <summary>
        /// Subscribe to a game event.
        /// </summary>
        /// <param name="del">Delegate wich should be called if the event is fired.</param>
        /// <typeparam name="T">Event type on which the delegate should react.</typeparam>
        public void Subscribe<T>(EventDelegate<T> del) where T : IGameEvent
        {
            // Early-out if we've already registered this delegate
            if (mDelegateLookup.ContainsKey(del)) { return; }

            // Create a new non-generic delegate which calls our generic one.
            // This is the delegate we actually invoke.
            EventDelegate internalDelegate = e => del((T) e);
            mDelegateLookup[del] = internalDelegate;

            EventDelegate tempDel;
            if (mDelegates.TryGetValue(typeof(T), out tempDel))
            {
                mDelegates[typeof(T)] = tempDel + internalDelegate;
            }
            else
            {
                mDelegates[typeof(T)] = internalDelegate;
            }
        }

        /// <summary>
        /// Fire a game event and invoke all its listeners.
        /// </summary>
        /// <param name="e">event to be fired</param>
        public void Fire(IGameEvent e)
        {
            EventDelegate del;
            if (mDelegates.TryGetValue(e.GetType(), out del))
            {
                del.Invoke(e);
            }
        }
    }
}