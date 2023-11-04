using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Vanaring
{
    public class POPUPNumberTextManager : MonoBehaviour
    {
        public static POPUPNumberTextManager Instance; 
        private void Awake()
        {
            if (Instance != null)
                Instance = this; 

            Instance = this; 
        }

        [SerializeField]
        private DestroyOnTimer _outputDMGTimerPrefab;
        [SerializeField]
        private DestroyOnTimer _outputHealTimerPrefab;
        // Start is called before the first frame update

        public void DisplayDPOPUPText(int accumulatedDMG, int accumulatedHP, CombatEntity showToThisEntity )
        {
            Debug.Log("display");
            if (accumulatedDMG > 0)
            {
                var v = MonoBehaviour.Instantiate(_outputDMGTimerPrefab, transform);
                v.GetComponent<TextMeshProUGUI>().text = accumulatedDMG.ToString();
                v.transform.position = UISpaceSingletonHandler.ObjectToUISpace(showToThisEntity.CombatEntityAnimationHandler.GetGUISpawnTransform());

                v.gameObject.SetActive(true);
            }

            if (accumulatedHP > 0)
            {
                var vv = MonoBehaviour.Instantiate(_outputHealTimerPrefab, transform);
                vv.GetComponent<TextMeshProUGUI>().text = accumulatedHP.ToString();
                vv.transform.position = UISpaceSingletonHandler.ObjectToUISpace(showToThisEntity.CombatEntityAnimationHandler.GetGUISpawnTransform());
                
                vv.gameObject.SetActive(true);

            }
        }
    }
}
