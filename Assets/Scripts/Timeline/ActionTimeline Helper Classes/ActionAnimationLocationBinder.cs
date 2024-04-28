using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring
{ 
    /// <summary>
    /// Dynamically assign Caster and Targets to CasterTransfrom and TargetTransform, to change its position to the Transform 
    /// location is respect to World Center (0,0,0) 
    /// </summary>
    public class ActionAnimationLocationBinder
    {
        private class RuntimeActionLocationBinder 
        {
            private CombatEntity _movedEntity;
            private Transform _oldParent ;
            private Transform _oldTransform;

            public RuntimeActionLocationBinder(CombatEntity movedEntity, Transform newParent)
            {
                _movedEntity = movedEntity;

                _oldTransform = _movedEntity.transform;
                
                if (_movedEntity.transform.parent != null)
                    _oldParent = _movedEntity.transform.parent;
                 

                _movedEntity.transform.parent = newParent; 
                _movedEntity.transform.localPosition = Vector3.zero ;  
                _movedEntity.transform.localRotation = Quaternion.identity ;

                
            }

            public void RestoreLocation()
            {
                if (_oldParent != null)
                    _movedEntity.transform.parent = _oldParent;
                else
                    _movedEntity.transform.parent = null;

                ////Maybe we don't need to restore transformation ? make it re-assign everytime switch control ?
                //_movedEntity.transform.position = _oldTransform.transform.position; 
                //_movedEntity.transform.rotation = _oldTransform.transform.rotation;
                //_movedEntity.transform.localScale = _oldTransform.transform.localScale; 
            }
        }


        List<RuntimeActionLocationBinder> _runtimeActionBinded = new List<RuntimeActionLocationBinder>(); 
        private List<Transform> _casterTransform = new List<Transform>();
        private List<Transform> _targetsTransform = new List<Transform>(); 
        public ActionAnimationLocationBinder(List<Transform> casterTransforms ,List<Transform> targetTransform ,GameObject caster, List<GameObject> targets )
        {
            _casterTransform = casterTransforms; 
            _targetsTransform = targetTransform; 

            foreach (var casterTransfrom in _casterTransform)
            {
                AssignNewBinder(caster.GetComponent<CombatEntity>(),casterTransfrom) ; 
            }

            int index = 0;
            foreach (var t in _targetsTransform)
            {

                AssignNewBinder(targets[index].GetComponent<CombatEntity>(), t );
                index++;
            }

        }

       public void AssignNewBinder(CombatEntity entity, Transform newParent)
        {
            var runtimeActionLocationBinder = new RuntimeActionLocationBinder(entity, newParent);
            _runtimeActionBinded.Add(runtimeActionLocationBinder);

        }

        public void ResetPositionBack()
        {
            foreach (var binder in _runtimeActionBinded)
            {
                binder.RestoreLocation(); 
            }

            _runtimeActionBinded.Clear(); 
        }

         

    }
}
