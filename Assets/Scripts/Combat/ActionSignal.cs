using System;
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
    public class ActionSignal
    {
        [SerializeField]
        protected List<int> _keys = new List<int>() ;

        [SerializeField]
        protected List<RuntimeEffectFactorySO> _effects = new List<RuntimeEffectFactorySO>() ; 

        private Dictionary<int, RuntimeEffectFactorySO> runtimeEffects = new Dictionary<int, RuntimeEffectFactorySO>() ;

        [SerializeField]
        private TimelineAsset _asset;
       
        public ActionSignal(ActionSignal copied)
        {
            int i = 0;
            foreach (var key in _keys)
            {
                runtimeEffects.Add(key, _effects[i]);
                i++; 
            }
        }

    }

    
}
