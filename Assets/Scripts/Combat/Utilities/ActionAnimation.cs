using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Vanaring_DepaDemo
{
    [Serializable] 
    public class ActionAnimationInfo     
    {
        //[SerializeField]
        //AnimationClip _skeletonAnimationClip ;  

        //public AnimationClip SkeletonAnimationClip => _skeletonAnimationClip ;
        [Header("Use for vfx and camera movement for the animation")] 
        [SerializeField]
        private GameObject _secondaryAnimationPrefab;

        public string TrigerID ;  



    }
}
