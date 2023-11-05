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
    [RequireComponent(typeof(SpellCasterHandler))]
    public abstract class CombatEntity : MonoBehaviour, IStatusEffectable, ITurnState, IDamagable, IAttackter
    {
        [Header("Right now we manually assign CharacterSheet, TO DO : Make it loaded from the main database")]
        [SerializeField]
        private CharacterSheetSO _characterSheet;

        [SerializeField]
        protected SpellCasterHandler _spellCaster;

        [SerializeField]
        private ItemUserHandler _itemUser;

        [SerializeField]
        private StatusEffectHandler _statusEffectHandler;

        private RuntimeCharacterStatsAccumulator _runtimeCharacterStatsAccumulator;

        [SerializeField]
        private CombatEntityAnimationHandler _combatEntityAnimationHandler;

        private EnergyOverflowHandler _energyOverflowHandler;

        private POPUPNumberTextHandler _dmgOutputPopHanlder;

        private EventBroadcaster _eventBroadcaster;

        private bool _isDead = false;
        private bool _isExhausted = true ; 

        public bool IsDead => _isDead;
        public bool IsExhausted => _isExhausted;

        protected Queue<ActorAction> _actionQueue = new Queue<ActorAction>();

        private EventBroadcaster GetEventBroadcaster()
        {
            if (_eventBroadcaster == null)
            {
                _eventBroadcaster = new EventBroadcaster();
                _eventBroadcaster.OpenChannel<int>("OnAttack");
                _eventBroadcaster.OpenChannel<int>("OnHeal");
                _eventBroadcaster.OpenChannel<int>("OnDamage");
                _eventBroadcaster.OpenChannel<CombatEntity>("OnTakeControl");
                _eventBroadcaster.OpenChannel<CombatEntity>("OnTakeControlLeave");
            }


            return _eventBroadcaster; 

        }

        protected virtual void Awake()
        {
            
            _dmgOutputPopHanlder = new POPUPNumberTextHandler(this); 
            _runtimeCharacterStatsAccumulator = new RuntimeCharacterStatsAccumulator(_characterSheet);
            _energyOverflowHandler = GetComponent<EnergyOverflowHandler>();
            _statusEffectHandler = new StatusEffectHandler(this); 

            if (_spellCaster == null)
            {
                throw new Exception("SpellCaster haven't been assigned (should never use 'GetComponent' for SpellCaster as it would be too slow') ");
            }
        }
 
        #region Turn Handler Methods 
        public abstract IEnumerator GetAction();

        // Take control and leave control should have its own space 
        public virtual IEnumerator TakeControl()
        {
            GetEventBroadcaster().InvokeEvent(this, "OnTakeControl");
            yield return null;
        }
        public virtual IEnumerator TakeControlLeave()
        {
            GetEventBroadcaster().InvokeEvent(this, "OnTakeControlLeave");

            yield return null;
        }

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
         
        public bool ReadyForControl()
        {
            return !_runtimeCharacterStatsAccumulator.IsStunt() && !IsDead && !IsExhausted;
        }

        #endregion
        public ActorAction GetActionRuntimeEffect( )
        {
            if (_actionQueue == null)
                _actionQueue = new Queue<ActorAction>();

            if (_actionQueue.Count == 0)
                return null;


 
            return _actionQueue.Dequeue();
        }

        public void AddActionQueue(ActorAction actorAction)
        {
            _actionQueue.Enqueue(actorAction);
        }


  

        /// <summary>
        /// Invoked before this character perform any action
        /// </summary>
        public IEnumerator OnPerformAction(ActorAction action)
        {
            //Call on perform action of the ActorAction
            yield return action.PreActionPerform();
            
            //Old one, now we need to call Timeline asset instead 
            //var eff = action.GetRuntimeEffect();

            //check if still be able to call the action
            if (ReadyForControl())
            {
                _isExhausted = true;

                yield return action.PerformAction(); 

                yield return action.PostActionPerform();

            }


        }

        public IEnumerator OnPostPerformAction()
        {
            yield return GetStatusEffectHandler().RunStatusEffectExpiredScheme();
        }
        #region GETTER

        public StatusEffectHandler GetStatusEffectHandler()
        {
            return _statusEffectHandler;
        }
        public RuntimeCharacterStatsAccumulator StatsAccumulator => _runtimeCharacterStatsAccumulator;
        public SpellCasterHandler SpellCaster => _spellCaster;
        public ItemUserHandler ItemUser => _itemUser;
        public CharacterSheetSO CharacterSheet => _characterSheet; 

        public CombatEntityAnimationHandler CombatEntityAnimationHandler => _combatEntityAnimationHandler;

        #endregion

        #region InterfaceFunction 
 
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

            _dmgOutputPopHanlder.AccumulateHP(amount);

            StartCoroutine(VisualHeal()); 

        }

        public IEnumerator VisualHeal(string animationTrigger = "No Animation")
        {
            GetEventBroadcaster().InvokeEvent<int>((int)0, "OnHeal");
            yield return null; 
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

            GetEventBroadcaster().InvokeEvent<int>((int)0, "OnDamage");

            yield return new WaitAll(this, _coroutine.ToArray());

            //If done playing animation, visually destroy the character (animation) not game object
            if (IsDead && ! _callingDeadScheme)
            {
                yield return DeadVisualClear();
            }

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

                GetEventBroadcaster().InvokeEvent(inputDmg, "OnAttack");
            }
        }

        public IEnumerator DeadVisualClear()
        {
            yield return _combatEntityAnimationHandler.DestroyVisualMesh();
        }

        /// <summary>
        /// Apply Stun will be called from EnergyOverflowHandler 
        /// </summary>
        public virtual void ApplyStun()
        {
            StatsAccumulator.ApplyStun();
        }

        public virtual void ApplyOverflow()
        {
            GetStatusEffectHandler().StunBreakStatusEffect(this);

        }
        #endregion

        #region SubEvents Methods 
                 
        public void SubOnAttackVisualEvent(UnityAction<int> argc)
        {
            GetEventBroadcaster().SubEvent(argc, "OnAttack");
        }
        public void SubOnDamageVisualEvent(UnityAction<int> argc)
        {
            GetEventBroadcaster().SubEvent(argc, "OnDamage");
        }

        public void SubOnTakeControlEvent(UnityAction<CombatEntity> argc)
        {
            GetEventBroadcaster().SubEvent(argc, "OnTakeControl");
        }

        public void SubOnHealVisualEvent(UnityAction<int> argc)
        {
            GetEventBroadcaster().SubEvent(argc, "OnHeal");
        }

        public void SubOnTakeControlLeaveEvent(UnityAction<CombatEntity> argc)
        {
            GetEventBroadcaster().SubEvent(argc, "OnTakeControlLeave");
        }

        public void UnSubOnAttackVisualEvent(UnityAction<int> argc)
        {
            GetEventBroadcaster().UnSubEvent(argc, "OnAttack");
        }  

        public void UnSubOnDamageVisualEvent(UnityAction<int> argc)
        {
            GetEventBroadcaster().UnSubEvent(argc, "OnDamage");
        }

        public void UnSubOnHealVisualEvent(UnityAction<int> argc)
        {
            GetEventBroadcaster().UnSubEvent(argc, "OnHeal");
        }

        public void UnSubOnTakeControlEvent(UnityAction<CombatEntity> argc)
        {
            GetEventBroadcaster().UnSubEvent(argc, "OnTakeControl");
        }

        public void UnSubOnTakeControlLeaveEvent(UnityAction<CombatEntity> argc)
        {
            GetEventBroadcaster().UnSubEvent(argc, "OnTakeControlLeave");
        }
        #endregion



        /// <summary>
        /// this function is invoked in every character after certain action is applied
        /// </summary>
        /// <returns></returns>





    }
}
