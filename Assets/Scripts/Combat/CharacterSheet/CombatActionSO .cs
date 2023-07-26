using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Vanaring_DepaDemo 
{
    [CreateAssetMenu(fileName= "Combat Ability" , menuName = "ScriptableObject/Combat/CombatAbility" )]
    public class CombatActionSO : ScriptableObject
    {
        [SerializeField]
        protected string _abilityName;

        [SerializeField]
        protected string _desscription;
        
        [SerializeField]
        private RuntimeEffectFactorySO _effect ;

        #region GETTER
        public string AbilityName => _abilityName; 
        public string Desscription => _desscription;
        public RuntimeEffectFactorySO EffectFactory => _effect;

        #endregion
    }
}
