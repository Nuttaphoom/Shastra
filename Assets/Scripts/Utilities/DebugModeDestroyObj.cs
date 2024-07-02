using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vanaring.Assets.Scripts.Utilities;

namespace Vanaring
{
    public class DebugModeDestroyObj : MonoBehaviour , ISceneLoaderWaitForSignal
    {
        public IEnumerator OnNewSceneLoad_BeforeSaveLoadPerform()
        {
            if (!EnableDebuggingChecker.Instance.IsDebugingModeEnable)
            {
                Destroy(gameObject);
            }

            yield return null;
        }

        public IEnumerator OnNotifySceneLoadingComplete()
        {
            if (!EnableDebuggingChecker.Instance.IsDebugingModeEnable)
            {
                Destroy(gameObject);
            }
            yield return null;
        }

        private void Awake()
        {
            if (! EnableDebuggingChecker.Instance.IsDebugingModeEnable )
            {
                Destroy(gameObject); 
            }
        }
    }
}
