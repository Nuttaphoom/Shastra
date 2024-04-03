using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Vanaring
{
    [CreateAssetMenu(fileName = "CombatCharacterSheet", menuName = "ScriptableObject/Character/CombatCharacterSheet")]

    public class CombatCharacterSheetSO : CharacterSheetSO 
    {

        /// <summary>
        /// Primary Attributes only use in the begining of combat to set up secondary attributes 
        /// If you wish to modify runtime attribute like DMG deal or MaxHP the modifer should be applied directly to secondary attributes 
        /// </summary>
        [Header("Character Primary Attribute Stats")]

        [SerializeField]
        private int _strength;

        [SerializeField]
        private int _vitality;

        [SerializeField]
        private int _intellect;

        [SerializeField]
        private int _agility;

        [SerializeField]
        private int _luck ;

        [SerializeField]
        private int _strength_per_level = 1;


        [SerializeField]
        private int _vitality_per_level = 1;

        [SerializeField]
        private int _intellect_per_level = 1;

        [SerializeField]
        private int _agility_per_level = 1;

        [SerializeField]
        private int _luck_per_level = 1 ;

        [Header("Ailment Resistant")]
        [SerializeField]
        private  AilmentResistantDataInfo _ailmentResistantDataInfo;

        [Header("Character Sprite")]
        [SerializeField]
        private  Sprite _characterIconGUI;
    
        [SerializeField]
        private AssetReferenceT<GameObject> _combatEntityPrefabAddress;

        #region GETTER 
        public AilmentResistantDataInfo ResistantData => _ailmentResistantDataInfo;

        public Sprite GetCharacterIcon => _characterIconGUI;

        public int GetStrength => _strength;

        public int GetVitality => _vitality;


        public int GetIntellect => _intellect;

        public int GetAgility => _agility;

        public int GetLuck => _luck;

        public int GetModStrength => _strength_per_level;

        public int GetModVitality => _vitality_per_level;


        public int GetModIntellect => _intellect_per_level;

        public int GetModAgility => _agility_per_level;

        public int GetModLuck => _luck_per_level;

        /// <summary>
        /// Functions to calculate secondary attribute with respect to Primary attributes
        /// </summary>
        public int GetBaseSecondaryAttribute_MaxHP => 50 + (_vitality * 14 ) ;
        public int GetBaseSecondaryAttribute_MaxMP => 70 + (_intellect * 4 );
        public int GetBaseSecondaryAttribute_PhysicalATK => 20 + (_strength * 3) ;
        public int GetBaseSecondaryAttribute_MagicalATK => 15 + (_intellect * 3);

        public float GetBaseSecondaryAttribute_Evasion => 100 + ( (_agility )  )  ;
        public float GetBaseSecondaryAttribute_ACC => 190 + (_agility ); 

        public GameObject GetCombatEntityPrefab
        {
            get
            {
                if (_combatEntityPrefabAddress == null)
                    throw new Exception("" + name + "combat entity prefab address hasn't never been asisigned");

                return PersistentAddressableResourceLoader.Instance.LoadResourceOperation<GameObject>(_combatEntityPrefabAddress) ; 
            }
        }

        #endregion
    }
}
