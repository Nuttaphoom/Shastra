using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring 
{
    public class DestroyOnTimer : MonoBehaviour
    {
        [SerializeField]
        [Header("In second")] 
        private float _ttl = 0;

        private void Awake()
        {
            if (_ttl == 0)
            {
                throw new Exception("Time to live is initially set to 0, which is forbidden"); 
            }
        }

        private void Update()
        {
            if (_ttl > 0)
            {
                _ttl -= 1 * Time.deltaTime;
            }
            if (_ttl <= 0)
            {
                Destroy(gameObject) ; 
            }

        }
    }
}
