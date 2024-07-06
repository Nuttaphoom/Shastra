using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using Vanaring.Assets.Scripts.Utilities;
using UnityEngine.Events;
using Unity.VisualScripting;

namespace Vanaring
{
    public class EntityInpectWindowGUI : CombatWindowGUI
    {
        [SerializeField] private GameObject gfx;
        [SerializeField] private PlayableDirector introDirector;
        [SerializeField] private TextMeshProUGUI entityName;

        [SerializeField] private GameObject entityLevelSection;
        [SerializeField] private TextMeshProUGUI entityLevel;

        [SerializeField] private GameObject bondSection;
        [SerializeField] private TextMeshProUGUI bondValueText;

        [SerializeField] private GameObject levelSection;

        [Header("HP")]
        [SerializeField] private GameObject hpSection;
        [SerializeField] private TextMeshProUGUI hpNumText;
        [SerializeField] private Image hpFillBar;

        [Header("MP")]
        [SerializeField] private GameObject mpSection;
        [SerializeField] private TextMeshProUGUI mpNumText;
        [SerializeField] private Image mpFillBar;

        [Header("PrimaryStat")]
        [SerializeField] private TextMeshProUGUI strStatText;
        [SerializeField] private TextMeshProUGUI vitStatText;
        [SerializeField] private TextMeshProUGUI intStatText;
        [SerializeField] private TextMeshProUGUI agiStatText;
        [SerializeField] private TextMeshProUGUI lckStatText;

        [SerializeField] private GameObject entityButtonTemplate;
        [SerializeField] private ControlBox currentControlBoxTemplate;
        [SerializeField] private GameObject hrzControlBox;

        private List<GameObject> allyButtonList = new List<GameObject>();
        private List<GameObject> enemyButtonList = new List<GameObject>();
        private List<ControlBox> controlBoxList = new List<ControlBox>();
        private List<SocketGUI> effSocketList = new List<SocketGUI>();
        private List<CombatEntity> entityList = new List<CombatEntity>();
        private int allyIndex = 0;
        private bool isAllyMode = true;
        [SerializeField] private GameObject allyHRZ;
        [SerializeField] private GameObject enemyHRZ;
        [SerializeField] private GameObject effVTCL;
        [SerializeField] private SocketGUI effSocket;

        #region EventBroadcast
        private EventBroadcaster _eventBroadcaster;

        private EventBroadcaster GetEventBroadcaster()
        {
            if (_eventBroadcaster == null)
            {
                _eventBroadcaster = new EventBroadcaster();
                _eventBroadcaster.OpenChannel<CombatEntity>("OnEntityInspect");
                _eventBroadcaster.OpenChannel<Null>("OnCloseInspectWindow");
            }

            return _eventBroadcaster;
        }

        public void SubOnEntityInspect(UnityAction<CombatEntity> argc)
        {
            GetEventBroadcaster().SubEvent<CombatEntity>(argc, "OnEntityInspect");
        }

        public void UnSubOnEntityInspect(UnityAction<CombatEntity> argc)
        {
            GetEventBroadcaster().UnSubEvent<CombatEntity>(argc, "OnEntityInspect");
        }

        public void SubOnCloseInspectWindow(UnityAction<Null> argc)
        {
            GetEventBroadcaster().SubEvent<Null>(argc, "OnCloseInspectWindow");
        }

        public void UnSubOnCloseInspectWindow(UnityAction<Null> argc)
        {
            GetEventBroadcaster().UnSubEvent<Null>(argc, "OnCloseInspectWindow");
        }
        #endregion EventBroadcast

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
                foreach (var item in controlBoxList)
                {
                    Destroy(item.gameObject);
                }
                controlBoxList.Clear();
                entityList.Clear();

                foreach (CombatEntity entity in CombatReferee.Instance.GetCompetatorsBySide(ECompetatorSide.Ally))
                {
                    GameObject newAllyButton = Instantiate(entityButtonTemplate, allyHRZ.transform);
                    newAllyButton.GetComponent<Button>().onClick.AddListener(() => LoadAllyEntityDetail(entity, true));
                    newAllyButton.GetComponentInChildren<Image>().sprite = entity.CombatCharacterSheet.GetCharacterIcon;
                    newAllyButton.gameObject.SetActive(true);
                    allyButtonList.Add(newAllyButton);
                    entityList.Add(entity);
                }
                foreach (CombatEntity entity in CombatReferee.Instance.GetCompetatorsBySide(ECompetatorSide.Hostile))
                {
                    GameObject newEnemyButton = Instantiate(entityButtonTemplate, enemyHRZ.transform);

                    newEnemyButton.GetComponent<Button>().onClick.AddListener(() => LoadAllyEntityDetail(entity, false));
                    newEnemyButton.gameObject.SetActive(true);
                    enemyButtonList.Add(newEnemyButton);
                    entityList.Add(entity);
                }

                int count = CombatReferee.Instance.GetCompetatorsBySide(ECompetatorSide.Ally).Count + CombatReferee.Instance.GetCompetatorsBySide(ECompetatorSide.Hostile).Count;

