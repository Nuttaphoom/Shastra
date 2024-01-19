using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

namespace Vanaring
{
    public abstract class GeneralDatabaseSO<RecordType> : ScriptableObject where RecordType : ScriptableObject
    {
        [SerializeField]
        private List<RecordType> _records = new List<RecordType>();  

        private Dictionary<string ,RecordType> _recordWithUniqueID = new Dictionary<string ,RecordType>();

        [SerializeField] private string id = string.Empty;
        public string Id => id;

        [ContextMenu("Generate Id")]
        private void GenerateId()
        {
            foreach (RecordType record in _records)
            {
                if (_recordWithUniqueID.ContainsValue(record))
                    continue; 

                string uniqueID = Guid.NewGuid().ToString();
 
                _recordWithUniqueID.Add(uniqueID, record);
            }
        }

        public RecordType GetRecord(string uniqueID)
        {
            if (_recordWithUniqueID.TryGetValue(uniqueID, out RecordType record))
            {
                return record; 
            }

            throw new Exception("" + uniqueID+ "could not be found in database of type " +typeof(RecordType));
        }


    }
}
