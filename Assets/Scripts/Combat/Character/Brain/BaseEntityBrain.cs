using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring_DepaDemo {

    [RequireComponent(typeof(CombatEntity))]
    public abstract class BaseEntityBrain : MonoBehaviour, ITurnState
    { 
        CombatEntity _combateEntity;
        
        private void Awake()
        {
            _combateEntity = GetComponent<CombatEntity>() ; 
        }

        public abstract IEnumerator TurnEnter(); 

        public abstract IEnumerator TurnLeave();

        public abstract IEnumerator GetAction( );

        public abstract IEnumerator TakeControl(); 

        public abstract IEnumerator TakeControlLeave();
    }
}
