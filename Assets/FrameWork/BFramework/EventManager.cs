using System;
using System.Collections.Generic;

namespace BFramework
{
    public class EventManager:IManager
    {
        public static readonly EventManager Global = new EventManager();

        private readonly Dictionary<Type, IEvent> mTypeEvents = new Dictionary<Type, IEvent>();

        public T GetEvent<T>() where T : IEvent
        {
            return mTypeEvents.TryGetValue(typeof(T), out var e) ? (T)e : default;
        }

        public T GetOrAddEvent<T>() where T : IEvent, new()
        {
            var eType = typeof(T);
            if (mTypeEvents.TryGetValue(eType, out var e))
            {
                return (T)e;
            }

            var t = new T();
            mTypeEvents.Add(eType, t);
            return t;
        }
        public void Send<T>() where T : new() => Global.GetEvent<Event<T>>()?.Trigger(new T());

        public void Send<T>(T e) => Global.GetEvent<Event<T>>()?.Trigger(e);

        public void Register<T>(Action<T> onEvent) => Global.GetOrAddEvent<Event<T>>().Register(onEvent);
        public void Register<T,U>(Action<T,U> onEvent) => Global.GetOrAddEvent<Event<T,U>>().Register(onEvent);
        public void UnRegister<T>(Action<T> onEvent)
        {
            var e = Global.GetEvent<Event<T>>();
            if (e != null)
            {
                e.UnRegister(onEvent);
            }
        }

        public void Init()
        {
         
        }
    }
 
    #region IEvent

    public interface IEvent
    {
        void Register(Action onEvent);
    }

    public class Event : IEvent
    {
        private Action mOnEvent = () => { };

        public void Register(Action onEvent)
        {
            mOnEvent += onEvent;
        }

        public void UnRegister(Action onEvent) => mOnEvent -= onEvent;

        public void Trigger() => mOnEvent?.Invoke();
    }

    public class Event<T> : IEvent
    {
        private Action<T> mOnEvent = e => { };

        public void Register(Action<T> onEvent)
        {
            mOnEvent += onEvent;
        }

        public void UnRegister(Action<T> onEvent) => mOnEvent -= onEvent;

        public void Trigger(T t) => mOnEvent?.Invoke(t);

        void IEvent.Register(Action onEvent)
        {
            Register(Action);
            void Action(T _) => onEvent();
        }
    }

    public class Event<T, K> : IEvent
    {
        private Action<T, K> mOnEvent = (t, k) => { };

        public void Register(Action<T, K> onEvent)
        {
            mOnEvent += onEvent;
        }

        public void UnRegister(Action<T, K> onEvent) => mOnEvent -= onEvent;

        public void Trigger(T t, K k) => mOnEvent?.Invoke(t, k);

        void IEvent.Register(Action onEvent)
        {
            Register(Action);
            void Action(T _, K __) => onEvent();
        }
    }
    #endregion
}

