using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class MianmeScript : MonoBehaviour
    {
        [SerializeField] private Animator ashaActorAnim;
        void Start()
        {
            ashaActorAnim.SetTrigger("Sit_Study_Learning");
        }

    }
}
