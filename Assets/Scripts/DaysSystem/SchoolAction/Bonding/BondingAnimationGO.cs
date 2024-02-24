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
            yield return _cutsceneDirector.PlayCutscene ();
            yield return bondingScheme.PostPerformActivity(); 
        }

        
    }
}
