using CustomYieldInstructions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using static Cinemachine.CinemachineTargetGroup;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;

namespace Vanaring_DepaDemo
{
    [RequireComponent(typeof(BaseEntityBrain))]
    public class CombatEntity : MonoBehaviour, IStatusEffectable, ITurnState, IDamagable, IAttackter
    {
        [Header("Right now we manually assign CharacterSheet, TO DO : Make it loaded from the main database")]
        [SerializeField]
        private CharacterSheetSO _characterSheet ;

        private BaseEntityBrain _baseEntityBrain ;

        [SerializeField]
        private SpellCasterHandler _spellCaster ;

        [SerializeField]
        private ItemUserHandler _itemUser;

        private StatusEffectHandler _statusEffectHandler ;  
        
        private RuntimeCharacterStatsAccumulator _runtimeCharacterStatsAccumulator ;

        [SerializeField]
        private CombatEntityAnimationHandler _combatEntityAnimationHandler ;

        private EnergyOverflowHandler _energyOverflowHandler ;

        private bool _isDead = false;

        public bool IsDead => _isDead; 

        public void Awake()
        {
            _runtimeCharacterStatsAccumulator = new RuntimeCharacterStatsAccumulator(_characterSheet) ;
            _statusEffectHandler = new StatusEffectHandler(this) ;
            _energyOverflowHandler = GetComponent<EnergyOverflowHandler>(); 

            if (! TryGetComponent(out _baseEntityBrain))
            {
                throw new Exception("BaseEntityBrain haven't been assigned"); 
            }

            if (_spellCaster == null)
            {
                throw new Exception("SpellCaster haven't been assigned (should never use 'GetComponent' for SpellCaster as it would be too slow') ");
            }

            if (_itemUser == null)
            {
                throw new Exception("ItemUser haven't been assigned (should never use 'GetComponent' for SpellCaster as it would be too slow') ");
            }
        }

    #region Turn Handler Methods 
        public IEnumerator TurnEnter()
        {
            if (_baseEntityBrain == null)
                throw new Exception("Base Entity Brain of " + gameObject.name + " hasn't been assgined") ;

            if (_statusEffectHandler == null)
                throw new Exception("Status Effect Handler hasn't never been init");
            
            yield return (_statusEffectHandler.ExecuteStatusRuntimeEffectCoroutine()) ;

            yield return _baseEntityBrain.TurnEnter(); 
        }

        public  IEnumerator TurnLeave()
        {
            if (_baseEntityBrain == null)
                throw new Exception("Base Entity Brain of " + gameObject.name + " hasn't been assgined");

            yield return _runtimeCharacterStatsAccumulator.ResetTemporaryIncreasedValue(); 
           
            yield return _baseEntityBrain.TurnLeave();
        }

        public IEnumerator GetAction( )
        {
            if (_baseEntityBrain == null)
                throw new Exception("Base Entity Brain of " + gameObject.name + " hasn't been assgined");

            IEnumerator coroutine = (_baseEntityBrain.GetAction());

            while (coroutine.MoveNext())
            {
                //No need to return null 
                if (coroutine.Current != null && coroutine.Current is RuntimeEffect)
                {
                    //Need to cast to RuntimeEffect before returning            
                    yield return (RuntimeEffect)coroutine.Current;
                }
                else 
                    yield return coroutine.Current ;
            }
        }

        public IEnumerator TakeControl()
        {
            yield return _baseEntityBrain.TakeControl(); 
        }

        public IEnumerator LeaveControl()
        {
            yield return _baseEntityBrain.TakeControlLeave(); 
        }
 
        public bool ReadyForControl()
        {
            return ! _runtimeCharacterStatsAccumulator.IsStunt() && ! IsDead  ;
        }

        public StatusEffectHandler GetStatusEffectHandler()
        {
            
            return _statusEffectHandler ;
        }
        #endregion

    #region GETTER
        public RuntimeCharacterStatsAccumulator StatsAccumulator => _runtimeCharacterStatsAccumulator;
        public SpellCasterHandler SpellCaster => _spellCaster;
        public ItemUserHandler ItemUser => _itemUser;

        public CombatEntityAnimationHandler CombatEntityAnimationHandler => _combatEntityAnimationHandler; 

        #endregion

        #region InterfaceFunction 

        public void LogicHurt(int inputdmg)
        {
            float trueDmg = inputdmg;

            //Do some math here
            trueDmg =  -trueDmg;

           _runtimeCharacterStatsAccumulator.ModifyHPStat(trueDmg);

            ColorfulLogger.LogWithColor(gameObject.name + "is hit with " + trueDmg + " remaining HP : " + _runtimeCharacterStatsAccumulator.GetHPAmount(), Color.red); 
       
            if (_runtimeCharacterStatsAccumulator.GetHPAmount() <= 0)
            {
                _isDead = true; 
            }
        }

        public IEnumerator VisualHurt(string animationTrigger = "Hurt")
        {
            //Display DMG Text here

            //Slow down time? 
            
            yield return _combatEntityAnimationHandler.PlayTriggerAnimation(animationTrigger); 
        
            //If done playing animation, visually destroy the character (animation) not game object
            if (IsDead)
            {

            }
        }


        //Receive animation info and play it accordingly 
        public IEnumerator Attack(List<CombatEntity> targets,float multiplier , ActionAnimationInfo animationinfo)
        {
            //Prepare for status effect  
            yield return _statusEffectHandler.ExecuteAttackStatusRuntimeEffectCoroutine(); 

            //1.) Do apply dmg 
            int inputDmg = (int) (multiplier * StatsAccumulator.GetATKAmount()) ;
            foreach (CombatEntity target in targets) {
                    target.LogicHurt(inputDmg);
            }

            List<IEnumerator> coroutines = new List<IEnumerator>() ;

            //2.) play animation
            foreach (CombatEntity target in targets)
            {
                CombatEntity entity = target;
                coroutines.Add(_combatEntityAnimationHandler.PlayVFXActionAnimation<string>(animationinfo.TargetVfxEntity , entity, entity.VisualHurt, "Hurt"));

            }

            coroutines.Add(_combatEntityAnimationHandler.PlayActionAnimation(animationinfo));

            
            yield return new WaitAll(this,coroutines.ToArray() );
        }

        
        #endregion
    }
}
