using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class SpellRegister : MonoBehaviour
    {
        private List<SpellActionSO> _registeredSpellActions ;

        /// <summary>
        /// Look for corresponding spell in the spell database
        /// </summary>
        /// <param name="uniqueID"></param>
        /// <exception cref="System.Exception"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public void LoadSpellFromDatabase(List<string> uniqueID)
        {
            if (_registeredSpellActions != null)
                throw new System.Exception("_registeredSpellActions  is not null : may try to laod spell from data base multiple time"); 

            _registeredSpellActions = new List<SpellActionSO>() ;

            throw new NotImplementedException(); 

            
        }
    }
}
