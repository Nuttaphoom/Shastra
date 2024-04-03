using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Vanaring.CombatRewardManager;

namespace Vanaring
{
    public class CombatRewardManager : MonoBehaviour
    {
        [SerializeField]
        private CombatRewardDisplayer _combatRewardDisplayer; 
        public struct EntityRewardData
        {
            public ControlableEntity ControlEntity ;
            public float ReceivedExp ; 
        }
        public class CombatRewardData
        {
            public List<EntityRewardData> RewardForEntities ;

            public CombatRewardData()
            {
                RewardForEntities = new List<EntityRewardData>();
            }

        }
        public IEnumerator CombatRewardSchemeStart(CombatReferee combatReferee)
        {
            //Calculate reward
            CombatRewardData combatReward = new CombatRewardData(); 
        
            foreach (CombatEntity entity in combatReferee.GetCompetatorsBySide(ECompetatorSide.Ally) ) 
            {
                EntityRewardData rewardEntity = new EntityRewardData() {
                    ControlEntity = entity as ControlableEntity,
                    ReceivedExp = 10, 
                } ;       
                combatReward.RewardForEntities.Add(rewardEntity);
            }

            //Display Reward  
            //yield return _combatRewardDisplayer.DisplayRewardUICoroutine(combatReward); 

            yield return SubmitCombatReward(combatReward);


            //Clear up data 
            yield return UpdatePartyMembersStatus(combatReward);
            ItemInventory.instance.RestoreRemainingItemIntoDatabase(); 
        }

        private IEnumerator SubmitCombatReward(CombatRewardData combatRewardData)
        {
            foreach (EntityRewardData rewardEntity in combatRewardData.RewardForEntities)
            {
                PersistentPlayerPersonalDataManager.Instance.CombatMemberDataLocator.GetRuntimeData(rewardEntity.ControlEntity.CombatCharacterSheet.CharacterName).LevelAttributeHandler.ReceiveEXP(rewardEntity.ReceivedExp);
            }

            yield return null; 
            //Submit exp reward
        }

        private IEnumerator UpdatePartyMembersStatus(CombatRewardData combatRewardData)
        {
            foreach (EntityRewardData rewardEntity in combatRewardData.RewardForEntities) { 
                MissionManagerSingleton.Instance.DungeonPartyHandler.UpdateMemberStatus(rewardEntity) ; 
            }
            yield return null; 
        }

         
    }
}
