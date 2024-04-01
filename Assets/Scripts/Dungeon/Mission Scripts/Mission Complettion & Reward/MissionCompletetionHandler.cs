using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vanaring 
{
    public class MissionCompletetionHandler
    {
        public IEnumerator ResolveMissionCompleteStatus(MissionCompleteStatus missionCompleteStatus)
        {
            ///Display nescesary UI and stuffs before 
            ///
            yield return null;
            ///backend get reward for the party 
            ///

            yield return null; 

        }
    }

    public struct MissionCompleteStatus
    {
        /// <summary>
        /// TRUE => Dungeon is ready to exit, not nescessary complete
        /// </summary>
        public bool ExitDungeon ;
        /// <summary>
        /// Kill all necessary monster or quest
        /// </summary>
        public bool CompleteDungeon; 
    }
}
