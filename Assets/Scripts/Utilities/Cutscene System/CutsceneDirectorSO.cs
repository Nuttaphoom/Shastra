using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    [CreateAssetMenu(fileName = "CutsceneDirectorSO" , menuName = "ScriptableObject/Cutscene/CutsceneSO")]
    public class CutsceneDirectorSO : ScriptableObject
    {
        [SerializeField]
        private List<CutsceneInstance> _cutsceneInstances;

        [SerializeField]
        private CutsceneLocationEnvironmentInstance _cutsceneLocationsEnvironment; 
    

    }
}
