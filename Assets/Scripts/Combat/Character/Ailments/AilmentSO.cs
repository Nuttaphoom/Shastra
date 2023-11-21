using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Vanaring
{ 

    public abstract class Ailment
    {
        protected  int _ttl = 0;

        public bool AlimentExpired()
        {
            return (_ttl <= 0);
        }

        public void UpdateTTL()
        {
            _ttl -= 1;
        }

        public abstract IEnumerator TakeControlEntity();

    }

    /// <summary>
    /// Ailment status effect will be completely different than normal status effect as it controls behavior of the patient 
    /// Right now in control only when owner's control enter is called 
    /// *** If Ailment is expired ,let's say, in Turn 1 (the effect affects still), it will be removed from combat entity at the begining of Turn 2
    /// </summary>
    public abstract class Ailment<T,U,V> : Ailment
    {
        public Ailment(int ttl) {
            _ttl = ttl;    
        }


        public abstract IEnumerable AilmentRecover(); 
        public abstract void InitAilment(T t, U u, V v);   
    }

    




}
