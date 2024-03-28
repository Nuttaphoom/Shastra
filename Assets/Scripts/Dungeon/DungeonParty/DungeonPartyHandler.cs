using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class RuntimePartyMember
    {
        private RuntimeCombatMemberData _runtimeCombatMemberData;
        
        /// <summary>
        /// Used for initialize data into combat, and save/load after combat end 
        /// </summary>
        private float _currentHP = 0; 
        private float _currentMP = 0;   
        
        public RuntimePartyMember(RuntimeCombatMemberData combatMemberData)
        {
            _runtimeCombatMemberData = combatMemberData;

            CombatCharacterSheetSO characterSheet = _runtimeCombatMemberData.GetCharacterSheet;

            _currentHP = characterSheet.GetSecondaryAttribute_MaxHP; 
            _currentMP = characterSheet.GetSecondaryAttribute_MaxMP; 

        }

        #region GETTER
        public CombatCharacterSheetSO GetCharacterSheet 
        { 
            get 
            {
                if (_runtimeCombatMemberData == null)
                    throw new System.Exception("_runtimeCombatMemberData hasn't never been assigned"); 

                return _runtimeCombatMemberData.GetCharacterSheet; 
            }
        }

        public CombatEntity InitializeCombatEntity
        {
            get
            {
                return MonoBehaviour.Instantiate(GetCharacterSheet.GetCombatEntityPrefab).GetComponent<CombatEntity>()   ;
            }
        }
        #endregion

    }
    public class DungeonPartyHandler : MonoBehaviour
    {
        public static DungeonPartyHandler Instance;

        [SerializeField]
        private List<RuntimePartyMember> _partyMembers ;

        #region GETTER
        public List<RuntimePartyMember> PartyMembers
        {
            get
            {
                return _partyMembers;
            }
        }

        #endregion
        private void Awake()
        {
            if (Instance != null)
            {
                throw new System.Exception("DungeonPartyHandler exit more than one instance , This obejct should be properly destroyed when exit dungeon"); 
                Destroy(Instance.gameObject);
            }

            Instance = this; 
        }

        public IEnumerator SetUpRuntimeParty()
        {
            _partyMembers = new List<RuntimePartyMember>();

            CombatMemberDataLocator combatMemberDataLocator = PersistentPlayerPersonalDataManager.Instance.CombatMemberDataLocator;
            foreach (var member in combatMemberDataLocator.GetRuntimeCombatMembers) {
                _partyMembers.Add(new RuntimePartyMember(member)); 
            }

            yield return null; 
        }

        public void OnExitDungeon()
        {
            throw new NotImplementedException(); 
        }
    }
}
