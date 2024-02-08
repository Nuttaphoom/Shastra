using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Vanaring.Assets.Scripts.Utilities.Cutscene_System
{
    [CreateAssetMenu(fileName = "CutsceneContainerSO", menuName = "ScriptableObject/Cutscene/CutsceneContainerSO")]
    public class CutsceneContainerSO : ScriptableObject
    {
        [Header("Scene with lecture inside it")]
        [SerializeField]
        private AssetReferenceT<CutsceneSceneDataSO> _cutsceneSceneAssetRef;

        public AssetReference GetCutsceneSceneRef
        {
            get
            {
                return _cutsceneSceneAssetRef ; 
            }
        }
    }
}
