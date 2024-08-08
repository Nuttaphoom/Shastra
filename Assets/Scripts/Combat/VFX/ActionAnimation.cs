using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Vanaring 
{
    [Serializable] 
    public class ActionAnimationInfo     
    {
        //[SerializeField]
        //AnimationClip _skeletonAnimationClip ;  

        //public AnimationClip SkeletonAnimationClip => _skeletonAnimationClip ;
        [Header("Use for vfx ")] 
        [SerializeField]
        private VFXEntity _casterVFXEntity;
        [SerializeField]
        private VFXEntity _targetVFXEntity;

        public VFXEntity CasterVfxEntity => _casterVFXEntity ;
        public VFXEntity TargetVfxEntity => _targetVFXEntity ;
        public bool IsProjectile => _isProjectile;

        public string SelfTrigerID ;
        public string TargetTrigerID;
        [SerializeField]
        private bool _isProjectile = false;

    }

 
}
