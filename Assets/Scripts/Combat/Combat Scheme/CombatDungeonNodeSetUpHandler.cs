using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class CombatDungeonNodeSetUpHandler : MonoBehaviour, ISceneLoaderWaitForSignal
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

            yield return CombatReferee.Instance.InitializeCombat();

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
