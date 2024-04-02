using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class CombatRewardManager : MonoBehaviour
    {
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
            CombatRewardData combatReward = new CombatRewardData(); 
        
            foreach (CombatEntity entity in combatReferee.GetCompetatorsBySide(ECompetatorSide.Ally) ) 
            {
                EntityRewardData rewardEntity = new EntityRewardData() {
                    ControlEntity = entity as ControlableEntity,
                    ReceivedExp = 10, 
                } ;       
                combatReward.RewardForEntities.Add(rewardEntity);
            }

            yield return UpdatePartyMembersStatus(combatReward);

            ItemInventory.instance.RestoreRemainingItemIntoDatabase(); 
        }

        public IEnumerator UpdatePartyMembersStatus(CombatRewardData combatRewardData)
        {
            Debug.Log("Update Party Member") ;
            foreach (EntityRewardData rewardEntity in combatRewardData.RewardForEntities) { 
                MissionManagerSingleton.Instance.DungeonPartyHandler.UpdateMemberStatus(rewardEntity) ; 
            }

            yield return null; 
        }

         
    }
}
