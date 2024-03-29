using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Vanaring.CombatRewardManager;

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
            ColorfulLogger.LogWithColor("New party init ", Color.red);
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

        public float GetCurrentPartyMemberHP => _currentHP;
        public float GetCurrentPartyMemberMP => _currentMP;

        public void UpdateValue(float hp, float mp)
        {
            _currentHP = hp; 
            _currentMP = mp; 
        }
        #endregion

    }
    public class DungeonPartyHandler 
    {

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

        public RuntimePartyMember GetPartyMember(string characterName)
        {
            foreach (RuntimePartyMember member in _partyMembers)
            {
                if (characterName== member.GetCharacterSheet.CharacterName)
                    return member; 
            }

            throw new Exception("Given name " + characterName + " couldn't be found in the party member datas");
        }
     

        #endregion
       

        public IEnumerator SetUpRuntimeParty()
        {
            _partyMembers = new List<RuntimePartyMember>();

            CombatMemberDataLocator combatMemberDataLocator = PersistentPlayerPersonalDataManager.Instance.CombatMemberDataLocator;
            foreach (var member in combatMemberDataLocator.GetRuntimeCombatMembers) {
                _partyMembers.Add(new RuntimePartyMember(member)); 
            }

            yield return null; 
        }

        public void UpdateMemberStatus(EntityRewardData entityRewardData)
        {
            Debug.Log("Update " + entityRewardData.ControlEntity.CombatCharacterSheet.CharacterName) ;

            foreach (RuntimePartyMember member in _partyMembers)
            {
                if (member.GetCharacterSheet.CharacterName != entityRewardData.ControlEntity.CombatCharacterSheet.CharacterName)
                    continue;

                CombatEntity combatEntity = entityRewardData.ControlEntity;
                Debug.Log("New HP of " + entityRewardData.ControlEntity.CombatCharacterSheet.CharacterName + " is " + combatEntity.StatsAccumulator.GetHPAmount()); 
                member.UpdateValue(combatEntity.StatsAccumulator.GetHPAmount(), combatEntity.SpellCaster.GetMP);



            }
        }
        public IEnumerator OnExitDungeon()
        {
            throw new NotImplementedException(); 
        }
    }
}
