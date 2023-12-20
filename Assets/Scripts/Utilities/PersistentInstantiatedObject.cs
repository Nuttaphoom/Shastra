using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring.Assets.Scripts.Utilities
{
    public class PersistentInstantiatedObject<BaseType> : MonoBehaviour where BaseType : MonoBehaviour
    {
        [SerializeReference]
        private static BaseType _instance;

        public static BaseType Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = CreateInstance();
                }

                return _instance;
            }
        }

        private static BaseType CreateInstance()
        {
            var go = new GameObject(""+ typeof(BaseType) );
            DontDestroyOnLoad(go);
            var component = go.AddComponent<BaseType>();
            return component;
        }


    }
}
