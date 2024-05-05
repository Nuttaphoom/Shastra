using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace Vanaring 
{
    public class CharacterSocketGUI : MonoBehaviour
    {
        [Header("Image")]
        [SerializeField] private Image characterImg;

        [Header("BarScaler")]
        [SerializeField] private Image hpBar, secondHpBar;
        [SerializeField] private Image mpBar, secondMpBar;
        [SerializeField] private Image outterFrame, outterFill;
        [SerializeField] private Image innerFrame, innerFill;

        [Header("TextMeshPro")]
        [SerializeField]
        private TextMeshProUGUI characterName;
        [SerializeField]
        private TextMeshProUGUI hpNumText;
        [SerializeField]
        private TextMeshProUGUI mpNumText;

        [Header("Components")]
        [SerializeField]
        private GameObject statusBarLayout;
        [SerializeField]
        private EffectIconGUI effectIcon;

        [SerializeField]
        private List<float> slotBarRatios;
        [SerializeField]
        private List<Image> fadeBlackImageList = new List<Image>();
        [SerializeField]
        private GameObject characterArrow;

        private int hpVal;
        private int maxHpVal;
        private int mpVal;
        private int maxMpVal;
        private int lightVal;
        private int darkVal;

        private CombatCharacterSheetSO _characterSheetSO;

        private Dictionary<string, EffectIconGUI> effectIconDict = new Dictionary<string, EffectIconGUI>();

        private CombatEntity _combatEntity;

         
        private void OnEnable()
        {
            if(_combatEntity != null)
            {
                _combatEntity.SubOnDamageVisualEvent(OnHPModified);
                _combatEntity.SpellCaster.SubOnModifyEnergy(OnEnergyModified);
                _combatEntity.SubOnHealVisualEvent(OnHPModified);
                _combatEntity.SpellCaster.SubOnMPModified(OnMPModified);
            }
        }

        private void OnDisable()
        {
            _combatEntity.UnSubOnDamageVisualEvent(OnHPModified);
            _combatEntity.SpellCaster.UnSubOnModifyEnergy(OnEnergyModified);
            _combatEntity.UnSubOnHealVisualEvent(OnHPModified);
            _combatEntity.SpellCaster.UnSubOnMPModified(OnMPModified);
        }

        private void Update()
        {
            
            if(_combatEntity != null)
            {
                Color imageColor = characterImg.color;
                if (_combatEntity.IsExhausted) { imageColor.a = 0.7f; }
                else { imageColor.a = 1.0f; }
                characterImg.color = imageColor;
            }
            
        }

        public void Init(CombatEntity combatEntity)
        {
            _combatEntity = combatEntity;
            effectIcon.gameObject.SetActive(false);
            SubOnEvent();

            _characterSheetSO = _combatEntity.CombatCharacterSheet;
            characterImg.sprite = _characterSheetSO.GetCharacterIcon;

            characterArrow.SetActive(false);

            characterName.text = _characterSheetSO.CharacterName;
 
            hpVal = (int) _combatEntity.StatsAccumulator.GetHPAmount();
            maxHpVal = (int)_combatEntity.StatsAccumulator.GetPeakHPAmount();
            mpVal = (int)_combatEntity.SpellCaster.GetMP;
            maxMpVal = (int)_combatEntity.SpellCaster.GetPeakMP;

            //lightVal = (int)_combatEntity.SpellCaster.GetPeakEnergyAmout(RuntimeMangicalEnergy.EnergySide.LightEnergy);
            //darkVal = (int)_combatEntity.SpellCaster.GetPeakEnergyAmout(RuntimeMangicalEnergy.EnergySide.DarkEnergy);

            lightVal = 3;
            darkVal = 3;

            secondHpBar.fillAmount = (float)hpVal / maxHpVal;

            UpdateHPScaleGUI();
            UpdateMPScaleGUI();

            InitEnergySlot();
        }

        #region SubEvent
        private void SubOnEvent()
        {
            _combatEntity.SubOnDamageVisualEvent(OnHPModified);
            _combatEntity.SpellCaster.SubOnModifyEnergy(OnEnergyModified);
            _combatEntity.SubOnHealVisualEvent(OnHPModified);
            _combatEntity.SpellCaster.SubOnMPModified(OnMPModified);
            _combatEntity.SubOnStatusEffectApplied(AddEffectIcon);
        }
        #endregion

        #region StatusEffect
        private void AddEffectIcon(EntityStatusEffectPair effect)
        {
            var statusRuntime = effect.StatusRuntime;
            var statusStackID = effect.StatusEffectFactory.Property.StackID();

            statusRuntime.SubOnStatusEffectBreak((bool isExpired) => { RemoveExpiredEffect(statusStackID, isExpired); });

            statusRuntime.SubOnStatusEffectExpire((bool isExpired) => { RemoveExpiredEffect(statusStackID, isExpired);  });

            statusRuntime.SubOnTTLUpdate((int ttl) => { UpdateEffectTTL(statusStackID, ttl); });

            //effectIcon.sprite = effect.StatusEffectFactory.StatusImage;

            Debug.Log("effectIconDict.count :  " + effectIconDict.Count);

            foreach (var key in effectIconDict.Keys)
            {
                ColorfulLogger.LogWithColor("key is " + key, Color.red) ;
            }


            if (!effectIconDict.ContainsKey(statusStackID))
            {
                //Debug.Log(statusStackID);
                EffectIconGUI newEffectIcon = Instantiate(effectIcon, statusBarLayout.transform);
                newEffectIcon.Init(statusRuntime);
                newEffectIcon.gameObject.SetActive(true);

                effectIconDict.Add(statusStackID, newEffectIcon);

                
                Debug.Log("Add new effect icon");
            }
            else
            {
                Debug.Log("Add same debuff");
                Destroy(effectIconDict[statusStackID]);
                effectIconDict.Remove(statusStackID);

                EffectIconGUI newEffectIcon = Instantiate(effectIcon, statusBarLayout.transform);
                newEffectIcon.Init(statusRuntime);
                newEffectIcon.gameObject.SetActive(true);
                effectIconDict.Add(statusStackID, newEffectIcon);
                
                //UpdateEffectTTL(statusStackID, effect.StatusRuntime.TimeToLive);
            }
        }

        private void RemoveExpiredEffect(string stackID, bool isexpire)
        {
            if (!isexpire)
                return;
            Debug.Log("Destroy effect");
            Destroy(effectIconDict[stackID]);
            effectIconDict.Remove(stackID);
        }

        private void UpdateEffectTTL(string effect, int currentTTL)
        {
            if (effectIconDict.ContainsKey(effect))
            {
                effectIconDict[effect].SetTimetoLiveText(currentTTL.ToString());
            }
            else
            {
                Debug.Log("No effect gui detect");
            }
        }
        #endregion

        private void InitEnergySlot()
        {
            //Debug.Log(_combatEntity.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.DarkEnergy) / 10f + " " 
            //    + _combatEntity.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.LightEnergy) / 10f);
            outterFrame.fillAmount = 1.0f;
            innerFrame.fillAmount = 1.0f;

            outterFill.fillAmount = 0.167f * (_combatEntity.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.DarkEnergy));
            innerFill.fillAmount = 0.167f * (_combatEntity.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.LightEnergy));
        }

        #region TurnStatus
        //public void ToggleTurnStatusDisplay(bool b)
        //{
        //    _turnStatusImage.gameObject.SetActive(b);
        //}
        #endregion
        public void ToggleOnTurnHighlightDisplay(bool b)
        {     
        }

        public void ToggleExpandSizeUI()
        {
            gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }

        public void ToggleShrinkSizeGUI()
        {
            gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }

        public void DisplayArrowOnTargetCharacter()
        {
            if (! characterArrow.activeSelf)
                characterArrow.SetActive(true);
        }

        public void HideArrowOnTargetCharacter()
        {
            if (characterArrow.activeSelf) 
                characterArrow.SetActive(false);
        }

        #region STAT
        private void UpdateHPScaleGUI()
        {
            hpBar.fillAmount = (float)hpVal / maxHpVal;
            hpNumText.text = hpVal.ToString();
            if (hpVal < 0)
            {
                hpNumText.text = "0";
            }
        }
        private void UpdateMPScaleGUI()
        {
            mpBar.fillAmount = (float)mpVal / maxMpVal;
            mpNumText.text = mpVal.ToString();
            if (mpVal < 0)
            {
                mpNumText.text = "0";
            }
        }

        private void OnHPModified(int damage)
        {
            
            hpVal = (int) _combatEntity.StatsAccumulator.GetHPAmount();
            Debug.Log(hpVal);
            if (hpVal <= 0)
            {
                foreach (Image image in fadeBlackImageList)
                {
                    Debug.Log(image);
                    Color grayColor = image.color;
                    grayColor = new Color(80.0f / 255.0f, 80.0f / 255.0f, 80.0f / 255.0f);
                    image.color = grayColor;
                }
            }
            float hptemp = maxHpVal == 0 ? (hpVal == 0 ? 1 : hpVal) : maxHpVal;
            UpdateHPScaleGUI();
            //StopAllCoroutines();
            StartCoroutine(IEAnimateBarScale(hpVal, hptemp, secondHpBar));
        }

        private void OnMPModified(float valueChange)
        {
            mpVal = (int) _combatEntity.SpellCaster.GetMP;
            if (mpVal <= 0)
            {
                mpBar.fillAmount = 0;
            }
            float mptemp = maxMpVal == 0 ? (mpVal == 0 ? 1 : mpVal) : maxMpVal;
            UpdateMPScaleGUI();
            StopAllCoroutines();
            StartCoroutine(IEAnimateBarScale(mpVal, mptemp, secondMpBar));
        }

        private void OnEnergyModified(CombatEntity caster, RuntimeMangicalEnergy.EnergySide side, int val)
        {
            Debug.Log(side + " val: " + val);
            
            if (side == RuntimeMangicalEnergy.EnergySide.LightEnergy)
            {
                Debug.Log(lightVal);
                if (lightVal > 0)
                {
                    lightVal += val;
                }
                lightVal = Mathf.Clamp(lightVal, 0, 3);
                innerFill.fillAmount = lightVal * 0.167f;
            }
            else
            {
                Debug.Log(darkVal);
                if (darkVal > 0)
                {
                    darkVal += val;
                }
                darkVal = Mathf.Clamp(darkVal, 0, 3);
                outterFill.fillAmount = darkVal * 0.167f;
            }
        }

        private IEnumerator IEAnimateBarScale(float currentVal, float maxVal, Image secondBar)
        {
            float tickRate = 0.5f / ((Mathf.Abs((currentVal / maxVal) - secondBar.fillAmount)) * 100);
            yield return new WaitForSeconds(0.5f);
            while (secondBar.fillAmount < currentVal / maxVal)
            {
                if (secondBar.fillAmount < 1.0f)
                {
                    secondBar.fillAmount += 0.01f;
                }
                else if (secondBar.fillAmount >= 1.0f)
                {
                    secondBar.fillAmount = 1.0f;
                }
                yield return new WaitForSeconds(tickRate);
            }
            while (secondBar.fillAmount > currentVal / maxVal)
            {
                if (secondBar.fillAmount > 0.0f)
                {
                    secondBar.fillAmount -= 0.01f;
                }
                else if (secondBar.fillAmount <= 0.0f)
                {
                    secondBar.fillAmount = 0.0f;
                }
                yield return new WaitForSeconds(tickRate);
            }
            yield return null;
        }

        #endregion
    }
}
