using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    [Serializable] 
    public class CutsceneDirector : MonoBehaviour
    {
        [SerializeField] 
        private CutsceneDirectorSO _sceneDataSO;  

        public IEnumerator PlayCutscene()
        {
            yield return null; 
        }
    }
}