                for (int i = 0; i < count; i++)
                {
                    ControlBox newBox = Instantiate(currentControlBoxTemplate, hrzControlBox.transform);
                    newBox.SetActiveImage(false);
                    newBox.gameObject.SetActive(true);
                    controlBoxList.Add(newBox);
                }
                currentControlBoxTemplate.gameObject.SetActive(false);
                controlBoxList[0].SetActiveImage(true);

                entityButtonTemplate.gameObject.SetActive(false);
            }

            SetupInfo();
        }
        public void ClosePanel()
        {
            _windowManager.OpenWindow(EWindowGUI.Main);
        }

        public void InvokeEvent()
        {

        }
        

        private void LoadAllyEntityDetail(CombatEntity entity, bool isAlly)
        {
            entityLevelSection.gameObject.SetActive(isAlly);
            mpSection.gameObject.SetActive(isAlly);
            bondSection.SetActive(isAlly);
            levelSection.SetActive(isAlly);

            entityName.text = entity.CombatCharacterSheet.CharacterName;
            hpNumText.text = (int)entity.StatsAccumulator.GetHPAmount() + "/" + entity.StatsAccumulator.GetPeakHPAmount();
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
                    newSocket.Init(desc.FieldName, eff.TimeToLive.ToString(), desc.FieldImage, desc.FieldDescription);
                    newSocket.gameObject.SetActive(true);
                    effSocketList.Add(newSocket);
                }
            }
            if (entity.GetAilmentHandler.GetCurrentAilment != null)
            {
                SocketGUI newSocket = Instantiate(effSocket, effVTCL.transform);
                DescriptionBaseField desc = entity.GetAilmentHandler.GetCurrentAilment.GetDescription;
                newSocket.Init(desc.FieldName, "", desc.FieldImage, desc.FieldDescription);
                newSocket.gameObject.SetActive(true);
                effSocketList.Add(newSocket);
            }
            strStatText.text = "" + entity.CombatCharacterSheet.GetStrength;
            vitStatText.text = "" + entity.CombatCharacterSheet.GetVitality;
            intStatText.text = "" + entity.CombatCharacterSheet.GetIntellect;
            agiStatText.text = "" + entity.CombatCharacterSheet.GetAgility;
            lckStatText.text = "" + entity.CombatCharacterSheet.GetLuck;

            if (isAlly)
            {
                if (!EnableDebuggingChecker.Instance.IsDebugingModeEnable )
                {
                    foreach (RuntimeCombatMemberData cmember in PersistentPlayerPersonalDataManager.Instance.CombatMemberDataLocator.GetRuntimeCombatMembers)
                    {
                        entityLevel.text = "Lv." + cmember.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentLevel.ToString();
                    }
                }

                mpNumText.text = entity.SpellCaster.GetMP + "/" + entity.SpellCaster.GetPeakMP;
                mpFillBar.fillAmount = (float)entity.SpellCaster.GetMP / entity.SpellCaster.GetPeakMP;
            }
        }

        private void SetupInfo()
        {
            foreach (var item in controlBoxList)
            {
                item.SetActiveImage(false);
            }
            if (isAllyMode)
            {
                controlBoxList[allyIndex].SetActiveImage(true);
                allyButtonList[allyIndex].GetComponent<Button>().onClick.Invoke();
                
            }
            else
            {
                controlBoxList[3+allyIndex].SetActiveImage(true);
                enemyButtonList[allyIndex].GetComponent<Button>().onClick.Invoke();
                
            }
        }

        public override void ClearData()
        {
        }

        public override void OnWindowActive()
        {
            allyIndex = 0;
            isAllyMode = true;
            _eventBroadcaster.InvokeEvent<CombatEntity>(entityList[allyIndex], "OnEntityInspect");
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
                allyIndex--;// = Math.Clamp(allyIndex - 1, 0, 2);

                if (!isAllyMode && allyIndex < 0)
                {
                    allyIndex = 2;
                    isAllyMode = true;
                }

                if (allyIndex < 0)
                {
                    allyIndex = 0;
                }
                if (isAllyMode)
                {
                    _eventBroadcaster.InvokeEvent<CombatEntity>(entityList[allyIndex], "OnEntityInspect");
                }
                else
                {
                    _eventBroadcaster.InvokeEvent<CombatEntity>(entityList[3 + allyIndex], "OnEntityInspect");
                }

                SetupInfo();
            }
            if (key == InputCode.Right)
            {
                allyIndex++; //= Math.Clamp(allyIndex + 1, 0, 2);
                
                if(!isAllyMode && allyIndex+1 > enemyButtonList.Count)
                {
                    allyIndex--;
                }

                if(allyIndex > 2)
                {
                    allyIndex = 0;
                    isAllyMode = false;
                }
                if (isAllyMode)
                {
                    _eventBroadcaster.InvokeEvent<CombatEntity>(entityList[allyIndex], "OnEntityInspect");
                }
                else
                {
                    _eventBroadcaster.InvokeEvent<CombatEntity>(entityList[3 + allyIndex], "OnEntityInspect");
                }

                SetupInfo();
            }
            if(key == InputCode.DeSelect )
            {
                _eventBroadcaster.InvokeEvent<Null>(null, "OnCloseInspectWindow");
                _windowManager.OpenWindow(EWindowGUI.Main);
            }
        }
    }
}
