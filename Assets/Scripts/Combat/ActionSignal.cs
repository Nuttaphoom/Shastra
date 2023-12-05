using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Vanaring 
{
    [Serializable]
    public class ActionTimelineSettingStruct
    {
        [SerializeField]
        private List<string> _trackToChangeName ;

        private List<Transform> _timelienActors;

        public ActionTimelineSettingStruct(ActionTimelineSettingStruct copied)
        {
            _trackToChangeName = new List<string>();
            _timelienActors = new List<Transform>();

            //Debug.Log(" copied._trackToChangeName : " + copied._trackToChangeName);
            //Debug.Log(" copied._timelienActors : " + copied._timelienActors);
            foreach (var trackName in copied._trackToChangeName)
                _trackToChangeName.Add(trackName); 

            //foreach (var transform in copied._timelienActors)  
            //    _timelienActors.Add(transform) ; 
        }

        public void AddActors(GameObject actor)
        {
            if (_timelienActors == null)
            {
                _timelienActors = new List<Transform>();
            }

            _timelienActors.Add(actor.transform);
        }

        public Transform GetObjectWithTrackName(string trackName)
        {
            for (int i = 0; i < _trackToChangeName.Count;i++)
            {
                if (_trackToChangeName[i] == trackName)
                {
                    return _timelienActors[i]  ;     
                }
            }

            throw new Exception("Given track name " + trackName + " is not listed in this action") ;
        }

        public GameObject GetObjectWithIndex(int index)
        {
            if (index >= _timelienActors.Count)
                return null;

            return _timelienActors[(int)index].gameObject;
        }

       

        public List<string> TrackNames => _trackToChangeName;
    }

    [Serializable] 
    public struct TimelineInfo
    {
        [SerializeField]
        private ActionTimelineSettingStruct _actionTimeLineSettingStruct;
        public ActionTimelineSettingStruct GetActionTimeLineSettingStruct => _actionTimeLineSettingStruct;

        [SerializeField]
        private TimelineActorSetupHandler _timelineActorSetupHandler;
        public TimelineActorSetupHandler GetTimelineActorSetupHandler => _timelineActorSetupHandler;
 
         

    }

    [Serializable] 
    public struct SignalEffectBindingStruct
    {
        [SerializeField]
        private SignalType _signalType;

        [SerializeField]
        private List<RuntimeEffectFactorySO> _runtimeEffects ;

        public bool IsSameSignalType(SignalType signal)
        {
            return _signalType == signal; 
        }

        public List<RuntimeEffectFactorySO > RuntimeEffects => _runtimeEffects;

    }
    

    /// <summary>
    /// Need to be treated as Runtime because we constantly modify these value everytime action is performed
    /// </summary>
    [Serializable] 
    public class ActionSignal
    {
        #region Inspector Setting  
 

        [SerializeField]
        private List<SignalEffectBindingStruct> _signalEffectBindings = new List<SignalEffectBindingStruct>() ;

        [SerializeField]
        private ActionTimelineSettingStruct _actionTimelineSetting  ;


        [SerializeField]
        private TimelineActorSetupHandler _timeLineActorSetupPrefab; 

        #endregion


        private Queue<RuntimeEffectFactorySO> _readyEffectQueue = new Queue<RuntimeEffectFactorySO>();

        public ActionSignal(ActionSignal copied)
        {
            if (copied._timeLineActorSetupPrefab == null)
            {
                Debug.LogWarning("TimeLineActorSetUpPrefab hasn't been assigned to  " + copied);
                return;
            }

            for (int i = 0; i < copied._signalEffectBindings.Count; i++ ) 
                _signalEffectBindings.Add(copied._signalEffectBindings[i]);

            Debug.Log("copied : " + copied);
            Debug.Log("copied.actiontimeline setting : " + copied._actionTimelineSetting); 
            _actionTimelineSetting = new ActionTimelineSettingStruct(copied._actionTimelineSetting) ;
            _timeLineActorSetupPrefab = copied._timeLineActorSetupPrefab; 
        }

        /// <summary>
        /// Should be called from DirectorManager only 
        /// </summary>
        /// <param name="signal"></param>
        public void ReceiveSignal(SignalType signal)
        {
            foreach (var signalBinding in _signalEffectBindings)
            {
                if (signalBinding.IsSameSignalType(signal))
                {

                    foreach (var effect in signalBinding.RuntimeEffects) { 
                        _readyEffectQueue.Enqueue(effect);
                    }
                    _signalEffectBindings.Remove(signalBinding);
                    break;
                }
            }


 
        }

        public void SetUpActorsSetting(List<GameObject> actors)
        {
            foreach (var obj in actors)
            {
                _actionTimelineSetting.AddActors(obj);
            }
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

        public bool SignalTerminated()
        {
            return _signalEffectBindings.Count == 0 && _readyEffectQueue.Count == 0; 
        }

        public TimelineActorSetupHandler GetTimelineActorSetupHanlder => _timeLineActorSetupPrefab; 
        public List<RuntimeEffectFactorySO> GetRuntimeEffects()
        {
            List<RuntimeEffectFactorySO> ret = new List<RuntimeEffectFactorySO>(); 

            foreach (var signalBinding in _signalEffectBindings )
            {
                foreach (var effect in signalBinding.RuntimeEffects)
                {
                    ret.Add(effect);
                }
            }
            return ret; 
        }
        #endregion
    }
     
}
