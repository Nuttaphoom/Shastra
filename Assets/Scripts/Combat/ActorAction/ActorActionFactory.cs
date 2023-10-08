using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Vanaring 
{
    public class ActorActionFactory : ScriptableObject
    { 
        [SerializeField]
        protected DescriptionBaseField _description;

        [SerializeField]
        protected TargetSelector _targetSelector;

        [SerializeField]
        protected ActionSignal _actionSignal; 
        
        #region GETTER
        public TargetSelector TargetSelect => _targetSelector;

        public string AbilityName => _description.FieldName; 
        public string Desscription => _description.FieldDescription;
        public Sprite AbilityImage => _description.FieldImage;
        #endregion
    }

    public abstract class ActorAction
    {
        protected TargetSelector _targetSelector;
        protected List<CombatEntity> _targets;
        protected ActionSignal _actionSignal;
        protected CombatEntity _caster;
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
            DirectorManager.Instance.PlayTimeline(_actionSignal) ; 

            //Play runtimeeffect when signal received  

            RuntimeEffectFactorySO factory;

            while ( ! _actionSignal.SignalTerminated())
            {
                if ((factory = _actionSignal.GetReadyEffect()) != null)
                {
                    RuntimeEffect effect = factory.Factorize(_targets);

                    yield return effect.ExecuteRuntimeCoroutine(_caster) ;
                    yield return effect.OnExecuteRuntimeDone(_caster) ;
                }

                yield return new WaitForEndOfFrame();
            }

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

        public TargetSelector GetTargetSelector()
        {
            return _targetSelector; 
        }


    }

    [Serializable]
    public struct DescriptionBaseField
    {
        [SerializeField]
        private string _fieldName ;
        [SerializeField]
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
