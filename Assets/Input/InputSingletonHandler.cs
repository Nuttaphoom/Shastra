using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class InputSingletonHandler : MonoBehaviour
    {
        private static InputSingletonHandler instance = null;
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else if (this != instance)
                Destroy(this);
        }

        void Update()
        {
            StartCoroutine(CentralInputReceiver.Instance().CustomUpdate());
        }
    }
}
