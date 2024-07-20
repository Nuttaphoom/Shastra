using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class OnSelectAnimateButton : MonoBehaviour
    {
        [SerializeField] private Animator buttonAnim;
        [SerializeField] private string selectAnimationName;
        [SerializeField] private string deselectAnimationName;

        public void OnSelect()
        {
            buttonAnim.Play(selectAnimationName);
        }

        public void OnDeselect()
        {
            if(deselectAnimationName != "")
            {
                buttonAnim.Play(deselectAnimationName);
            }
        }
    }
}
