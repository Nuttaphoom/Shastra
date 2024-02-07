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
    [Serializable]
    public class PartyMemberActionRegister    
    {
        [SerializeField]
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
                throw new System.Exception("Try to load spell from data base multiple time.This isn't allowed. " +
                    "The system should be loaded only 1 time when the save is loaded, and modified the SpellAction thoughtout the lifetime of application, " +
                    "and save the uniqueID when the game is saved");

            _registeredSpellActions = new List<SpellActionSO>();

            for (int i = 0; i < uniqueID.Count; i++)
            {
               _registeredSpellActions.Add( m_spellDatabase.GetRecord(uniqueID[i]) ) ;
            }

        }

        //public object SaveSpellActionData()
        //{
        //    //Save the Unique id from all of the SpellActionSO in _registeredSpellActions
        //    //Getting Address of the record in database =>
        //    List<string> keys = new List<string>();
        //    foreach (SpellActionSO spellSO in _registeredSpellActions)
        //    {
        //        keys.Add(m_spellDatabase.GetRecordKey(spellSO));
        //    }
        //    return keys;
        //}

        public void UnlockSpellAction(SpellActionSO spellToUnlock)
        {
            if (_registeredSpellActions == null)
            {
                Debug.LogWarning("RegisteredSpellAction is null, this shouldn't be allowed as the load system should be responsible for creating the isntance"); 
                _registeredSpellActions = new List<SpellActionSO>();
            }
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


        #region Save System
        public object CaptureState()
        {
            if (m_spellDatabase == null)
            {
                LoadSpellDatabaseOP();
            }

            List<string> keys = new List<string>();
            foreach (SpellActionSO spellSO in _registeredSpellActions)
            {
                keys.Add(m_spellDatabase.GetRecordKey(spellSO));
            }

            return keys;
        }

        public void RestoreState(object state)
        {
            var saveData = (List<string>)state;

            LoadSpellFromDatabase(saveData);
        }

        #endregion

    }
}
