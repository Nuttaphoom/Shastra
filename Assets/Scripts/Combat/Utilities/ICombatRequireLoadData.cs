using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vanaring.Assets.Scripts.Combat.Utilities
{
    public interface ICombatRequireLoadData  
    {

        /// <summary>
        /// this will be called from CombatReferee
        /// For loading data from main runtime Database like in PersistentPersonal into designated class 
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadDataFromDatabase() ;
    }
}
