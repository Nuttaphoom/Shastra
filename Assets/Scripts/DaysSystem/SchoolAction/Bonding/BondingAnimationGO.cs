using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Vanaring.Assets.Scripts.DaysSystem.SchoolAction;

namespace Vanaring
{
    public class BondingAnimationGO : MonoBehaviour 
    {
        [SerializeField]
        private CutsceneDirector _cutsceneDirector;

        public IEnumerator PlayCutscene(NPCBondingScheme bondingScheme)
        {
            Debug.Log("play cutscene"); 
            yield return _cutsceneDirector.PlayCutscene ();

            Debug.Log("cutscene end");
            yield return bondingScheme.PostPerformActivity(); 
        }

        
    }
}
