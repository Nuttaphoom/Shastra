using Cinemachine;
using CustomYieldInstructions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Numerics;
using System.Runtime.InteropServices;
using Unity.PlasticSCM.Editor.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static Cinemachine.CinemachineTargetGroup;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;

namespace Vanaring
{
    [RequireComponent(typeof(SpellCasterHandler))]
    public abstract class CombatEntity : MonoBehaviour, ITurnState, IDamagable, IAttackter
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

        [SerializeField]
        protected AilmentHandler _ailmentHandler; 


        /// <summary>
        /// TODO : these IsDead, Is.... variables should be removed
        /// </summary>
        private bool _isDead = false ;
        private bool _isExhausted = true ;

        public bool IsDead => _isDead;
        public bool IsExhausted => _isExhausted;

        protected Queue<ActorAction> _actionQueue = new Queue<ActorAction>();

        #region GetEventBroadcaster Methods 

        private EventBroadcaster _eventBroadcaster;
        private EventBroadcaster GetEventBroadcaster()
        {
            if (_eventBroadcaster == null)
            {
                _eventBroadcaster = new EventBroadcaster();

                _eventBroadcaster.OpenChannel<int>("OnHeal");
                _eventBroadcaster.OpenChannel<int>("OnDamage");
                _eventBroadcaster.OpenChannel<CombatEntity>("OnTakeControl");
                _eventBroadcaster.OpenChannel<CombatEntity>("OnTakeControlLeave");
                _eventBroadcaster.OpenChannel<EntityActionPair>("OnPerformAction");
            }

            return _eventBroadcaster;
        }

        public void SubOnAilmentRecoverEventChannel(UnityAction<EntityAilmentEffectPair> func)
        {
            _ailmentHandler.SubOnAilmentRecoverEventChannel(func);
        }
 
        public void SubOnAilmentControlEventChannel(UnityAction<EntityAilmentEffectPair> func)
        {
            _ailmentHandler.SubOnAilmentControlEventChannel(func);
        }

        public void SubOnStatusEffectApplied(UnityAction<EntityStatusEffectPair> func) 
        {
            _statusEffectHandler.SubOnStatusEffectApplied(func); 
        }
  
        public void SubOnDamageVisualEvent(UnityAction<int> argc)
        {
            GetEventBroadcaster().SubEvent(argc, "OnDamage");
        }

        public void SubOnPerformAction(UnityAction<EntityActionPair> argc)
        {
            GetEventBroadcaster().SubEvent(argc, "OnPerformAction");
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
        public void SubOnOnAilmentAppliedEventChannel(UnityAction<EntityAilmentApplierEffect> func)
        {
            _ailmentHandler.SubOnOnAilmentAppliedEventChannel(func);

        }

        public void UnSubOnStatusEffectApplied(UnityAction<EntityStatusEffectPair> func)
        {
            _statusEffectHandler.UnSubOnStatusEffectApplied(func);
        }
    
        public void UnSubOnPerformAction(UnityAction<EntityActionPair> argc)
        {
            GetEventBroadcaster().UnSubEvent(argc, "OnPerformAction");
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
      
        public void UnSubOnAilmentControlEventChannel(UnityAction<EntityAilmentEffectPair> func)
        {
            _ailmentHandler.UnSubOnAilmentControlEventChannel(func); 
        }

        public void UnSubOnAilmentRecoverEventChannel(UnityAction<EntityAilmentEffectPair> func)
        {
            _ailmentHandler.UnSubOnAilmentRecoverEventChannel(func);
        }

     
        public void UnSubOnOnAilmentAppliedEventChannel(UnityAction<EntityAilmentApplierEffect> func)
        {
            _ailmentHandler.UnSubOnOnAilmentAppliedEventChannel(func);

        }

        #endregion

        protected virtual void Awake()
        {
            _ailmentHandler = new AilmentHandler(this); 
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
        public abstract IEnumerator InitializeEntityIntoCombat(); 

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
            _isExhausted = false;

            if (_statusEffectHandler == null)
                throw new Exception("Status Effect Handler hasn't never been init");

            yield return (_statusEffectHandler.ExecuteStatusRuntimeEffectCoroutine());

            yield return _ailmentHandler.CheckForExpiration();

            _ailmentHandler.ProgressAlimentTTL();

        }

        public virtual IEnumerator TurnLeave()
        {
            _isExhausted = false; 

            yield return _runtimeCharacterStatsAccumulator.ResetTemporaryIncreasedValue();

            yield return _statusEffectHandler.RunStatusEffectExpiredScheme();
        }
         
        public bool ReadyForControl()
        {
            return  !IsDead && !IsExhausted;
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

                GetEventBroadcaster().InvokeEvent<EntityActionPair>(new EntityActionPair() { Actor = this, PerformedAction = action } ,"OnPerformAction");

                yield return action.PerformAction(); 

                yield return action.PostActionPerform();

            }
        }

        public IEnumerator OnPostPerformAction()
        {
    
            //2. check status effect 
            yield return _statusEffectHandler.RunStatusEffectExpiredScheme();
        }

        #region GETTER


        public EnergyOverflowHandler OverflowHandler => _energyOverflowHandler ;
        public RuntimeCharacterStatsAccumulator StatsAccumulator => _runtimeCharacterStatsAccumulator;
        public SpellCasterHandler SpellCaster => _spellCaster;
        public ItemUserHandler ItemUser => _itemUser;
        public CharacterSheetSO CharacterSheet => _characterSheet; 

        public CombatEntityAnimationHandler CombatEntityAnimationHandler => _combatEntityAnimationHandler;

        #endregion

        #region Combat Methods 
        public IEnumerator ApplyNewEffect(StatusRuntimeEffectFactorySO  statusEffect, StatusEffectApplierRuntimeEffect applierFactory, CombatEntity applier)
        {
            yield return _statusEffectHandler.ApplyNewEffect(statusEffect,applierFactory, applier); 
        }

        public void LogicHurt(CombatEntity attacker, int inputdmg)
        {
            float trueDmg = inputdmg;
            //Do some math here
            trueDmg = -trueDmg;

            _runtimeCharacterStatsAccumulator.ModifyHPStat(trueDmg);
    
            _dmgOutputPopHanlder.AccumulateDMG(inputdmg); 

            if (_runtimeCharacterStatsAccumulator.GetHPAmount() <= 0)
            {
                _isDead = true;
            }

            StartCoroutine(VisualHurt( "Hurt")) ;

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
        public IEnumerator VisualHurt(string animationTrigger = "No Animation" )
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
            }

        }

        public IEnumerator DeadVisualClear()
        {
            yield return _combatEntityAnimationHandler.DestroyVisualMesh();
        }

 

        public IEnumerator ApplyAilment(Ailment ailment)
        {
            yield return _ailmentHandler.LogicApplyAilment(ailment);

        }

        public virtual void ApplyOverflow()
        {
            _statusEffectHandler.StunBreakStatusEffect(this);
        }
        #endregion
    }
}
