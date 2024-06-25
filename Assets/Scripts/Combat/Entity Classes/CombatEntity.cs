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
using Kryz.CharacterStats;
using Vanaring.Assets.Scripts.Combat.Utilities;
using Vanaring.Assets.Scripts.Utilities.StringConstant;
using static PixelCrushers.DialogueSystem.ActOnDialogueEvent;

namespace Vanaring
{
    [RequireComponent(typeof(SpellCasterHandler))]
    public abstract class CombatEntity : MonoBehaviour, ITurnState, IDamagable, IAttackter, ICombatRequireLoadData
    {
        //[Header("Right now we manually assign CharacterSheet, TO DO : Make it loaded from the main database")]
        [SerializeField]
        protected CombatCharacterSheetSO _characterSheet;

        [SerializeField]
        protected SpellCasterHandler _spellCaster;

        [SerializeField]
        private ItemUserHandler _itemUser;

        [SerializeField] 
        protected BreakTriggerHandler _breakTriggerHandler ;

        public BreakTriggerHandler BreakTriggerHandler {
            get
            {
                return _breakTriggerHandler; 
            }
        } 

        private StatusEffectHandler _statusEffectHandler;

        protected RuntimeCharacterStatsAccumulator _runtimeCharacterStatsAccumulator;

        [SerializeField]
        private CombatEntityAnimationHandler _combatEntityAnimationHandler;

        private EnergyOverflowHandler _energyOverflowHandler;

        protected AilmentHandler _ailmentHandler;

        protected CombatEntityActionHandler _combatEntityActionHandler; 

        /// <summary>
        /// TODO : these IsDead, Is.... variables should be removed
        /// </summary>
        private bool _isDead = false ;
        private bool _isExhausted = true ;


        private bool _forceRelieve = false; 
     


        #region EventBroadcaster Methods 

