using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class CombatMissionNodeSetUpHandler : MonoBehaviour, ISceneLoaderWaitForSignal
    {
        CombatDungeonNodeLoaderData _combatDungeonNodeLoaderData ; 
        public IEnumerator OnNewSceneLoad_BeforeSaveLoadPerform()
        {
            yield return null; 
        }

        public IEnumerator OnNotifySceneLoadingComplete()
        {
            _combatDungeonNodeLoaderData = (PersistentSceneLoader.Instance.ExtractSavedData<CombatDungeonNodeLoaderData>("CombatDungeonNodeDataUser") ).GetData() ;  

            //Use data from CombatNode and set up those data before Initialize anything 

            FindObjectOfType<EntityLoader>().ReceiveEntityLoaderPool(_combatDungeonNodeLoaderData.EnemyLoaderPool) ;

            StartCoroutine(InitializeCombat()) ;

            yield return null; 
        }

        private IEnumerator InitializeCombat()
        {
            List<RuntimePartyMember> memberInParty = DungeonManagerSingleton.Instance.MissionManager.DungeonPartyHandler.PartyMembers;

            yield return CombatReferee.Instance.InitializeCombat(memberInParty);

            //Play intro 

            CombatReferee.Instance.BeginNewBattle();
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
