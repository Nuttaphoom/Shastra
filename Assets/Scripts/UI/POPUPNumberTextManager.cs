using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Vanaring
{
    public class POPUPNumberTextManager : MonoBehaviour
    {
        public static POPUPNumberTextManager Instance;

        private Dictionary<CombatEntity, List<GameObject>> _spawnedTextDict = new Dictionary<CombatEntity, List<GameObject>>(); 
        
        private void Awake()
        {
            if (Instance != null)
                Instance = this; 

            Instance = this; 
        }

        private void Update()
        {
            if (Instance != this)
                Destroy(gameObject);

            foreach (var key in _spawnedTextDict.Keys)
            {
                for (int i = 0; i < _spawnedTextDict[key].Count; i++)
                {
                    if (_spawnedTextDict[key][i] == null)
                    {
                        _spawnedTextDict[key].RemoveAt(i);
                        i--;
                        continue;
                    }

                    _spawnedTextDict[key][i].transform.position = UISpaceSingletonHandler.ObjectToUISpace(key.CombatEntityAnimationHandler.GetGUISpawnTransform());
                }
            }
        }
        [SerializeField]
        private DestroyOnTimer _outputDMGTimerPrefab;
        [SerializeField]
        private DestroyOnTimer _outputHealTimerPrefab;
        [SerializeField]
        private DestroyOnTimer _normalTimerPrefab; 
        // Start is called before the first frame update

        public void DisplayDPOPUPDamageHealText(int accumulatedDMG, int accumulatedHP, CombatEntity showToThisEntity )
        {
            GameObject textObj; 

            if (accumulatedDMG > 0)
            {
                textObj = InstantiatedTextPrefab(_outputDMGTimerPrefab.gameObject, accumulatedDMG.ToString(), showToThisEntity);
                textObj.gameObject.SetActive(true);
                AddNewEntityToTextList(showToThisEntity, textObj.gameObject);

            }

            if (accumulatedHP > 0)
            {
                textObj = InstantiatedTextPrefab(_outputHealTimerPrefab.gameObject, accumulatedHP.ToString(), showToThisEntity);
                textObj.gameObject.SetActive(true);
                AddNewEntityToTextList(showToThisEntity, textObj.gameObject);

            }
        }

        public void DisplayPOPUPText(CombatEntity showToThisEntity, string s)
        {
            var textObj = InstantiatedTextPrefab(_normalTimerPrefab.gameObject, s, showToThisEntity);
            textObj.gameObject.SetActive(true);
            AddNewEntityToTextList(showToThisEntity, textObj.gameObject);
        }

        public GameObject InstantiatedTextPrefab(GameObject template, string text, CombatEntity showToThisEntity)
        {
            GameObject ret = MonoBehaviour.Instantiate(template, transform);
            ret.GetComponent<TextMeshProUGUI>().text = text; 
            ret.transform.position = UISpaceSingletonHandler.ObjectToUISpace(showToThisEntity.CombatEntityAnimationHandler.GetGUISpawnTransform());

            return ret; 
        }

        private void AddNewEntityToTextList(CombatEntity showToThisEntity, GameObject textGO)
        {
            if (!_spawnedTextDict.ContainsKey(showToThisEntity))
                _spawnedTextDict.Add(showToThisEntity, new List<GameObject>());

            _spawnedTextDict[showToThisEntity].Add(textGO);
        }
    }
}
