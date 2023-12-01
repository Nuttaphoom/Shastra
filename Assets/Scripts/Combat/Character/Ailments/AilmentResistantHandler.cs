using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Vanaring.AilmentLocator;
using UnityEngine;

namespace Vanaring 
{
    public class AilmentResistantHandler
    {
      Dictionary<AilmentType, bool> _runtime_ailmentResistant_data_dict = null; 

        public AilmentResistantHandler(AilmentResistantDataInfo resistantData)
        {
            _runtime_ailmentResistant_data_dict = new Dictionary<AilmentType, bool>(); 
            foreach (AilmentType type in Enum.GetValues(typeof(AilmentType)))
            {
                _runtime_ailmentResistant_data_dict.Add(type, resistantData.IsResistantTo(type));
            }

        }
    }

    [Serializable]
    public struct AilmentResistantDataInfo
    {
        [SerializeField]
        private bool _StunResistant;

        [SerializeField]
        private bool _ParalizedResistant;

        public bool IsResistantTo(AilmentType type)
        {
            if (type == AilmentType.Stun)
            {
                return _StunResistant;
            }
            else if (type == AilmentType.Paralized)
            {
                return _ParalizedResistant;
            }

            throw new Exception(" ResistantData " + type + "can not be found");
        }
    }
}
