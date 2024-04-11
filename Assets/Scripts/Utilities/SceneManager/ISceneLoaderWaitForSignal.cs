using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public interface ISceneLoaderWaitForSignal
    {
        IEnumerator OnNewSceneLoad_BeforeSaveLoadPerform(); 

        /// <summary>
        /// Called AFTER Save/Load operation
        /// </summary>
        /// <returns></returns>
        IEnumerator OnNotifySceneLoadingComplete();
    }
}
