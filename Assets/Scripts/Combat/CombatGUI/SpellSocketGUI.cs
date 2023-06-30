using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring_DepaDemo
{
    
    public class SpellSocketGUI : MonoBehaviour
    {
         
        private SpellAbilitySO _spellSO; 

        public void Init(SpellAbilitySO spell)
        {
            _spellSO = spell; 

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent(out Button button))
                {
                    button.onClick.AddListener(() => { Debug.Log("hi!"); });
                } 
            }

        }

        
    }
}
