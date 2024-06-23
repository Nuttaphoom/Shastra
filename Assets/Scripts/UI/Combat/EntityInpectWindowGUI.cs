using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Playables;

namespace Vanaring
{
    public class EntityInpectWindowGUI : CombatWindowGUI
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
        private List<SocketGUI> effSocketList = new List<SocketGUI>();
        private int allyIndex = 0;
        private bool isAllyMode = true;
        [SerializeField] private GameObject allyHRZ;
        [SerializeField] private GameObject enemyHRZ;
        [SerializeField] private GameObject effVTCL;
        [SerializeField] private SocketGUI effSocket;
        public bool isDebugingMode = true;

        [ContextMenu("Init")]
        public void Init()
        {
            gfx.SetActive(true);
            effSocket.gameObject.SetActive(false);
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
        }

        public void ClosePanel()
        {
            _windowManager.OpenWindow(EWindowGUI.Main);
        }

        private void LoadAllyEntityDetail(CombatEntity entity, bool isAlly)
        {
            return;
            entityLevelSection.gameObject.SetActive(isAlly);
            mpSection.gameObject.SetActive(isAlly);
            priStatSection.gameObject.SetActive(isAlly);

            entityName.text = entity.CombatCharacterSheet.CharacterName;
            hpNumText.text = "HP: " + entity.StatsAccumulator.GetHPAmount() + "/" + entity.StatsAccumulator.GetPeakHPAmount();
            hpFillBar.fillAmount = (float)entity.StatsAccumulator.GetHPAmount() / entity.StatsAccumulator.GetPeakHPAmount();

            foreach (var item in effSocketList)
            {
                Destroy(item.gameObject);
            }
            effSocketList.Clear();
            foreach (string key in entity.StatusEffectHandler.Effects.Keys)
            {
                List<StatusRuntimeEffect> effs = entity.StatusEffectHandler.Effects[key];
                foreach (StatusRuntimeEffect eff in effs)
                {
                    SocketGUI newSocket = Instantiate(effSocket, effVTCL.transform);
                    DescriptionBaseField desc = eff.GetStatusEffectDescription();
                    newSocket.Init(desc.FieldName, eff.TimeToLive.ToString(), desc.FieldImage);
                    newSocket.gameObject.SetActive(true);
                    effSocketList.Add(newSocket);
                }
            }

            if (isAlly)
            {
                if (!isDebugingMode)
                {
                    foreach (RuntimeCombatMemberData cmember in PersistentPlayerPersonalDataManager.Instance.CombatMemberDataLocator.GetRuntimeCombatMembers)
                    {
                        entityLevel.text = "Lv." + cmember.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentLevel.ToString();
                    }
                }


                mpNumText.text = "MP: " + entity.SpellCaster.GetMP + "/" + entity.SpellCaster.GetPeakMP;
                mpFillBar.fillAmount = (float)entity.SpellCaster.GetMP / entity.SpellCaster.GetPeakMP;

                strStatText.text = "STR " + entity.CombatCharacterSheet.GetModStrength;
                vitStatText.text = "VIT " + entity.CombatCharacterSheet.GetModVitality;
                intStatText.text = "INT " + entity.CombatCharacterSheet.GetModIntellect;
                agiStatText.text = "AGI " + entity.CombatCharacterSheet.GetModAgility;
                lckStatText.text = "LCK " + entity.CombatCharacterSheet.GetModLuck;
            }
        }

        private Transform FindGameObjectTransformByName(Transform transform, string name)
        {
            return transform.Find(name);
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

        public override void ClearData()
        {
        }

        public override void OnWindowActive()
        {
        }

        public override void OnWindowDeActive()
        {
        }

        public override void LoadWindowData(CombatEntity entity)
        {
            Init();
        }

        public override void ReceiveKeysFromWindowManager(InputCode key)
        {
            if (key == InputCode.Left)
            {
                allyIndex = Math.Clamp(allyIndex - 1, 0, 2);
                SetupInfo();
            }
            if (key == InputCode.Right)
            {
                allyIndex = Math.Clamp(allyIndex + 1, 0, 2);
                SetupInfo();
            }
            if (key == InputCode.C)
            {
                allyIndex = 0;
                isAllyMode = !isAllyMode;
                SetupInfo();
            }
            if(key == InputCode.Skill || key == InputCode.InspectionOpen)
            {
                _windowManager.OpenWindow(EWindowGUI.Main);
            }
            //if (key == KeyCode.T)
            //{
            //    ClosePanel();
            //}
        }
    }
}
