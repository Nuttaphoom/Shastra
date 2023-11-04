using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class EnemyHUDWindowManager : MonoBehaviour
    {
        [SerializeField] private EnemyHUD enemyHudTemplate;
        private List<EnemyHUD> enemyHUDList = new List<EnemyHUD>();

        private void Start()
        {
            StartCoroutine(InitTest());
        }

        //Display what reference(list) want to
        public void DisplayEnemyHUD(List<CombatEntity> entities)
        {
 
            if(entities.Count < 1)
            {
                Debug.Log("No enemy on field/detect");
                return;
            }
            if(enemyHUDList.Count > 0)
            {
                Debug.Log(enemyHUDList.Count);
                DisableEnemyHUD();
                SelectHUDToDisplay(entities);
            }
            else    //Create new instead
            {
                foreach (CombatEntity entity in entities)
                {
                    EnemyHUD newEnemyHUD = Instantiate(enemyHudTemplate, transform);
                    newEnemyHUD.Init(entity);
                    SetUITransform(newEnemyHUD, entity);
                    enemyHUDList.Add(newEnemyHUD);
                    //newEnemyHUD.gameObject.SetActive(false);
                }
            }
        }

        private void SelectHUDToDisplay(List<CombatEntity> entities)
        {
            foreach (EnemyHUD hud in enemyHUDList)
            {
                CombatEntity owner = entities.Find(entity => entity == hud.GetOwner());

                if (owner != null)
                {
                    hud.gameObject.SetActive(true);
                }
                else
                {
                    hud.gameObject.SetActive(false);
                }
            }
        }

        private void SetUITransform(EnemyHUD hud ,CombatEntity entity)
        {
            Vector3 screen = UISpaceSingletonHandler.ObjectToUISpace(entity.transform);
            hud.transform.position = screen;
            hud.transform.position += new Vector3(0, 200, 0);
        }


        private void Update()
        {
            if (enemyHUDList.Count > 0)
            {
                SetAllUITransform();
            }
        }

        private void SetAllUITransform()
        {
            int i = 0;
            foreach (CombatEntity entity in CombatReferee.instance.GetCompetatorsBySide(ECompetatorSide.Hostile))
            {
                if(enemyHUDList[i] == null)
                {
                    Debug.Log("Enemy didn't load their HUD");
                    throw new Exception("EnenmyHUD is not load for this entity:" + entity);
                }
                else
                {
                    SetUITransform(enemyHUDList[i], entity);
                }
                i++;
            } 
        }

        private void DisableEnemyHUD()
        {
            foreach (EnemyHUD enemyHUD in enemyHUDList)
            {
                enemyHUD.gameObject.SetActive(false);
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

        private void OnUpdateValue()
        {

        }

        private IEnumerator InitTest()
        {
            if(enemyHUDList.Count > 0)
            {
                ClearEnemyHUD();
            }
            yield return new WaitForSeconds(1.0f);
            DisplayEnemyHUD(CombatReferee.instance.GetCompetatorsBySide(ECompetatorSide.Hostile));
        }
    }
}
