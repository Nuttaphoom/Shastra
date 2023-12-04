using Cinemachine;
using CustomYieldInstructions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Numerics;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static Cinemachine.CinemachineTargetGroup;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;
namespace Vanaring
{
    
    public class EventBroadcaster
    {
        private Dictionary<string, IEventChannel> _events = new Dictionary<string, IEventChannel>();

        public void OpenChannel<T>(string key)
        {
            if (_events.ContainsKey(key)) 
                return;


            _events[key] = new EventChannelRuntime<T>();
        }

        ~EventBroadcaster()
        {
            _events.Clear(); 
        }

        public void SubEvent<T>(UnityAction<T> callback, string key)
        {
            if (callback == null)
                throw new ArgumentNullException();


            if (!_events.ContainsKey(key))
                throw new Exception(key + " is not found in this broadcaster");


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
            }else
            {
                throw new Exception(key + " channel can not be found"); 
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
            if (param is U uParam )
            {
                return uParam;
            }
            else if (param is null)
            {
                return (U)param; 
            }
            else
            {
                throw new InvalidCastException($"Invalid cast: Expected type {typeof(U)}, but got {param.GetType()}");
            }
        }
    }

    #region StructForSubscription 
    public struct EntityActionPair
    {
        public CombatEntity Actor ;
        public ActorAction PerformedAction ;
    }
    public struct EntityStatusEffectPair
    {
        public CombatEntity Actor ;
        public StatusRuntimeEffectFactorySO StatusEffectFactory ;
        public StatusEffectApplierRuntimeEffect ApplierFactory ;
    }
    public struct EntityAilmentEffectPair
    {
        public CombatEntity Actor;
        public Ailment Ailment; 
    }
    public struct EntityAilmentApplierEffect
    {
        public EntityAilmentEffectPair AilmentEffectPair;
        public bool SucessfullyAttach;
        public bool ResistantBlocked;
    }
    
    public struct EnergyModifyerEffectPair
    {
        public RuntimeMangicalEnergy.EnergySide EnergySide;
        public int Amount; 
    }
    #endregion
}





