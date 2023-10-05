using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Vanaring 
{
    [Serializable]
    struct ActionTimelineSettingStruct
    {
        [SerializeField]
        private List<string> _trackToChangeName;
        
        private List<object> _timelienActors ; 
        
        public void AddActors(object actor)
        {
            if (_timelienActors == null) 
                _timelienActors = new List<object>() ; 
            
            _timelienActors.Add(actor);
        }
    }
    [Serializable] 
    public class ActionSignal
    {
        [SerializeField]
        protected List<int> _keys = new List<int>() ;

        [SerializeField]
        protected List<RuntimeEffectFactorySO> _effects = new List<RuntimeEffectFactorySO>() ; 

        private Dictionary<int, RuntimeEffectFactorySO> runtimeEffects = new Dictionary<int, RuntimeEffectFactorySO>() ;

        [SerializeField]
        private TimelineAsset _timelineAsset ;

        [SerializeField]
        private ActionTimelineSettingStruct _actionTimelineSetting; 

        [SerializeField] 
        private List<string> _trackToChangedName = new List<string>() ;
        private List<object> _timelineActors = new List<object>() ; 

        public ActionSignal(ActionSignal copied)
        {
            int i = 0;
            foreach (var key in _keys)
            {
                runtimeEffects.Add(key, _effects[i]);
                i++; 
            }
        }

        public IEnumerator SetUpActionTimeLineSetting(List<object> actors)
        {
            for (int i = 0; i < _trackToChangedName.Count; i++)
            {
                _timelineActors.Add(actors[i]); 
            }
            yield return null; 
        }

        public void SimulateEnergyModifier(CombatEntity target)
        {
            foreach (var factory in _effects)
            {
                factory.SimulateEnergyModifier(target) ; 
            }
        }

    }

    
}
