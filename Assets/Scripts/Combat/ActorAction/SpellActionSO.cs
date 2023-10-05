using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring 
{ 
    [CreateAssetMenu(fileName = "Spell Ability", menuName = "ScriptableObject/Combat/SpellAbility")]
    public class SpellActionSO : ActorActionFactory
    {
        [Header("===Require Energy amount before casting===")]
        [SerializeField]
        private EnergyModifierData _requiredEnergy;

        public EnergyModifierData RequiredEnergy => _requiredEnergy;

        public SpellAbilityRuntime Factorize(CombatEntity caster)
        {
            return new SpellAbilityRuntime(RequiredEnergy, caster, _targetSelector, _actionSignal); 
        } 

    }

    public class SpellAbilityRuntime : IActorAction
    {       
        private CombatEntity _caster; 

        private EnergyModifierData _energyModifier;

        private List<CombatEntity> _targets;

        private TargetSelector _targetSelector; 

        private ActionSignal _actionSignal ;

        public SpellAbilityRuntime(EnergyModifierData energyModifier, CombatEntity caster, TargetSelector targetSelector, ActionSignal actionSignal)
        {
            _energyModifier = energyModifier ;
            _caster = caster ;
            _targetSelector = targetSelector ;
            _actionSignal = new ActionSignal(actionSignal) ;
        }

        public IEnumerator PreActionPerform()
        {
            yield return null ; 
        }

        public IEnumerator PostActionPerform()
        {
            _caster.SpellCaster.ModifyEnergy(_caster, _energyModifier.Side, _energyModifier.Amount);
            yield return null; 
        }

        public void SetActionTarget(List<CombatEntity> targets)
        {
            _targets = new List<CombatEntity>(); 
            foreach (var entity in targets)
            {
                _targets.Add(entity);
            }
        }

        public TargetSelector GetTargetSelector()
        {
            return _targetSelector; 
        }

        public IEnumerator Simulate(CombatEntity target)
        {
            throw new NotImplementedException() ;
            yield return null ; 
        }

        public IEnumerator PerformAction()
        {
            //Set up 
            SetUpTimeLineActorSetting();

            //Play Timeline in DirectorManager and register signal


            //Play runtimeeffect when signal received  

            RuntimeEffectFactorySO factory ; 

            while (_actionSignal.SignalTerminated())
            {
                if ( ( factory = _actionSignal.GetReadyEffect() ) != null) {
                    RuntimeEffect effect =  factory.Factorize(_targets) ;

                    yield return effect.ExecuteRuntimeCoroutine(_caster) ;
                    yield return effect.OnExecuteRuntimeDone(_caster) ;
                }

                yield return new WaitForEndOfFrame() ; 
            }
            
            throw new NotImplementedException("The imeplementation is not finished!") ;  
        }

        private void SetUpTimeLineActorSetting()
        {
            List<object> actors = new List<object>();

            actors.Add(_caster); 
            foreach (var entity in _targets)
            {
                actors.Add(entity);
            }

            _actionSignal.SetUpActorsSetting(actors); 
        }
    }


}
