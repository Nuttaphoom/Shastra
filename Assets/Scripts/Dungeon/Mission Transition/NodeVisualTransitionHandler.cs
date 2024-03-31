using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring 
{
    [Serializable]
    public class NodeVisualTransitionHandler
    {
        [SerializeField]
        private Transform _right_X;

        [SerializeField]
        private Transform _minusRight_X;

        [SerializeField]
        private Transform _forward_Z;

        [SerializeField]
        private Transform _minusForward_Z;

        public Transform GetCorrectPosition(TransitionDirection direction  )
        {
            switch (direction )
            {
                case (TransitionDirection.Forward_Z):
                    return _forward_Z;
                case (TransitionDirection.MinusForward_Z):
                    return _minusForward_Z ;
                case (TransitionDirection.Right_X):
                    return _right_X; 
                case (TransitionDirection.MinusRight_X):
                    return _minusRight_X;
            }

            throw new Exception("given direction is not match") ;
        }
    }
}
