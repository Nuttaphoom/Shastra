using CustomYieldInstructions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
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
        private CharacterSheetSO _characterSheet;

        private BaseEntityBrain _baseEntityBrain;

        [SerializeField]
        private SpellCasterHandler _spellCaster;

        [SerializeField]
        private ItemUserHandler _itemUser;

        [SerializeField]
        private StatusEffectHandler _statusEffectHandler;

        private RuntimeCharacterStatsAccumulator _runtimeCharacterStatsAccumulator;

        [SerializeField]
        private CombatEntityAnimationHandler _combatEntityAnimationHandler;

        private EnergyOverflowHandler _energyOverflowHandler;



        private bool _isDead = false;

        public bool IsDead => _isDead;
        public void Awake()
        {
            _runtimeCharacterStatsAccumulator = new RuntimeCharacterStatsAccumulator(_characterSheet);
            _energyOverflowHandler = GetComponent<EnergyOverflowHandler>();

            if (!TryGetComponent(out _baseEntityBrain))
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
                throw new Exception("Base Entity Brain of " + gameObject.name + " hasn't been assgined");

            if (_statusEffectHandler == null)
                throw new Exception("Status Effect Handler hasn't never been init");

            yield return (_statusEffectHandler.ExecuteStatusRuntimeEffectCoroutine());

            yield return _baseEntityBrain.TurnEnter();

            //If the entity is clear for control, make it idle 
            if (ReadyForControl())
            {
                _combatEntityAnimationHandler.PlayTriggerAnimation("Idle");
            }
        }

        public IEnumerator TurnLeave()
        {
            if (_baseEntityBrain == null)
                throw new Exception("Base Entity Brain of " + gameObject.name + " hasn't been assgined");

            yield return _runtimeCharacterStatsAccumulator.ResetTemporaryIncreasedValue();

            yield return _statusEffectHandler.RunStatusEffectExpiredScheme();

            yield return _baseEntityBrain.TurnLeave();
        }

        public IEnumerator GetAction()
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
                    yield return coroutine.Current;
            }
        }


        public IEnumerator TakeControl()
        {
            yield return _baseEntityBrain.TakeControl();
        }

        public IEnumerator TakeControlSoftLeave()
        {
            yield return _baseEntityBrain.TakeControlSoftLeave();
        }
        public IEnumerator LeaveControl()
        {
            yield return _baseEntityBrain.TakeControlLeave();
        }

        public bool ReadyForControl()
        {
            return !_runtimeCharacterStatsAccumulator.IsStunt() && !IsDead;
        }

        public StatusEffectHandler GetStatusEffectHandler()
        {
            return _statusEffectHandler;
        }


        #endregion

        #region GETTER
        public RuntimeCharacterStatsAccumulator StatsAccumulator => _runtimeCharacterStatsAccumulator;
        public SpellCasterHandler SpellCaster => _spellCaster;
        public ItemUserHandler ItemUser => _itemUser;

        public CombatEntityAnimationHandler CombatEntityAnimationHandler => _combatEntityAnimationHandler;

        #endregion

        #region InterfaceFunction 
        private UnityAction<int> _OnUpdateVisualDMG;
        private UnityAction<int> _OnUpdateVisualDMGEnd;
        public void LogicHurt(CombatEntity attacker, int inputdmg)
        {

            float trueDmg = inputdmg;

            //Do some math here
            trueDmg = -trueDmg;

            _runtimeCharacterStatsAccumulator.ModifyHPStat(trueDmg);

            ColorfulLogger.LogWithColor(gameObject.name + "is hit with " + trueDmg + " remaining HP : " + _runtimeCharacterStatsAccumulator.GetHPAmount(), Color.red);

            if (_runtimeCharacterStatsAccumulator.GetHPAmount() <= 0)
            {
                _isDead = true;
            }
        }

        public IEnumerator VisualHurt(CombatEntity attacker, string animationTrigger = "No Animation")
        {
            //Display DMG Text here

            //Slow down time? 

            List<IEnumerator> _coroutine = new List<IEnumerator>();

            if (animationTrigger != "No Animation")
            {
                if (IsDead)
                {
                    _coroutine.Add(_combatEntityAnimationHandler.DestroyVisualMesh());
                }
                _coroutine.Add(_combatEntityAnimationHandler.PlayTriggerAnimation(animationTrigger));

            }

            _OnUpdateVisualDMG?.Invoke(0);

            yield return new WaitAll(this, _coroutine.ToArray());

            //If done playing animation, visually destroy the character (animation) not game object
            if (IsDead)
            {
                yield return _combatEntityAnimationHandler.DestroyVisualMesh();
            }

            _OnUpdateVisualDMGEnd?.Invoke(0);

            yield return null;

        }

        //Receive animation info and play it accordingly 
        public IEnumerator LogicAttack(List<CombatEntity> targets, EDamageScaling scaling)
        {
            //Prepare for status effect  
            yield return _statusEffectHandler.ExecuteAttackStatusRuntimeEffectCoroutine();


            //1.) Do apply dmg 
            int inputDmg = VanaringMathConst.GetATKWithScaling(scaling, StatsAccumulator.GetATKAmount());
            foreach (CombatEntity target in targets)
            {
                target.LogicHurt(this, inputDmg);
            }


            //2.) the VisualAttack should be called somewhere else or the hp bar wouldn't be updated

        }


        public void SubOnDamageVisualEvent(UnityAction<int> argc)
        {
            _OnUpdateVisualDMG += argc;
        }

        public void UnSubOnDamageVisualEvent(UnityAction<int> argc)
        {
            _OnUpdateVisualDMG -= argc;
        }

        public void SubOnDamageVisualEventEnd(UnityAction<int> argc)
        {
            _OnUpdateVisualDMGEnd += argc;
        }

        public void UnSubOnDamageVisualEventEnd(UnityAction<int> argc)
        {
            _OnUpdateVisualDMGEnd -= argc;
        }


        #endregion
    }
}
