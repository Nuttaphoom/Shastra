using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Vanaring 
{
    [Serializable]
    public struct ActionTimelineSettingStruct
    {
        [SerializeField]
        private List<string> _trackToChangeName;

        private List<GameObject> _timelienActors ; 
        
        public void AddActors(GameObject actor)
        {
            if (_timelienActors == null) 
                _timelienActors = new List<GameObject>()  ; 
            
            _timelienActors.Add(actor);
        }

        public GameObject GetObjectWithTrackName(string trackName)
        {
            for (int i = 0; i < _trackToChangeName.Count;i++)
            {
                if (_trackToChangeName[i] == trackName)
                {
                    return _timelienActors[i]; 
                }
            }

            throw new Exception("Given track name " + trackName + " is not listed in this action") ;
        }

        public List<string> TrackNames => _trackToChangeName;
    }
    [Serializable] 
    public class ActionSignal
    {
        #region Inspector Setting  
        [SerializeField]
        private TimelineAsset _timelineAsset;

        [Header("Signal need to be match give _effects")]
        [SerializeField]
        private List<SignalType> _signals = new List<SignalType>() ;

        [Header("Sequence of effect that would be called from Timeline when signal received")]
        [SerializeField]
        private List<RuntimeEffectFactorySO> _effects = new List<RuntimeEffectFactorySO>() ;

        [SerializeField]
        private ActionTimelineSettingStruct _actionTimelineSetting;
        #endregion
        
        private Dictionary<SignalType, RuntimeEffectFactorySO> _runtimeEffectWithSignal= new Dictionary<SignalType, RuntimeEffectFactorySO>() ;
        private Queue<RuntimeEffectFactorySO> _readyEffectQueue = new Queue<RuntimeEffectFactorySO>();

        public ActionSignal(ActionSignal copied)
        {
            for (int i = 0; i < copied._signals.Count; i++ )
                _runtimeEffectWithSignal.Add(copied._signals[i], copied._effects[i]);

            if (copied._signals.Count != copied._effects.Count)
                throw new Exception("Signal and Effect is not relavant");

            _actionTimelineSetting = copied._actionTimelineSetting;
            _timelineAsset = copied._timelineAsset;


        }

        /// <summary>
        /// Should be called from DirectorManager only 
        /// </summary>
        /// <param name="signal"></param>
        public void ReceiveSignal(SignalType signal)
        {
            if (! _runtimeEffectWithSignal.ContainsKey(signal))
                throw new Exception(signal + "is not found in dictionary") ;
 
            _readyEffectQueue.Enqueue(_runtimeEffectWithSignal[signal]);
            _runtimeEffectWithSignal.Remove(signal); 
        }

        public void SetUpActorsSetting(List<GameObject> actors)
        {
            foreach (var obj in actors)
                _actionTimelineSetting.AddActors(obj);
        }

        #region GETTER 
        public ActionTimelineSettingStruct GetActionTimelineSettingStruct => _actionTimelineSetting; 
        public RuntimeEffectFactorySO GetReadyEffect()
        {
            bool contained = _readyEffectQueue.Count > 0;
            RuntimeEffectFactorySO effect;

            if (contained)
                effect = _readyEffectQueue.Dequeue();
            else
                effect = null;

            return (effect) ;
        }

        public TimelineAsset TimelineAsset => _timelineAsset;
        public bool SignalTerminated()
        {
            return _runtimeEffectWithSignal.Count == 0 && _readyEffectQueue.Count == 0; 
        }

        public List<RuntimeEffectFactorySO> GetRuntimeEffects()
        {
            List<RuntimeEffectFactorySO> ret = new List<RuntimeEffectFactorySO>(); 

            foreach (var key in _runtimeEffectWithSignal.Keys)
            {
                ret.Add(_runtimeEffectWithSignal[key]); //.SimulateEnergyModifier(); 
            }
            return ret; 
        }
        #endregion
    }


}
