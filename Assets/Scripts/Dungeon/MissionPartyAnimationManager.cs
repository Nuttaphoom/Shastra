using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class MissionPartyAnimationManager : MonoBehaviour
    {
        private Animator animator;

        private void Start()
        {
            animator.GetComponent<Animator>();
        }

        public void PerformMove()
        {
            animator.SetFloat("CharacterMovement", 1.0f);
        }
    }
}
