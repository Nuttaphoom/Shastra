using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring 
{
    public class AilmentLocator : MonoBehaviour
    {
        [Serializable]
        private struct AilmentLocationStruct
        {
            [SerializeField] 
            private AilmentType _ailmentType ; 

            [SerializeField]
            private AilmentFactorySO _ailmentFactorySO ;

            public AilmentType AilmentType => _ailmentType;
            public AilmentFactorySO AilmentFactorySO => _ailmentFactorySO;  
            
            public bool IsCorrectAilmentType(AilmentType type)
            {
                return type == _ailmentType; 
            }
        }

        [SerializeField]
        private List<AilmentLocationStruct> _ailmentLocation =   new List<AilmentLocationStruct>();  

        public static AilmentLocator Instance ; 

        private void Awake()
        {
            Instance = this; 
        } 

        public AilmentFactorySO GetAilmentObject(AilmentType type)
        {
            foreach (var location in _ailmentLocation)
            {
                if (location.IsCorrectAilmentType(type))
                    return location.AilmentFactorySO ; 
            }
            throw new Exception(type + " can not be found within locator"); 
        }

        public enum AilmentType
        {
            Stun 
        }
    }
}
