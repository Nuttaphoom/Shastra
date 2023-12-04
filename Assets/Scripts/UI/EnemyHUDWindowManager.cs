using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Vanaring
{
    public class EnemyHUDWindowManager : MonoBehaviour
    {
        [SerializeField] private EnemyHUD enemyHudTemplate;
        private List<EnemyHUD> enemyHUDList = new List<EnemyHUD>();
        private Dictionary<CombatEntity, EnemyHUD> instantiatedEnemyHUD = new Dictionary<CombatEntity, EnemyHUD>();
       
        public void DisplayEnemyHUD(List<CombatEntity> entities)
        {
            if (entities.Count <= 0)
                return;

            if (entities[0] is not AIEntity) 
                return;

            foreach (EnemyHUD hud in GetAllInstantiatedHUD())
            {
                hud.HideHUDVisual();
            }

            foreach (var combatEntity in entities)
            {
                if (!instantiatedEnemyHUD.ContainsKey(combatEntity))
                {
                    EnemyHUD newEnemyHUD = Instantiate(enemyHudTemplate, transform);
                    newEnemyHUD.Init(combatEntity) ;
                    instantiatedEnemyHUD.Add(combatEntity, newEnemyHUD );
                }
                 
                instantiatedEnemyHUD[combatEntity].DisplayHUDVisual()  ;
                
            }
        }

        public void RemoveSlotBreakHighlightOnHUD()
        {
            foreach (EnemyHUD hud in GetAllInstantiatedHUD())
            {
                hud.ClearBreakSlotHighlight();
            }
        }

        public void DisableEnemyHUD()
        {
            foreach (var hud in GetAllInstantiatedHUD()) {
                hud.HideHUDVisual(); 
            }
        }


        public void ClearEnemyHUD()
        {
            for (int i = enemyHUDList.Count - 1; i >= 0; i--)
            {
                Destroy(enemyHUDList[i]);
                enemyHUDList.RemoveAt(i);
            }
        }

     
        private List<EnemyHUD> GetAllInstantiatedHUD()
        {
            var keys = instantiatedEnemyHUD.Keys.ToList(); // Create a copy of the keys
            List<EnemyHUD> ret = new List<EnemyHUD>(); 
            for (int i = 0; i < keys.Count; i++)
            {
                var key = keys[i];
                if (instantiatedEnemyHUD[key] == null)
                {
                    instantiatedEnemyHUD.Remove(key);
                }
                else
                {
                    ret.Add(instantiatedEnemyHUD[key]);
                }
            }

            return ret; 
        }
    }
}
