using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class CombatBacklogNotification : MonoBehaviour
    {
        [SerializeField]
        private CombatReferee _referee;


        private void Awake()
        {
            if (_referee == null)
                throw new Exception("Referee need to be assigned to CombatBacklogNotification") ; 
            
        }


    }
}
