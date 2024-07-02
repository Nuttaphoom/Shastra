using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Vanaring.Assets.Scripts.Utilities;

namespace Vanaring
{
    public class CombatMissionNodeSetUpHandler : MonoBehaviour, ISceneLoaderWaitForSignal
    {
 

        [SerializeField]
        private EntityLoaderPoolSO _debugPool;

        
    

        private CombatDungeonNodeLoaderData _combatDungeonNodeLoaderData ;

        private void Awake()
        {
            if (EnableDebuggingChecker.Instance.IsDebugingModeEnable)
            {
                FindObjectOfType<EntityLoader>().ReceiveEntityLoaderPool(_debugPool);

            }
        }
        private void Start  ()
        {
            if (EnableDebuggingChecker.Instance.IsDebugingModeEnable)
            {
                StartCoroutine(InitializeCombat());
            }
        } 

        public IEnumerator OnNewSceneLoad_BeforeSaveLoadPerform()
        {
            yield return null; 
        }

        public IEnumerator OnNotifySceneLoadingComplete()
        {

            if (!EnableDebuggingChecker.Instance.IsDebugingModeEnable)
            {

                _combatDungeonNodeLoaderData = (PersistentSceneLoader.Instance.ExtractSavedData<CombatDungeonNodeLoaderData>("CombatDungeonNodeDataUser") ).GetData() ;  

                //Use data from CombatNode and set up those data before Initialize anything 

                FindObjectOfType<EntityLoader>().ReceiveEntityLoaderPool(_combatDungeonNodeLoaderData.EnemyLoaderPool) ;
                FindObjectOfType<CombatRewardManager>().SetUpRewardDataPool(_combatDungeonNodeLoaderData.CombatRewards); 

                StartCoroutine(InitializeCombat());
                yield return null;
            }
        }

        private IEnumerator InitializeCombat()
        {
            List<RuntimePartyMember> memberInParty = null; 

            if (!EnableDebuggingChecker.Instance.IsDebugingModeEnable)
            {
                memberInParty = DungeonManagerSingleton.Instance.MissionManager.DungeonPartyHandler.PartyMembers;
            } 

            yield return CombatReferee.Instance.InitializeCombat(memberInParty);

            //Play intro 


            CombatReferee.Instance.BeginNewBattle();
        }

         
    }
}
