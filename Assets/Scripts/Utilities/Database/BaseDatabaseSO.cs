using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using Unity.Collections;
using System.Security.Permissions;
using JetBrains.Annotations;

namespace Vanaring
{
 
    public abstract class BaseDatabaseSO<RecordType> : ScriptableObject where RecordType : ScriptableObject
    {
        [SerializeField]
        private List<DatabaseKeyPair> _records;
 
        [Tooltip("Generate ID for any unregistered record stored in this database")]
        [ContextMenu("Generate Id")]
        public  void GenerateId()
        {
            Debug.Log("Generating ID"); 
            
            foreach (DatabaseKeyPair record in _records)
            {
                if (! record.IsContainRecorded())
                {
                    record.SetKey("Empty");
                    continue;
                } 
                if (record.IsContainKey)
                    continue;

                string uniqueID = Guid.NewGuid().ToString();
 
                record.SetKey(uniqueID) ;
            }
        }

        public void RemoveDuplicatedRecord()
        {
            for (int i = 0; i <  _records.Count; i++)
            {
                for (int j = i + 1 ; j < _records.Count; j++)
                {
                    if (_records[i].GetRecorded() == _records[j].GetRecorded()) 

                }
            }
        }

        public RecordType GetRecord(string uniqueID)
        {
            foreach (var pair in _records)
            {
                if (pair.IsCorrectKey(uniqueID))
                    return pair.GetRecorded();    
            }

            throw new Exception("" + uniqueID+ "could not be found in database of type " +typeof(RecordType));
        }

        [Serializable]
        private class DatabaseKeyPair
        {
            [SerializeField]
            private RecordType _recorded ;
            private string _key  = "Empty";

            public bool IsContainKey
            {
                get
                {
                    return _key != "Empty" ; 
                }
            }

            public void SetKey(string key)
            {
                _key = key;
            }

            public bool IsCorrectKey(string key)
            {
                return _key == key; 
            }

            public RecordType GetRecorded()
            {
                return _recorded; 
            }

            public bool IsContainRecorded()
            {
                return _recorded != null ; 
            }

            
        }

    }

   
}
