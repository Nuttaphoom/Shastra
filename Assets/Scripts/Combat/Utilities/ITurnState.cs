using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vanaring 
{
    public interface ITurnState
    {
        public abstract IEnumerator GetAction();

        public abstract IEnumerator TurnEnter();

    


        /// <summary>
        /// Should be call after each action called to modify used data (like status effect) (every entities are call at the same time)     
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerator TurnLeave();
    }
}
