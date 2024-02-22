using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public interface ISceneLoaderWaitForSignal
    {
        IEnumerator OnNotifySceneLoadingComplete();
    }
}
