using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Vanaring
{
    public class TESTSCRIPT_LOADMAP : MonoBehaviour
    {
        
         public void LoadMapScene()
        {
            PersistentSceneLoader.Instance.LoadMapScene( );
        }
    }
}
