using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Vanaring
{
    public class CombatMissionNodeSetUpHandler : MonoBehaviour, ISceneLoaderWaitForSignal
    {
        [SerializeField]
        private bool _onDebugMode = false;

        [SerializeField, AllowNesting, NaughtyAttributes.ShowIf("_onDebugMode")]
        private EntityLoaderPoolSO _debugPool;
 

        private CombatDungeonNodeLoaderData _combatDungeonNodeLoaderData ;

        private void Awake()
        {
            if (_onDebugMode)
            {
                FindObjectOfType<EntityLoader>().ReceiveEntityLoaderPool(_debugPool);

            }
        }
        private void Start  ()
        {
            if (_onDebugMode)
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

            if (! _onDebugMode)
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

            if (!_onDebugMode)
            {
                memberInParty = DungeonManagerSingleton.Instance.MissionManager.DungeonPartyHandler.PartyMembers;
            } 

            yield return CombatReferee.Instance.InitializeCombat(memberInParty);

            //Play intro 

            CombatReferee.Instance.BeginNewBattle();
        }

         
    }
}
