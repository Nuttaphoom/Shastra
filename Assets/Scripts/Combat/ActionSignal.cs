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
        private TimelineAsset _timelineAsset;

        [SerializeField]
        protected List<int> _keys = new List<int>() ;

        [SerializeField]
        protected List<RuntimeEffectFactorySO> _effects = new List<RuntimeEffectFactorySO>() ; 

        private Dictionary<int, RuntimeEffectFactorySO> _runtimeEffectWithSignal= new Dictionary<int, RuntimeEffectFactorySO>() ;

        [SerializeField]
        private ActionTimelineSettingStruct _actionTimelineSetting; 

        private List<CombatEntity> _targets = new List<CombatEntity>(); 
        public ActionSignal(ActionSignal copied, List<CombatEntity> targets)
        {
            int i = 0;
            foreach (var key in _keys)
            {
                _runtimeEffectWithSignal.Add(key, _effects[i]);
                i++; 
            }

             
        }

        public void ReceiveSignal(int signal)
        {

        }

        public IEnumerator InitializeScheme()
        {
            yield return null; 
        }

        public IEnumerator SetUpActionTimeLineSetting(List<object> actors)
        {
             
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
