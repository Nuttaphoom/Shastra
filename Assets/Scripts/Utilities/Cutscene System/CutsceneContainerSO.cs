using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Vanaring 
{
    [CreateAssetMenu(fileName = "CutsceneContainerSO", menuName = "ScriptableObject/Cutscene/CutsceneContainerSO")]
    public class CutsceneContainerSO : ScriptableObject
    {
        [Header("Scene with Activity cutscene inside it")]
        [SerializeField]
        private AssetReferenceT<CutsceneSceneDataSO> _cutsceneSceneAssetRef;

        public CutsceneSceneDataSO GetCutsceneSceneDataSO
        {
            get
            {
                return PersistentAddressableResourceLoader.Instance.LoadResourceOperation<CutsceneSceneDataSO>(_cutsceneSceneAssetRef); 
            }
        }
        
    }
}
