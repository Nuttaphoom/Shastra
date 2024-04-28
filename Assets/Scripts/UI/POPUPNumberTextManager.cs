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

                    //_spawnedTextDict[key][i].transform.position = UISpaceSingletonHandler.ObjectToUISpace(key.CombatEntityAnimationHandler.GetGUISpawnTransform());
                    //_spawnedTextDict[key][i].transform.position = RandomPointInCircle(_spawnedTextDict[key][i].transform.position, 100f);
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


        public void DisplayDamageText(int dmgToDisplay , CombatEntity showToThisEntity)
        {
            dmgToDisplay = Mathf.Abs(dmgToDisplay);
            DisplayPopUpText(dmgToDisplay.ToString(), showToThisEntity, _outputDMGTimerPrefab); 
        }
        public void DisplayHealText(int healToDisplay, CombatEntity showToThisEntity)
        {
            healToDisplay = Mathf.Abs(healToDisplay) ;
            DisplayPopUpText(healToDisplay.ToString(), showToThisEntity, _outputHealTimerPrefab);
        }
        public void DisplayDodgeText(CombatEntity showToThisEntity)
        {
            DisplayPopUpText("DODGE", showToThisEntity, _normalTimerPrefab); 
        }

        public void DisplayGeneralPOPUPText(CombatEntity showToThisEntity, string text)
        {
            DisplayPopUpText(text, showToThisEntity, _normalTimerPrefab);
        }


        private void DisplayPopUpText(string text, CombatEntity showToThisEntity, DestroyOnTimer textPrefab)
        {
            GameObject textObj;
            textObj = InstantiatedTextPrefab(textPrefab.gameObject, text, showToThisEntity);
            textObj.gameObject.SetActive(true);
            AddNewEntityToTextList(showToThisEntity, textObj.gameObject);

        }

        private GameObject InstantiatedTextPrefab(GameObject template, string text, CombatEntity showToThisEntity)
        {
            GameObject ret = MonoBehaviour.Instantiate(template, transform);
            TextMeshProUGUI tex = ret.GetComponentInChildren<TextMeshProUGUI>();
            if (tex != null)
            {
                tex.text = text;
            }

            tex.fontSize = Random.Range(50f, 85f);
            ret.transform.position = UISpaceSingletonHandler.ObjectToUISpace(showToThisEntity.CombatEntityAnimationHandler.GetTargetIconTransform());
            ret.transform.position = RandomPointInCircle(ret.transform.position, 10f);

            return ret; 
        }

        Vector2 RandomPointInCircle(Vector2 center, float radius)
        {
            float angle = Random.Range(0f, 360f);
            float x = center.x + Random.Range(3f, radius) * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = center.y + Random.Range(3f, radius) * Mathf.Sin(angle * Mathf.Deg2Rad);
            return new Vector2(x, y);
        }

        private void AddNewEntityToTextList(CombatEntity showToThisEntity, GameObject textGO)
        {
            if (!_spawnedTextDict.ContainsKey(showToThisEntity))
                _spawnedTextDict.Add(showToThisEntity, new List<GameObject>());

            _spawnedTextDict[showToThisEntity].Add(textGO);
        }
    }
}
