using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Vanaring.Assets.Scripts.Utilities.StringConstant;

namespace Vanaring
{
    public class PartyMemberActionRegister    
    {

        private List<SpellActionSO> _registeredSpellActions ;

        private SpellDatabaseSO m_spellDatabase ;


        /// <summary>
        /// Look for corresponding spell in the spell database
        /// </summary>
        /// <param name="uniqueID"></param>
        /// <exception cref="System.Exception"></exception>
        /// <exception cref="NotImplementedException"></exception>
        
    
        public void LoadSpellFromDatabase(List<string> uniqueID)
        {
            LoadSpellDatabaseOP();

            if (_registeredSpellActions != null)
                throw new System.Exception("Try to laod spell from data base multiple time.This isn't allowed. " +
                    "The system should be loaded only 1 time when the save is loaded, and modified the SpellAction thoughtout the lifetime of application, " +
                    "and save the uniqueID when the game is saved");

            _registeredSpellActions = new List<SpellActionSO>() ;

            for (int i = 0; i < uniqueID.Count; i++)
            {
               _registeredSpellActions.Add( m_spellDatabase.GetRecord(uniqueID[i]) ) ;
            }

        }

        public void SaveSpellActionData()
        {
            //Save the Unique id from all of the SpellActionSO in _registeredSpellActions
            //Getting Address of the record in database =>
            //m_spellDatabase.GetRecordKey(_registeredSpellActions[0]);



        }

        public void UnlockSpellAction(SpellActionSO spellToUnlock)
        {
            if (_registeredSpellActions == null)
                throw new Exception("_registerSpellActions is null"); 
            //Dont forget to save all of the _registeredSpellActions again when application ends of saved
            _registeredSpellActions.Add(spellToUnlock) ;
            Debug.Log("RegisteredSpellAction.Count : " + _registeredSpellActions.Count);
        }

        private void LoadSpellDatabaseOP()
        { 
            m_spellDatabase = PersistentAddressableResourceLoader.Instance.LoadResourceOperation<SpellDatabaseSO>(DatabaseAddressLocator.GetSpellDatabaseAddress);
            
        }



      
        public List<SpellActionSO> GetRegisteredSpell()
        {
            if (_registeredSpellActions == null)
                throw new Exception("RegisteredSpellAction is null : the LoadSpellFromDatabase may not be called properly"); 
            
            return _registeredSpellActions ; 
        }

        
    }
}
