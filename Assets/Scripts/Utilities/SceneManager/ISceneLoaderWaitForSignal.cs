using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public interface ISceneLoaderWaitForSignal
    {
        //Should be replaced Awake in Location components 
        IEnumerator OnNotifySceneLoadingComplete();
    }
}
