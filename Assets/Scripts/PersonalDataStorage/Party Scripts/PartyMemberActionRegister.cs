using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Vanaring
{
    public class PartyMemberActionRegister : MonoBehaviour
    {
        private const string _spellDatabaseAddress = "SpellDatabaseSOAddress" ; 

        private List<SpellActionSO> _registeredSpellActions ;

        private SpellDatabaseSO m_spellDatabase ;


        /// <summary>
        /// Look for corresponding spell in the spell database
        /// </summary>
        /// <param name="uniqueID"></param>
        /// <exception cref="System.Exception"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerator LoadSpellFromDatabase(List<string> uniqueID)
        {
            if (_registeredSpellActions != null)
                throw new System.Exception("Try to laod spell from data base multiple time.This isn't allowed. " +
                    "The system should be loaded only 1 time when the save is loaded, and modified the SpellAction thoughtout the lifetime of application, " +
                    "and save the uniqueID when the game is saved");

            LoadSpellDatabaseOP();

            _registeredSpellActions = new List<SpellActionSO>() ;

            for (int i = 0; i < uniqueID.Count; i++)
            {
               _registeredSpellActions.Add( m_spellDatabase.GetRecord(uniqueID[i]) ) ;
            }

            yield return null ;
        }

        private void LoadSpellDatabaseOP()
        { 
            m_spellDatabase = PersistentAddressableResourceLoader.Instance.LoadResourceOperation<SpellDatabaseSO>(_spellDatabaseAddress);
            
        }

      
        public List<SpellActionSO> GetRegisteredSpell()
        {
            if (_registeredSpellActions == null)
                throw new Exception("RegisteredSpellAction is null : the LoadSpellFromDatabase may not be called properly"); 
            
            return _registeredSpellActions ; 
        }

        
    }
}
