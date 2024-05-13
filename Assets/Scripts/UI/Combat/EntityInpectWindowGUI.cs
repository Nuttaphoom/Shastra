using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Playables;

namespace Vanaring
{
    public class EntityInpectWindowGUI : MonoBehaviour, IInputReceiver
    {
        [SerializeField] private GameObject gfx;
        [SerializeField] private PlayableDirector introDirector;
        [SerializeField] private TextMeshProUGUI entityName;

        [SerializeField] private GameObject entityLevelSection;
        [SerializeField] private TextMeshProUGUI entityLevel;

        [Header("HP")]
        [SerializeField] private GameObject hpSection;
        [SerializeField] private TextMeshProUGUI hpNumText;
        [SerializeField] private Image hpFillBar;

        [Header("MP")]
        [SerializeField] private GameObject mpSection;
        [SerializeField] private TextMeshProUGUI mpNumText;
        [SerializeField] private Image mpFillBar;

        [Header("PrimaryStat")]
        [SerializeField] private GameObject priStatSection;
        [SerializeField] private TextMeshProUGUI strStatText;
        [SerializeField] private TextMeshProUGUI vitStatText;
        [SerializeField] private TextMeshProUGUI intStatText;
        [SerializeField] private TextMeshProUGUI agiStatText;
        [SerializeField] private TextMeshProUGUI lckStatText;

        [SerializeField] private GameObject entityButtonTemplate;

        private List<GameObject> allyButtonList = new List<GameObject>();
        private List<GameObject> enemyButtonList = new List<GameObject>();
        private int allyIndex = 0;
        private bool isAllyMode = true;
        [SerializeField] private GameObject allyHRZ;
        [SerializeField] private GameObject enemyHRZ;

        [ContextMenu("Init")]
        public void Init()
        {
            gfx.SetActive(true);
            if (CombatReferee.Instance != null)
            {
                foreach (var item in allyButtonList)
                {
                    Destroy(item);
                }
                allyButtonList.Clear();
                foreach (var item in enemyButtonList)
                {
                    Destroy(item);
                }
                enemyButtonList.Clear();
                foreach (CombatEntity entity in CombatReferee.Instance.GetCompetatorsBySide(ECompetatorSide.Ally))
                {
                    GameObject newAllyButton = Instantiate(entityButtonTemplate, allyHRZ.transform);
                    newAllyButton.GetComponent<Button>().onClick.AddListener(() => LoadAllyEntityDetail(entity, true));
                    newAllyButton.GetComponentInChildren<Image>().sprite = entity.CombatCharacterSheet.GetCharacterIcon;
                    newAllyButton.gameObject.SetActive(true);
                    allyButtonList.Add(newAllyButton);
                }
                foreach (CombatEntity entity in CombatReferee.Instance.GetCompetatorsBySide(ECompetatorSide.Hostile))
                {
                    GameObject newEnemyButton = Instantiate(entityButtonTemplate, enemyHRZ.transform);
                    newEnemyButton.GetComponent<Button>().onClick.AddListener(() => LoadAllyEntityDetail(entity, false));
                    newEnemyButton.gameObject.SetActive(true);
                    enemyButtonList.Add(newEnemyButton);
                }
                entityButtonTemplate.gameObject.SetActive(false);
            }

            SetupInfo();
            CentralInputReceiver.Instance().AddInputReceiverIntoStack(this);
        }

        public void ClosePanel()
        {
            CentralInputReceiver.Instance().RemoveInputReceiverIntoStack(this);
            gfx.SetActive(false);
        }

        private void LoadAllyEntityDetail(CombatEntity entity, bool isAlly)
        {
            entityLevelSection.gameObject.SetActive(isAlly);
            mpSection.gameObject.SetActive(isAlly);
            priStatSection.gameObject.SetActive(isAlly);

            entityName.text = entity.CombatCharacterSheet.CharacterName;
            hpNumText.text = "HP: " + entity.StatsAccumulator.GetHPAmount() + "/" + entity.StatsAccumulator.GetPeakHPAmount();
            hpFillBar.fillAmount = (float)entity.StatsAccumulator.GetHPAmount() / entity.StatsAccumulator.GetPeakHPAmount();

            if (isAlly)
            {
                
                mpNumText.text = "MP: " + entity.SpellCaster.GetMP + "/" + entity.SpellCaster.GetPeakMP;
                mpFillBar.fillAmount = (float)entity.SpellCaster.GetMP / entity.SpellCaster.GetPeakMP;

                strStatText.text = "STR " + entity.CombatCharacterSheet.GetModStrength;
                vitStatText.text = "VIT " + entity.CombatCharacterSheet.GetModVitality;
                intStatText.text = "INT " + entity.CombatCharacterSheet.GetModIntellect;
                agiStatText.text = "AGI " + entity.CombatCharacterSheet.GetModAgility;
                lckStatText.text = "LCK " + entity.CombatCharacterSheet.GetModLuck;
            }
        }

        public void ReceiveKeys(KeyCode key)
        {
            if (key == KeyCode.A)
            {
                allyIndex = Math.Clamp(allyIndex - 1, 0, 2);
                SetupInfo();
            }
            if (key == KeyCode.D)
            {
                allyIndex = Math.Clamp(allyIndex + 1, 0, 2);
                SetupInfo();
            }
            if (key == KeyCode.C)
            {
                allyIndex = 0;
                isAllyMode = !isAllyMode;
                SetupInfo();
            }
        }

        private void SetupInfo()
        {
            if (isAllyMode)
            {
                allyButtonList[allyIndex].GetComponent<Button>().onClick.Invoke();
            }
            else
            {
                enemyButtonList[allyIndex].GetComponent<Button>().onClick.Invoke();
            }
        }
    }
}
