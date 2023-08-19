using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring_DepaDemo { 
    public abstract class HierarchyUIWindow : MonoBehaviour
    {
        [SerializeField]
        private Transform _graphicElement; 

        protected void SetGraphicMenuActive(bool b)
        {
            _graphicElement.gameObject.SetActive(b);
        }

        [SerializeField]
        protected CombatGraphicalHandler _combatGraphicalHandler; 
        public abstract void OnWindowDisplay(CombatGraphicalHandler combatGraphicalHandler);
        public abstract void OnWindowOverlayed();
    
    }
}
