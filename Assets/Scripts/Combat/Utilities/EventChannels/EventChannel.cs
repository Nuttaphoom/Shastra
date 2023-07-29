

using UnityEngine;
using UnityEngine.Events;

namespace Vanaring_DepaDemo
{
    public abstract class EventChannel<T, U, V> : ScriptableObject
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
}