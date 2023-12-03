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

        //Display what reference(list) want to
        //public void DisplayEnemyHUD(List<CombatEntity> entities)
        //{

        //    if(entities.Count < 1)
        //    {
        //        Debug.Log("No enemy on field/detect");
        //        return;
        //    }
        //    if(enemyHUDList.Count > 0)
        //    {
        //        Debug.Log(enemyHUDList.Count);
        //        DisableEnemyHUD();
        //        SelectHUDToDisplay(entities);
        //    }
        //    else    //Create new instead
        //    {
        //        foreach (CombatEntity entity in entities)
        //        {
        //            EnemyHUD newEnemyHUD = Instantiate(enemyHudTemplate, transform);
        //            newEnemyHUD.Init(entity);
        //            SetUITransform(newEnemyHUD, entity);
        //            enemyHUDList.Add(newEnemyHUD);
        //            //newEnemyHUD.gameObject.SetActive(false);
        //        }
        //    }
        //}

       
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

            //foreach (var key in instantiatedEnemyHUD.Keys)
            //{
            //    if (!entities.Contains(key))
            //        instantiatedEnemyHUD[key].HideHUDVisual(); // .SetActive(false);
            //}

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
