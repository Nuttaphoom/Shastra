using CustomYieldInstructions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEngine.EventSystems.EventTrigger;


namespace Vanaring
{
    [CreateAssetMenu(fileName = "AttackRuntimeEffectFactory", menuName = "ScriptableObject/RuntimeEffect/AttackRuntimeEffectFactory")]
    public class AttackRuntimeEffectFactory : RuntimeEffectFactorySO
    {
        [SerializeField]
        private EDamageScaling _damagScaling;

        [SerializeField]
        private int realDmg = -1;

        public override RuntimeEffect Factorize(List<CombatEntity> targets)
        {
            return SetUpRuntimeEffect(new AttackRuntimeEffect(_damagScaling, realDmg   ), targets); 
        }

        public override void SimulateEnergyModifier(CombatEntity combatEntity)
        {
            //combatEntity.SpellCaster.Simulate(RuntimeMangicalEnergy.EnergySide.LightEnergy, 100, null); 
        }
    }

    public class AttackRuntimeEffect : RuntimeEffect
    {
        private EDamageScaling _damagScaling;
        private int _realDmg;
        public AttackRuntimeEffect(EDamageScaling scaling, int realDmg     )
        {
            _damagScaling  = scaling;
            _realDmg = realDmg; 

        }

        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {
            if (_realDmg > 0)
            {
                throw new Exception("Direct dmg, without caster, is not function, TODO "); 
            }

            //Command caster to attack enemy 
            if (caster == null)
                throw new Exception("Caster can not be null");
 
            yield return caster.LogicAttack(_targets, _damagScaling ) ;

            ////2 Visual 
            List<IEnumerator> coroutines = new List<IEnumerator>();
            foreach (var target in _targets)
            {
                coroutines.Add(target.VisualHurt(caster, "Hurt") ) ;
            }

            yield return new WaitAll(caster,coroutines.ToArray());

            


        }

       








    }
}