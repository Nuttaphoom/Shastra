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
        [Header("Use for vfx ")] 
        [SerializeField]
        private VFXEntity _casterVfxAnimationPrefab;
        [SerializeField]
        private VFXEntity _targetVfxAnimationPrefab;

        public VFXEntity CasterVfxAnimationPrefab => _casterVfxAnimationPrefab;
        public VFXEntity TargetVfxAnimationPrefab => _targetVfxAnimationPrefab;

        public string SelfTrigerID ;
    
    
    }

 
}
