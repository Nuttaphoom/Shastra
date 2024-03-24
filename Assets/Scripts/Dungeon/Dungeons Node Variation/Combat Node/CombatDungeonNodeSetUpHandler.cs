using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class CombatDungeonNodeSetUpHandler : MonoBehaviour, ISceneLoaderWaitForSignal
    {
        public IEnumerator OnNotifySceneLoadingComplete()
        {
            Debug.Log("here");
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
