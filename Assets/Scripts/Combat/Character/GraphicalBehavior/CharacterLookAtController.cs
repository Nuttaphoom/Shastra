using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring.Assets.Scripts.Combat.Character.GraphicalBehavior
{
    [Serializable]
    public class CharacterLookAtController
    {
        [Header("Center of rotation")]
        [SerializeField]
        private  Transform _rotationJoin ; 
        public void RotateToTarget(Transform target,float elapsedTime) {
            Vector3 lookAtVec = target.position - _rotationJoin.position;
            lookAtVec.y = 0;

        }
    }
}
