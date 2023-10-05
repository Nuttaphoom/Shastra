using Cinemachine;
using CustomYieldInstructions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Numerics;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static Cinemachine.CinemachineTargetGroup;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;

namespace Vanaring
{
    public abstract class CombatEntity : MonoBehaviour, IStatusEffectable, ITurnState, IDamagable, IAttackter
    {
        [Header("Right now we manually assign CharacterSheet, TO DO : Make it loaded from the main database")]
        [SerializeField]
        private CharacterSheetSO _characterSheet;

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


        [SerializeField]
        private DamageOutputPopupHandler _dmgOutputPopHanlder; 

        private bool _isDead = false;
        private bool _isExhausted = false; 

        public bool IsDead => _isDead;

        public bool IsExhausted => _isExhausted;

        protected Queue<IActorAction> _actionQueue = new Queue<IActorAction>(); 
        protected virtual void Awake()
        {
            _dmgOutputPopHanlder = new DamageOutputPopupHandler(_dmgOutputPopHanlder, this); 
            _runtimeCharacterStatsAccumulator = new RuntimeCharacterStatsAccumulator(_characterSheet);
            _energyOverflowHandler = GetComponent<EnergyOverflowHandler>();
            _statusEffectHandler = new StatusEffectHandler(this); 

            if (_spellCaster == null)
            {
                throw new Exception("SpellCaster haven't been assigned (should never use 'GetComponent' for SpellCaster as it would be too slow') ");
            }


        }

        #region Turn Handler Methods 
        public virtual IEnumerator TurnEnter()
        {
            if (_statusEffectHandler == null)
                throw new Exception("Status Effect Handler hasn't never been init");

            yield return (_statusEffectHandler.ExecuteStatusRuntimeEffectCoroutine());

        }

        public virtual IEnumerator TurnLeave()
        {
            _isExhausted = false; 

            yield return _runtimeCharacterStatsAccumulator.ResetTemporaryIncreasedValue();

            yield return _statusEffectHandler.RunStatusEffectExpiredScheme();
        }

        public abstract IEnumerator GetAction();

        // Take control and leave control should have its own space 
        public abstract IEnumerator TakeControl();
        public abstract IEnumerator TakeControlLeave();
         
        //is okay 
        public bool ReadyForControl()
        {
            return !_runtimeCharacterStatsAccumulator.IsStunt() && !IsDead && !IsExhausted;
        }


        #endregion
        public IActorAction GetActionRuntimeEffect(bool peek = false)
        {
            if (_actionQueue == null)
                _actionQueue = new Queue<IActorAction>();

            if (_actionQueue.Count == 0)
                return null;

            if (peek)
                return _actionQueue.Peek();

            return _actionQueue.Dequeue();
        }

        public void AddActionQueue(IActorAction actorAction)
        {
            _actionQueue.Enqueue(actorAction);
        }


        public StatusEffectHandler GetStatusEffectHandler()
        {
            return _statusEffectHandler;
        }

        /// <summary>
        /// Invoked before this character perform any action
        /// </summary>
        public IEnumerator OnPerformAction(IActorAction action)
        {
            //Call on perform action of the ActorAction
            yield return action.PreActionPerform();
            
            //Old one, now we need to call Timeline asset instead 
            //var eff = action.GetRuntimeEffect();

            //check if still be able to call the action
            if (ReadyForControl())
            {
                _isExhausted = true;

                //yield return eff.ExecuteRuntimeCoroutine(this);

                //yield return eff.OnExecuteRuntimeDone(this);

                yield return action.PostActionPerform();

            }


        }

        public IEnumerator OnPostPerformAction()
        {
            yield return GetStatusEffectHandler().RunStatusEffectExpiredScheme();
        }
        #region GETTER
        public RuntimeCharacterStatsAccumulator StatsAccumulator => _runtimeCharacterStatsAccumulator;
        public SpellCasterHandler SpellCaster => _spellCaster;
        public ItemUserHandler ItemUser => _itemUser;
        public CharacterSheetSO CharacterSheet => _characterSheet; 

        public CombatEntityAnimationHandler CombatEntityAnimationHandler => _combatEntityAnimationHandler;

        #endregion

        #region InterfaceFunction 
        private UnityAction<int> _OnUpdateVisualDMG ;
        private UnityAction<int> _OnUpdateVisualDMGEnd ;
        public void LogicHurt(CombatEntity attacker, int inputdmg)
        {
            float trueDmg = inputdmg;
            //Do some math here
            trueDmg = -trueDmg;

            _runtimeCharacterStatsAccumulator.ModifyHPStat(trueDmg);

            ColorfulLogger.LogWithColor(gameObject.name + "is hit with " + trueDmg + " remaining HP : " + _runtimeCharacterStatsAccumulator.GetHPAmount(), Color.red);

            _dmgOutputPopHanlder.AccumulateDMG(inputdmg); 

            if (_runtimeCharacterStatsAccumulator.GetHPAmount() <= 0)
            {
                _isDead = true;
            }


        }
        public void LogicHeal(int amount)
        {    
            int increasedAmoubnt = StatsAccumulator.ModifyHPStat(amount);
            _dmgOutputPopHanlder.AccumulateHP(increasedAmoubnt); 
        }

        public IEnumerator VisualHeal(string animationTrigger = "No Animation")
        {
            _OnUpdateVisualDMG?.Invoke(0);

            yield return new WaitForSeconds(2.0f); 

            _OnUpdateVisualDMGEnd?.Invoke(0);

        }
        public IEnumerator VisualHurt(CombatEntity attacker , string animationTrigger = "No Animation" )
        {
            bool _callingDeadScheme = false;

            List<IEnumerator> _coroutine = new List<IEnumerator>();

            if (animationTrigger != "No Animation")
            {
                if (IsDead)
                {
                    _coroutine.Add(DeadVisualClear());
                    _callingDeadScheme = true;
                }
                _coroutine.Add(_combatEntityAnimationHandler.PlayTriggerAnimation(animationTrigger));

            }

            _OnUpdateVisualDMG?.Invoke(0);

            yield return new WaitAll(this, _coroutine.ToArray());

            //If done playing animation, visually destroy the character (animation) not game object
            if (IsDead && ! _callingDeadScheme)
            {
                yield return DeadVisualClear();
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

        /// <summary>
        /// this function is invoked in every character after certain action is applied
        /// </summary>
        /// <returns></returns>
   

        public IEnumerator DeadVisualClear()
        {
            yield return _combatEntityAnimationHandler.DestroyVisualMesh();
        }


        #endregion
    }
}
