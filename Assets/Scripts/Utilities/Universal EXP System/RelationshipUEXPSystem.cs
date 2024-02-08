using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Vanaring
{
    [Serializable]
    public class RelationshipUEXPSystem : BaseUEXPSystem
    {
    
        /// <summary>
        /// Formula is "4 + _currentLevel"
        /// </summary>
        /// <returns></returns>
        public override float GetEXPCap()
        { 
            return  (float) 4 + _currentLevel ;
        }

        
    }
}
