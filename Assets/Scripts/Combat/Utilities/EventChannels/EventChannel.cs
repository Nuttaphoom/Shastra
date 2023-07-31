

using UnityEngine;
using UnityEngine.Events;

namespace Vanaring_DepaDemo
{
    public abstract class EventChannel<T, U, V> : DescriptionScriptableObject
    {
        private UnityAction<T, U, V> OnEvent;

        public void SubEvent(UnityAction<T, U, V> eve)
        {
            OnEvent += eve;
        }

        public void UnSubEvent(UnityAction<T, U, V> eve)
        {
            OnEvent -= eve;
        }

        public void PlayEvent(T a, U b, V c)
        {
            OnEvent?.Invoke(a, b, c);
        }


    }

    public abstract class EventChannel<T, U> : DescriptionScriptableObject
    {
        private UnityAction<T, U> OnEvent;

        public void SubEvent(UnityAction<T, U> eve)
        {
            OnEvent += eve;
        }

        public void UnSubEvent(UnityAction<T, U> eve)
        {
            OnEvent -= eve;
        }

        public void PlayEvent(T a, U b)
        {
            OnEvent?.Invoke(a, b);
        }


    }

    public abstract class EventChannel<T > : DescriptionScriptableObject
    {
        private UnityAction<T > OnEvent;

        public void SubEvent(UnityAction<T > eve)
        {
            OnEvent += eve;
        }

        public void UnSubEvent(UnityAction<T > eve)
        {
            OnEvent -= eve;
        }

        public void PlayEvent(T a)
        {
            OnEvent?.Invoke(a );
        }


    }
}