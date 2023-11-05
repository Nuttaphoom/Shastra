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
        // Start is called before the first frame update

        public void DisplayDPOPUPText(int accumulatedDMG, int accumulatedHP, CombatEntity showToThisEntity )
        {
            if (accumulatedDMG > 0)
            {
                _outputDMGTimerPrefab.gameObject.SetActive(true);
                var v = MonoBehaviour.Instantiate(_outputDMGTimerPrefab, transform);
                v.GetComponent<TextMeshProUGUI>().text = accumulatedDMG.ToString();
                v.transform.position = UISpaceSingletonHandler.ObjectToUISpace(showToThisEntity.CombatEntityAnimationHandler.GetGUISpawnTransform());

                AddNewEntityToTextList(showToThisEntity, v.gameObject);

                _outputDMGTimerPrefab.gameObject.SetActive(false);
            }

            if (accumulatedHP > 0)
            {
                _outputHealTimerPrefab.gameObject.SetActive(true);
                var vv = MonoBehaviour.Instantiate(_outputHealTimerPrefab, transform);
                vv.GetComponent<TextMeshProUGUI>().text = accumulatedHP.ToString();
                vv.transform.position = UISpaceSingletonHandler.ObjectToUISpace(showToThisEntity.CombatEntityAnimationHandler.GetGUISpawnTransform());
                
                vv.gameObject.SetActive(true);

                AddNewEntityToTextList(showToThisEntity, vv.gameObject);

                _outputHealTimerPrefab.gameObject.SetActive(false) ;
            }
        }

        private void AddNewEntityToTextList(CombatEntity showToThisEntity, GameObject textGO)
        {
            if (!_spawnedTextDict.ContainsKey(showToThisEntity))
                _spawnedTextDict.Add(showToThisEntity, new List<GameObject>());

            _spawnedTextDict[showToThisEntity].Add(textGO);
        }
    }
}
