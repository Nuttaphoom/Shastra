using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Vanaring_DepaDemo 
{
    [CreateAssetMenu(fileName= "Combat Ability" , menuName = "ScriptableObject/Combat/CombatAbility" )]
    public class CombatActionSO : ScriptableObject
    { 
        [SerializeField]
        protected DescriptionBaseField _description; 

        [SerializeField]
        private RuntimeEffectFactorySO _effect ;

        #region GETTER
        public string AbilityName => _description.FieldName; 
        public string Desscription => _description.FieldDescription;
        public RuntimeEffectFactorySO EffectFactory => _effect;

        #endregion
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
