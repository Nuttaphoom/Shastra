using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Vanaring 
{
    public abstract class ActorActionFactory : ScriptableObject
    {
        [Header("===Description Information Of this Action ===")]
        [Header("=== Action name will be dispaly on BacklogNotification")]
        [SerializeField]
        protected DescriptionBaseField _actionDescription;

        [Header("===Action Target Detail===")]
        [SerializeField]
        protected TargetSelector _targetSelector;

        [Header("=== Animation ===")]
        [SerializeField]
        protected ActionSignal _actionSignal;


        public abstract ActorAction FactorizeRuntimeAction(CombatEntity combatEntity );

        #region GETTER
        public TargetSelector TargetSelect => _targetSelector;
        public string AbilityName => _actionDescription.FieldName;
        public string Desscription => _actionDescription.FieldDescription;
        public Sprite AbilityImage => _actionDescription.FieldImage;

        public ActionSignal ActionSignal => _actionSignal;
        public DescriptionBaseField DescriptionBaseField => _actionDescription; 
 
        #endregion

    }
    

    public abstract class ActorAction
    {
        protected TargetSelector _targetSelector;
        protected List<CombatEntity> _targets = new List<CombatEntity>() ;
        protected ActionSignal _actionSignal;
        protected CombatEntity _caster;
        protected DescriptionBaseField _description;


        private List<Coroutine> _ongoingEffect = new List<Coroutine>();

        public ActorAction(ActorActionFactory actionFactory, CombatEntity caster)
        {
            ColorfulLogger.LogWithColor("construct ActorAction",Color.yellow) ;
            _description = actionFactory.DescriptionBaseField ;
            _targetSelector = actionFactory.TargetSelect ;
            _actionSignal = new ActionSignal(actionFactory.ActionSignal) ;
            
            _caster = caster; 
        }
        public abstract IEnumerator Simulate(CombatEntity target); 
        /// <summary>
        /// This method should be invoked prior to taking any action as it might causes the Actor to be exhaunted.
        /// </summary>
        /// <returns>An IEnumerator representing the pre-action process.</returns>
        public abstract IEnumerator PreActionPerform();

        /// <summary>
        /// This method should be invoked if and only if the action was successfully executed.
        /// </summary>
        /// <returns>An IEnumerator representing the post-action process.</returns>
        public abstract IEnumerator PostActionPerform();

        /// <summary>
        /// PerformAction resonsbile for playing animation and effect until the timeline is done playing
        /// </summary>
        /// <param name="targets"></param>
        /// 
        public IEnumerator PerformAction()
        {
            //Set up 
            SetUpTimeLineActorSetting();

            //Play Timeline in DirectorManager and register signal
            DirectorManager.Instance.PlayTimeline(_actionSignal);

            //Play runtimeeffect when signal received  

            RuntimeEffectFactorySO factory;

            do
            {
                Debug.Log("wait for signal terminated") ;

                if ((factory = _actionSignal.GetReadyEffect()) != null)
                {
                    Debug.Log("get ready effect"); 
                    RuntimeEffect effect = factory.Factorize(_targets);
                    _ongoingEffect.Add(_caster.StartCoroutine(CasterStartExecuteEffectCoroutine(effect)));
                }

                yield return new WaitForEndOfFrame();
            } while ((!_actionSignal.SignalTerminated())); 

            while (_ongoingEffect.Count > 0)
            {
                Debug.Log("onGoing effect") ;

                yield return new WaitForEndOfFrame();
            }
            while (DirectorManager.Instance.IsPlayingTimeline)
            {
                Debug.Log("playing timeline") ;

                yield return new WaitForEndOfFrame();
            }

            DirectorManager.Instance.ClearCurrentTimeline() ; 

        }


        private IEnumerator CasterStartExecuteEffectCoroutine(RuntimeEffect effect)
        {
            yield return effect.ExecuteRuntimeCoroutine(_caster);
            yield return effect.OnExecuteRuntimeDone(_caster);
            _ongoingEffect.RemoveAt(0);
        }

        private void SetUpTimeLineActorSetting()
        {
            List<GameObject> actors = new List<GameObject>();

            actors.Add(_caster.gameObject);
            
            foreach (var entity in _targets)
            {
                actors.Add(entity.gameObject);
            }

            _actionSignal.SetUpActorsSetting(actors);
        }
    
    
        public void SetActionTarget(List<CombatEntity> targets)
        {
            _targets = new List<CombatEntity>();
            foreach (var entity in targets)
            {
                _targets.Add(entity);
            }
        }
        public List<CombatEntity> GetActionTargets()
        {
            return _targets; 
        }
        
        public TargetSelector GetTargetSelector()
        {
            return _targetSelector; 
        }

        public DescriptionBaseField GetDescription()
        {
            return _description;
        }


    }

    [Serializable]
    public struct DescriptionBaseField
    {
        [SerializeField]
        private string _fieldName ;
        [SerializeField]
        [TextArea(3, 5)]
        private string _fieldDescription ;
        [SerializeField]
        private Sprite _fieldImage;

        public DescriptionBaseField(string name, string description, Sprite image)
        {
            _fieldName = name;
            _fieldDescription = description;
            _fieldImage = image;
        }
        public string FieldName => _fieldName; 
        public string FieldDescription => _fieldDescription; 
        public Sprite FieldImage => _fieldImage; 
    }

    
}
