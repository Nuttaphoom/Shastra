using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Vanaring 
{
    [CreateAssetMenu(fileName= "Combat Ability" , menuName = "ScriptableObject/Combat/CombatAbility" )]
    public class ActorActionFactory : ScriptableObject
    { 
        [SerializeField]
        protected DescriptionBaseField _description;

        [SerializeField]
        protected TargetSelector _targetSelector;

        [SerializeField]
        private RuntimeEffectFactorySO _effect ;

        #region GETTER
        public TargetSelector TargetSelect => _targetSelector;

        public string AbilityName => _description.FieldName; 
        public string Desscription => _description.FieldDescription;
        public Sprite AbilityImage => _description.FieldImage;
        public RuntimeEffectFactorySO EffectFactory => _effect;
        #endregion
    }

    public interface IActorAction 
    {
        public IEnumerator Simulate(CombatEntity target); 
        /// <summary>
        /// This method should be invoked prior to taking any action as it might causes the Actor to be exhaunted.
        /// </summary>
        /// <returns>An IEnumerator representing the pre-action process.</returns>
        public IEnumerator PreActionPerform();

        /// <summary>
        /// This method should be invoked if and only if the action was successfully executed.
        /// </summary>
        /// <returns>An IEnumerator representing the post-action process.</returns>
        public IEnumerator PostActionPerform();
        public void SetActionTarget(List<CombatEntity> targets);

        public RuntimeEffect GetRuntimeEffect();
        public TargetSelector GetTargetSelector(); 

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
