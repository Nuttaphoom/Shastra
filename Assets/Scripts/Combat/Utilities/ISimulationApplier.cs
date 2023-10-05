using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vanaring
{
    public interface ISimulationApplier<T,U,V> 
    {
        public void Simulate(T argc, U argv, V arga) ;
        public bool CheckSimulation() ; 
    }

}


 