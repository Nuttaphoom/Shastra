using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring 
{
    public abstract class BackpackItemSO : ScriptableObject
    {
        public abstract DescriptionBaseField GetDescriptionBaseField(); 


    }
}
