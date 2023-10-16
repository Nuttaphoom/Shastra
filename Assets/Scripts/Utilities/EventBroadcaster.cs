using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Vanaring
{
    public class EventBroadcaster
    {
        private Dictionary<string, IEventChannel> _events = new Dictionary<string, IEventChannel>();

        public void SubEvent<T>(UnityAction<T> callback, string key)
        {
            if (callback == null)
                throw new ArgumentNullException();


            if (!_events.ContainsKey(key))
                _events[key] = new EventChannelRuntime<T>();


            _events[key].SubEvent(callback);
        }

        public void UnSubEvent<T>(UnityAction<T> callback, string key)
        {
            if (callback == null)
                throw new ArgumentNullException();

            if (!_events.ContainsKey(key))
                throw new Exception(key + "doesn't exit");


            _events[key].UnSubEvent(callback);

        }

        public void InvokeEvent<T>(T data, string key)
        {
            if (_events.TryGetValue(key, out var eventChannel))
            {
                eventChannel.InvokeEvent(data);
            }
        }
    }

  

    public interface IEventChannel
    {
        void SubEvent(object param);
        void UnSubEvent(object param);
        void InvokeEvent(object param);
    }

    public class EventChannelRuntime<U> : IEventChannel
    {
        private UnityAction<U> _event;

        public void InvokeEvent(object param)
        {
            _event?.Invoke(GetTrueInput(param)); 
        }

        public void SubEvent(object param)
        {
            _event += (UnityAction<U>)param;
        }

        public void UnSubEvent(object param)
        {

            _event -= (UnityAction<U>)param;
        }

        private U GetTrueInput(object param)
        {
            if (param is U uParam)
            {
                return uParam;
            }
            else
            {
                throw new InvalidCastException($"Invalid cast: Expected type {typeof(U)}, but got {param.GetType()}");
            }


        }

    }
}





 