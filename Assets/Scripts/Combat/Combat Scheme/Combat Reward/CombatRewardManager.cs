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

        private List<EventRewardData> _rewardPool ; 
        public struct EntityRewardData
        {
            public ControlableEntity ControlEntity ;
            public float ReceivedExp ;
        }
        public class CombatRewardData
        {
            public List<EntityRewardData> RewardForEntities ;
            public List<EventRewardData> Rewards;

            public CombatRewardData()
            {
                RewardForEntities = new List<EntityRewardData>();
            }

        }

        /// <summary>
        /// Called by CombatMissionNodeSetUpHandler too pass in mission data extracted from MissionNode
        /// </summary>
        /// <returns></returns>
        public void SetUpRewardDataPool(List<EventRewardData> pool)
        {
            _rewardPool = new List<EventRewardData>(); 
            foreach (EventRewardData e in pool)
            {
                _rewardPool.Add(e); 
            }

            
        }


        public IEnumerator CombatRewardSchemeStart(CombatReferee combatReferee)
        {
            if (_rewardPool == null)
                throw new System.Exception("Reward Pool hasn't never been set up") ;

            var playerTeam = combatReferee.GetCompetatorsBySide(ECompetatorSide.Ally);

            //Calculate reward    
            CombatRewardData combatReward = new CombatRewardData();
            
            combatReward.Rewards = _rewardPool;

            foreach (CombatEntity entity in playerTeam) 
            {
                EntityRewardData rewardEntity = new EntityRewardData() {
                    ControlEntity = entity as ControlableEntity,
                    ReceivedExp = 100,
                } ;       
                combatReward.RewardForEntities.Add(rewardEntity);
            }

            //Display Reward  
            yield return _combatRewardDisplayer.DisplayRewardUICoroutine(combatReward); 

            yield return SubmitCombatReward(combatReward);


            //Clear up data 
            yield return UpdatePartyMembersStatus(playerTeam);
            ItemInventory.instance.RestoreRemainingItemIntoDatabase(); 
        }

        private IEnumerator SubmitCombatReward(CombatRewardData combatRewardData)
        {
            foreach (EntityRewardData rewardEntity in combatRewardData.RewardForEntities)
            {
                PersistentPlayerPersonalDataManager.Instance.CombatMemberDataLocator.GetRuntimeData(rewardEntity.ControlEntity.CombatCharacterSheet.CharacterName).LevelAttributeHandler.ReceiveEXP(rewardEntity.ReceivedExp);
            }

            foreach (EventRewardData rewardData in combatRewardData.Rewards)
            {
                rewardData.GetReward().SubmitReward(); 
            }
            

            yield return null; 
            //Submit exp reward
        }

        private IEnumerator UpdatePartyMembersStatus(List<CombatEntity> playerTeam)
        {
            //throw new System.Exception("member exp should be update in PersistentPersonal and the party should be re calculate remaining status accordingly");
            foreach (var entity in playerTeam)
            {
                DungeonManagerSingleton.Instance.MissionManager.DungeonPartyHandler.UpdateMemberStatus(entity);
                
                //MissionManagerSingleton.Instance.DungeonPartyHandler.UpdateMemberStatus(rewardEntity);
            } 

            
            yield return null; 
        }

         
    }
}
