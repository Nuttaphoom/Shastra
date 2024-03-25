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

        //public int GetStrength => _strength ;

        //public int GetVitality => _vitality ;


        //public int GetIntellect => _intellect;

        //public int GetAgility => _agility ;

        //public int GetLuck => _luck;

        /// <summary>
        /// Functions to calculate secondary attribute with respect to Primary attributes
        /// </summary>
        public int GetSecondaryAttribute_MaxHP => 50 + (_vitality * 14 ) ;

        public int GetSecondaryAttribute_MaxMP => 70 + (_intellect * 4 );
        public int GetSecondaryAttribute_PhysicalATK => 20 + (_strength * 3) ;
        public int GetSecondaryAttribute_MagicalATK => 15 + (_intellect * 3);

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