        private EventBroadcaster _eventBroadcaster;
        private EventBroadcaster GetEventBroadcaster()
        {
            if (_eventBroadcaster == null)
            {
                _eventBroadcaster = new EventBroadcaster();

                _eventBroadcaster.OpenChannel<int>("OnHeal");
                _eventBroadcaster.OpenChannel<int>("OnDamage");
                _eventBroadcaster.OpenChannel<Null>("OnDodgeAttack");
                _eventBroadcaster.OpenChannel<CombatEntity>("OnTakeControl");
                _eventBroadcaster.OpenChannel<CombatEntity>("OnTakeControlLeave");
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
        public void SubOnDodgeAttackVisualEvent(UnityAction<Null> argc)
        {
            GetEventBroadcaster().SubEvent(argc, "OnDodgeAttack");
        }

        public void SubOnPerformAction(UnityAction<EntityActionPair> argc)
        {
            ActionHandler.SubOnPerformAction(argc);
        }

        public void SubOnPostPerformAction(UnityAction<EntityActionPair> argc)
        {
            ActionHandler.SubOnPostPerformAction(argc);
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
        public void UnSubOnPostPerformAction(UnityAction<EntityActionPair> argc)
        {
            ActionHandler.UnSubOnPostPerformAction(argc);
        }
        public void UnSubOnStatusEffectApplied(UnityAction<EntityStatusEffectPair> func)
        {
            _statusEffectHandler.UnSubOnStatusEffectApplied(func);
        }
    
        public void UnSubOnPerformAction(UnityAction<EntityActionPair> argc)
        {
           ActionHandler.UnSubOnPerformAction(argc);
        }            
        public void UnSubOnDodgeAttackVisualEvent(UnityAction<Null> argc)
        {
            
            GetEventBroadcaster().UnSubEvent(argc, "OnDodgeAttack");
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

        #region Abstract Methods 
        public abstract IEnumerator LoadDataFromDatabase();
        public abstract IEnumerator GetAction();
        #endregion

        #region Turn Handler Methods 
        /// <summary>
        /// Controlable load data from Database here 
        /// Enemy will display spawning effect here 
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator InitializeEntityIntoCombat()
        {
            _ailmentHandler = new AilmentHandler(this);
            POPUPNumberTextHandler _dmgOutputPopHanlder = new POPUPNumberTextHandler(this);
            _energyOverflowHandler = GetComponent<EnergyOverflowHandler>();
            _statusEffectHandler = new StatusEffectHandler(this);
            _combatEntityActionHandler = new CombatEntityActionHandler(this); 

            _breakTriggerHandler.Initialize(this);

            if (_spellCaster == null)
            {
                throw new Exception("SpellCaster haven't been assigned (should never use 'GetComponent' for SpellCaster as it will be too slow') ");
            }

            yield return null; 
        }
 


        // Take control and leave control should have its own space 
        public virtual IEnumerator TakeControl()
        {
            GetEventBroadcaster().InvokeEvent(this, "OnTakeControl");

            yield return null;
        }
        public virtual IEnumerator TakeControlLeave()
        {
            GetEventBroadcaster().InvokeEvent(this, "OnTakeControlLeave");

            GetComponent<EntityCameraManager>().DisableAllAttachedCamera();

            DirectorManager.Instance.ClearCurrentTimeline();


            yield return null;
        }

        public virtual IEnumerator TurnEnter()
        {
            SetExhaunst(false);

            if (_statusEffectHandler == null)
                throw new Exception("Status Effect Handler hasn't never been init");

            if (_ailmentHandler == null)
                throw new Exception(gameObject.name + " 's ailment handler is null"); 

            yield return (_statusEffectHandler.ExecuteStatusRuntimeEffectCoroutine());

            yield return _ailmentHandler.CheckForExpiration();


            _ailmentHandler.ProgressAlimentTTL();

        }

        public virtual IEnumerator TurnLeave()
        {
            SetExhaunst(false); 
            //yield return _runtimeCharacterStatsAccumulator.ResetTemporaryIncreasedValue();

            yield return _statusEffectHandler.RunStatusEffectExpiredScheme();
        }

        public bool ReadyForControl()
        {
            return !IsDead && !IsExhausted;
        }



        public bool ReadyToPerformAction()
        {//We can add functionality to prevent performing action when there is an ailment later 
            return !IsDead ;
        }

        #endregion

        #region Actor Action Methods 
        /// <summary>
        /// Invoked before this character perform any action
        /// </summary>
        public virtual IEnumerator OnPerformAction(  )
        {
            Debug.Log("is exhaunted set ");

            SetExhaunst(true);


            yield return ActionHandler.PerformActionInQueue();


        }


        public IEnumerator OnPostPerformAction()
        {
            //1. check status effect 
            yield return _statusEffectHandler.RunStatusEffectExpiredScheme();
            //2. check if there are any reaction 
            //2.1) check Break Trigger effect 
            yield return _breakTriggerHandler.ResolveTrigger();

        }

        public IEnumerator GetAilmentAction()
        {
            if (_ailmentHandler.DoesAilmentOccur())
            {
                yield return _ailmentHandler.AlimentControlGetAction();
            }
        }

        #endregion

        #region GETTER
        public bool IsDead => _isDead;
        public void SetExhaunst(bool exhaunt, bool forceRelieve = false )
        {
            //_forceRelieve = false;

            _isExhausted = exhaunt;

            if (forceRelieve && !exhaunt)
                _forceRelieve = true; 
        }

        public bool WasGetForcedRelieved(bool notRemove = false)
        {
            if (_forceRelieve)
            {
                if (!notRemove)
                {
                    _forceRelieve = false;
                }

                return true;
            }
            else
                return false;
        }
        public bool IsExhausted { get => _isExhausted;  }

        public CombatEntityActionHandler ActionHandler => _combatEntityActionHandler; 
        public EnergyOverflowHandler OverflowHandler => _energyOverflowHandler ;
        public RuntimeCharacterStatsAccumulator StatsAccumulator => _runtimeCharacterStatsAccumulator;
        public StatusEffectHandler StatusEffectHandler => _statusEffectHandler;
        public SpellCasterHandler SpellCaster => _spellCaster;
        public ItemUserHandler ItemUser => _itemUser;
        public CombatCharacterSheetSO CombatCharacterSheet => _characterSheet; 

        public CombatEntityAnimationHandler CombatEntityAnimationHandler => _combatEntityAnimationHandler;

        #endregion

        #region Attack Hurt Methods  

        public IEnumerator LogicModifyEnergy(CombatEntity target, EnergyModifierData energyModiiferData)
        {

            if (target.SpellCaster.IsEnergyOverflow())
                goto End; 
                    
            target.LogicModifiedEnergy(energyModiiferData);

            if (target.SpellCaster.IsEnergyOverflow())
            {
                _breakTriggerHandler.EnableTriggerAction(target);

                yield return target.OverflowHandler.OverflowResolve();
            }

        End:
            yield return null; 

        }

        public void LogicModifiedEnergy(EnergyModifierData energyModiiferData)
        {
            SpellCaster.ModifyEnergy(energyModiiferData.Side, energyModiiferData.Amount);

        }

        public IEnumerator LogicAttack(List<CombatEntity> targets, EDamageScaling scaling)
        {
            //Prepare for status effect  
            yield return _statusEffectHandler.ExecuteAttackStatusRuntimeEffectCoroutine();

            //1.) Do apply dmg 
            float realDMG = VanaringMathConst.GetATKWithNoise(scaling, StatsAccumulator.GetPhysicalATKAmount());
             

            foreach (CombatEntity target in targets)
            {
                float finalDMG = realDMG + (UnityEngine.Random.Range(realDMG / 100 * 10, -realDMG / 100 * 10));
                StatModifier statsModifer = new StatModifier(-finalDMG, StatModType.Flat);

                target.LogicHurt(this, statsModifer);
            }

        }
        public void LogicHurt(CombatEntity attacker, StatModifier mod)
        {
            //Calculate hit chance 
            float attackerACC = attacker._runtimeCharacterStatsAccumulator.GetAccuracyAmount();
            float defenderEVS = _runtimeCharacterStatsAccumulator.GetEvasionAmount();

            float hitchance = AttributeFormulaLocator.CalculateHitChance(attackerACC, defenderEVS);

            int hitDice = UnityEngine.Random.Range(0, 100);

            //Check if attack hit sucessfully 
            //Hit 
            //Rn we disable dodge 
            if (hitDice < hitchance || true)
            {
                _runtimeCharacterStatsAccumulator.ModifyHPStat(mod);
                //Right now we don't use complex dmg formula 
                int finalDMG = (int)mod.Value;

                //_dmgOutputPopHanlder.AccumulateDMG(finalDMG);

                if (_runtimeCharacterStatsAccumulator.GetHPAmount() <= 0)
                {
                    _isDead = true;
                }

                StartCoroutine(VisualHurt(finalDMG, "Hurt"));
            }

            //Miss 
            else
            {
                Debug.Log("Miss with " + hitDice + " > " + hitchance);
                GetEventBroadcaster().InvokeEvent<Null>(null, "OnDodgeAttack");
            }
        }
        public void LogicHeal(StatModifier statModifier)
        {
            StatsAccumulator.ModifyHPStat(statModifier);
            StartCoroutine(VisualHeal((int)statModifier.Value));
        }
        public IEnumerator VisualHeal(int healAmount)
        {
            GetEventBroadcaster().InvokeEvent<int>((int)healAmount, "OnHeal");
            yield return null;
        }
        public IEnumerator VisualHurt(int dmg, string animationTrigger = "No Animation")
        {

            List<IEnumerator> _coroutine = new List<IEnumerator>();
            if (IsDead)
            {
                _coroutine.Add(DeadVisualAnimationScheme());
            }
            else if (animationTrigger != "No Animation")
            {
                _coroutine.Add(_combatEntityAnimationHandler.PlayTriggerAnimation(animationTrigger));
            }
            if (dmg != 0) 
                GetEventBroadcaster().InvokeEvent(dmg, "OnDamage");

            yield return new WaitAll(this, _coroutine.ToArray());

        }
        #endregion

        #region Status Effect Methods 
        public IEnumerator ApplyNewEffect(StatusRuntimeEffectFactorySO  statusEffect, StatusEffectApplierRuntimeEffect applierFactory, CombatEntity applier)
        {
            yield return _statusEffectHandler.ApplyNewEffect(statusEffect,applierFactory, applier);
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

        public IEnumerator DeadVisualAnimationScheme()
        {
            yield return _combatEntityAnimationHandler.DeadVisualPresentation();
        }

 

       

        
    }
}
