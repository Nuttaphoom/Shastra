
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


namespace Vanaring
{

    [CreateAssetMenu(fileName = "AilmentAppilerRuntimeEffectFactorySO ", menuName = "ScriptableObject/RuntimeEffect/AilmentApplier/AilmentAppilerRuntimeEffectFactorySO ")]
    public class AilmentAppilerRuntimeEffectFactorySO : RuntimeEffectFactorySO
    {
        [SerializeField]
        private AilmentLocator.AilmentType _ailmentType;
        [SerializeField]
        private int _initTTL = 1;
        public override RuntimeEffect Factorize(List<CombatEntity> targets)
        {
            AilmentAppilerRuntimeEffect retEffect = new AilmentAppilerRuntimeEffect(_ailmentType, _initTTL );
            foreach (CombatEntity target in targets)
            {
                retEffect.AssignTarget(target);
            }

            return retEffect;
        }

        public override void SimulateEnergyModifier(CombatEntity target)
        {

        }
    }


    public class AilmentAppilerRuntimeEffect : RuntimeEffect
    {
        private AilmentLocator.AilmentType _ailmentType;
        private int _initTTL = 1; 
        public AilmentAppilerRuntimeEffect( AilmentLocator.AilmentType type, int ttl  )  
        {
            _ailmentType = type;
            _initTTL = ttl; 
        }

        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity owner)
        {
            foreach (CombatEntity target in _targets)
            {
                if (target is CombatEntity)
                {
                   target.ApplyAilment( AilmentLocator.Instance.GetAilmentObject(_ailmentType).FactorizeAilment(target, _initTTL));//  target.ApplyStun();
                }
            }

            yield return null;
        }


         


    }


}