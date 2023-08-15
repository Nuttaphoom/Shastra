using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vanaring_DepaDemo 
{
    public interface ITurnState
    {
        public abstract IEnumerator GetAction();

        public abstract IEnumerator TurnEnter();

        public abstract IEnumerator AfterGetAction(); 
        public abstract IEnumerator TakeControlSoftLeave();


        /// <summary>
        /// Should be call after each action called to modify used data (like status effect) 
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerator TurnLeave();
    }
}
