using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    [Serializable] 
    public class CutsceneDirector : MonoBehaviour
    {
         

        public IEnumerator PlayCutscene()
        {
            yield return null; 
        }
    }
}
