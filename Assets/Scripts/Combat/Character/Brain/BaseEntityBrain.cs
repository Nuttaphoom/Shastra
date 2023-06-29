using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring_DepaDemo {

    [RequireComponent(typeof(CombatEntity))]
    public abstract class BaseEntityBrain : MonoBehaviour
    {
       

        public abstract IEnumerator TurnEnter(); 

        public abstract IEnumerator TurnLeave();

        public abstract IEnumerator GetAction( );
    }
}
