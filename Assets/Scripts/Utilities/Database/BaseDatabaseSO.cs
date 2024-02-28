using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using Unity.Collections;
using System.Security.Permissions;
using JetBrains.Annotations;
using System.Xml;

namespace Vanaring
{
 
    public abstract class BaseDatabaseSO<RecordType> : ScriptableObject where RecordType : ScriptableObject
    {
        [SerializeField]
        protected List<DatabaseKeyPair> _records;

        public  void GenerateId()
        {
            Debug.Log("Generating ID"); 
            
            foreach (DatabaseKeyPair record in _records)
            {
                if (! record.IsContainRecorded())
                {
                    record.SetKey("EMPTY");
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
                    {
                        _records.RemoveAt(j);
                        i = 0;
                        break; 
                    }

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

            throw new Exception("" + uniqueID+ "could not be found in database of type " + typeof(RecordType));
        }

        public string GetRecordKey(RecordType type)
        {
            foreach (var pair in _records)
            {
                if (pair.GetRecorded() == type)
                    return pair.GetKey(); 
            }

            throw new Exception("" + type + "could not be found in database of type " + typeof(RecordType));

        }

        [Serializable]
        protected class DatabaseKeyPair
        {
            [SerializeField]
            private RecordType _recorded ;
            [SerializeField]
            private string _key = "EMPTY" ;

            public bool IsContainKey
            {
                get
                {
                    if (_key == "EMPTY")
                        return false;

                    if (_key == null)
                        return false;  

                    return true ; 
                }
            }

            public void SetKey(string key)
            {
                Debug.Log("set key + " + key); 
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

            public string GetKey()
            {
                return _key; 
            }

            public bool IsContainRecorded()
            {
                return _recorded != null ; 
            }

            
        }

    }

   
}
