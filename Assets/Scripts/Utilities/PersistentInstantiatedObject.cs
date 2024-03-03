using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring 
{
    public class PersistentInstantiatedObject<BaseType> : MonoBehaviour where BaseType : MonoBehaviour
    {
        [SerializeReference]
        protected static BaseType _instance;

        public static BaseType Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (FindObjectOfType<BaseType>() == null)
                        _instance = CreateInstance();

                    else
                        _instance = FindObjectOfType<BaseType>(); 
                }

                return _instance;
            }
        }

       

        private static BaseType CreateInstance()
        {
            var go = new GameObject("" + typeof(BaseType) );
            DontDestroyOnLoad(go); 
            var component = go.AddComponent<BaseType>();
            return component;
        }


    }
}
