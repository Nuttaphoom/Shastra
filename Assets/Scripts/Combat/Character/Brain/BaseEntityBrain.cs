using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring{

    [RequireComponent(typeof(CombatEntity))]
    public abstract class BaseEntityBrain : MonoBehaviour, ITurnState
    { 
        protected CombatEntity _combateEntity;
        
        protected virtual void Awake()
        {
            _combateEntity = GetComponent<CombatEntity>() ; 
        }

        public abstract IEnumerator TurnEnter(); 

        public abstract IEnumerator TurnLeave();

        public abstract IEnumerator GetAction( );

        public abstract IEnumerator TakeControl();

        public abstract IEnumerator TakeControlSoftLeave (); 
        public abstract IEnumerator TakeControlLeave();

        public abstract IEnumerator AfterGetAction();

        public abstract IEnumerator OnDeadVisualClear(); 
    }
}
