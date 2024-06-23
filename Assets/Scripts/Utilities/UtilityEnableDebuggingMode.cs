using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.iOS;
using UnityEngine;

namespace Vanaring.Assets.Scripts.Utilities
{
    [CreateAssetMenu( fileName = "DebugMode", menuName = "ScriptableObject/DebugModeEnable" )]
    public class UtilityEnableDebuggingMode : ScriptableObject
    {
        [SerializeField] 
        private bool _isDebugModeEnable = false ; 
    
        public bool IsDebugModeEnable => _isDebugModeEnable;  
    }

    public class EnableDebuggingChecker : PersistentInstantiatedObject<EnableDebuggingChecker>
    {
        [SerializeField] 
        public bool IsDebugingModeEnable => PersistentAddressableResourceLoader.Instance.LoadResourceOperation<UtilityEnableDebuggingMode>("DebugMode").IsDebugModeEnable;
        
    }




}
