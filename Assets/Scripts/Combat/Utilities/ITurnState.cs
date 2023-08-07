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


        public abstract IEnumerator TakeControlSoftLeave();
        public abstract IEnumerator TurnLeave();
    }
}
