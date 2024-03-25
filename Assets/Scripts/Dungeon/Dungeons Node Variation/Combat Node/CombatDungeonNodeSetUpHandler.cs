using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class CombatDungeonNodeSetUpHandler : MonoBehaviour, ISceneLoaderWaitForSignal
    {
        public IEnumerator OnNewSceneLoad_BeforeSaveLoadPerform()
        {
            yield return null; 
        }

        public IEnumerator OnNotifySceneLoadingComplete()
        {
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
