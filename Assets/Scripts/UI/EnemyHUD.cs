using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;
using UnityEditor;
using Unity.Collections;
using Unity.Jobs;
using DG.Tweening;

namespace Vanaring
{
    public class EnemyHUD : MonoBehaviour
    {
        [Header("Spawning offset regard to VFX position of the Entity")]
        [SerializeField]
        private Vector3 _spawnOffset; 

        [SerializeField]
        private GameObject _visualMesh; 

        private CombatEntity _owner;

        [SerializeField]
        private TextMeshProUGUI enemyName;

        [Header("Energy bar value")]
        private int lightScale = 1;
        private int darkScale = 1;

        [SerializeField] private Image lightImage;

        [Header("HP bar value")]
        private float hpVal;
        private float maxHP;
        [SerializeField] private Image hpImage;
        [SerializeField] private Image secondhpImage;


        [Header("EnergySlot")]
        [SerializeField] private Image lightSlotImg;
        [SerializeField] private Image darkSlotImg;
        private Color defaultSlotColor;
        private List<Image> energySlotList = new List<Image>();
        private int maxLight = 0;
        private int maxDark = 0;
        [SerializeField] private GameObject horizontalLayout;
        [SerializeField] private TextMeshProUGUI lightTmpText;
        [SerializeField] private TextMeshProUGUI darkTmpText;

        [SerializeField] private List<Image> darkSlotList;
        [SerializeField] private List<Image> lightSlotList;
        [SerializeField] private List<Image> highlightSlotList;

        public void Init(CombatEntity owner)
        {
            _owner = owner;
            _owner.SpellCaster.SubOnModifyEnergy(OnEnergyModified);
            _owner.SubOnDamageVisualEvent(OnHPModified);

            hpVal = _owner.StatsAccumulator.GetHPAmount();
            hpImage.fillAmount = hpVal / _owner.StatsAccumulator.GetPeakHPAmount();
            secondhpImage.fillAmount = hpVal / _owner.StatsAccumulator.GetPeakHPAmount();

            maxHP = _owner.StatsAccumulator.GetPeakHPAmount();

            lightScale = _owner.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.LightEnergy);
            darkScale = _owner.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.DarkEnergy);
            maxLight = _owner.SpellCaster.GetPeakEnergyAmout(RuntimeMangicalEnergy.EnergySide.LightEnergy);
            maxDark = _owner.SpellCaster.GetPeakEnergyAmout(RuntimeMangicalEnergy.EnergySide.DarkEnergy);

            lightTmpText.text = lightScale.ToString();
            darkTmpText.text = darkScale.ToString();

            DisplayEnergySlot(maxLight, maxDark, false);

            enemyName.text = _owner.CharacterSheet.CharacterName.ToString();
        }

        private void Update()
        {
            if (_visualMesh.activeSelf)
            {
                transform.position = UISpaceSingletonHandler.ObjectToUISpace(_owner.GetComponent<CombatEntityAnimationHandler>().GetGUISpawnTransform()) + _spawnOffset ;
            }
        }

        private void OnEnable()
        {
            if (_owner == null)
                return;  

            _owner.SpellCaster.SubOnModifyEnergy(OnEnergyModified);
            _owner.SubOnDamageVisualEvent(OnHPModified);
        }

        private void OnDisable()
        {
            _owner.SpellCaster.UnSubOnModifyEnergy(OnEnergyModified);
            _owner.UnSubOnDamageVisualEvent(OnHPModified);
        }

        #region Energy
        /// <summary>
        /// val = increased value
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="side"></param>
        /// <param name="val" ></param>
        private void DisplayEnergySlot(int lightVal, int darkVal, bool isBreak)
        {
            foreach (Image slot in highlightSlotList)
            {
                slot.gameObject.SetActive(false);
            }
            if (lightVal >= 0)
            {
                foreach (Image slot in lightSlotList)
                {
                    slot.gameObject.SetActive(true);
                    
                }
                for (int i = 0; i < lightSlotList.Count; i++)
                {
                    if(i < lightVal)
                    {
                        lightSlotList[i].color = Color.white;
                    }
                    else
                    {
                        if (isBreak)
                        {
                            Color defaultColor = lightSlotList[i].color;
                            defaultColor.a = 0.0f;
                            lightSlotList[i].color = defaultColor;
                        }
                        else
                        {
                            lightSlotList[i].color = Color.black;
                        }
                        
                    }
                }
            }

            if (darkVal >= 0)
            {
                foreach (Image slot in darkSlotList)
                {
                    slot.gameObject.SetActive(true);
                }
                for (int i = 0; i < darkSlotList.Count; i++)
                {
                    if (i < darkVal)
                    {
                        darkSlotList[i].color = Color.white;
                    }
                    else
                    {
                        if (isBreak && i < maxDark)
                        {
                            Color defaultColor = darkSlotList[i].color;
                            defaultColor.a = 0.0f;
                            darkSlotList[i].color = defaultColor;
                        }
                        else
                        {
                            darkSlotList[i].color = Color.black;
                        }
                    }

                }
            }
        }
        public void DisplayEnergyBreakSlotOnTarget(int energySize, int amount)
        {
            int curEnergy = _owner.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.LightEnergy);
            if(curEnergy == 0)
            {
                return;
            }

        }

        private void OnEnergyModified(CombatEntity caster, RuntimeMangicalEnergy.EnergySide side, int val)
        {
            if(val == 0)
                return;
           
            lightScale = _owner.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.LightEnergy);
            darkScale = _owner.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.DarkEnergy);
            Debug.Log(lightScale + " " + darkScale);
            DisplayEnergySlot(lightScale, darkScale, true);
            //StartCoroutine(OnModifyEnergyVisualUpdateCoroutine(caster, side, val));

        }

        private IEnumerator SlotBreak(int maxSlot, int curScale)
        {
            //Debug.Log("max= " + maxSlot + " cur= " + curScale);
            for (int i = maxSlot-1; i >= 0; i--)
            {
                if (i+1 > curScale)
                {
                    //Break
                    Color curColor = energySlotList[i].color;
                    curColor.a = 0.3f;
                    energySlotList[i].color = curColor;
                }
                else
                {
                    //Stay
                    Color curColor = energySlotList[i].color;
                    curColor.a = 1.0f;
                    energySlotList[i].color = curColor;
                }
                yield return new WaitForSeconds(0.1f);
            }
            yield return null;
        }

        private IEnumerator SlotRecovery()
        {
            int i = 0;
            foreach (Image slot in energySlotList)
            {
                //Set recovery pop
                Color curColor = energySlotList[i].color;
                curColor.a = 1.0f;
                curColor = Color.white;
                energySlotList[i].color = curColor;
                yield return new WaitForSeconds(0.1f);
                //Set default sprite
                defaultSlotColor.a = 1.0f;
                energySlotList[i].color = defaultSlotColor;
                i++;
            }
            yield return null;
        }

        public void HideHUDVisual()
        {
            if (!_visualMesh.activeSelf)
                return; 

            _visualMesh.gameObject.SetActive(false); 
        }

        public void DisplayHUDVisual()
        {
            if (_visualMesh.activeSelf)
                return; 

            _visualMesh.gameObject.SetActive(true); 
        }
         
        #endregion
        #region HP
        private void OnHPModified(int damage)
        {
            DisplayHUDVisual();

            hpVal = _owner.StatsAccumulator.GetHPAmount();

            float hptemp = maxHP == 0 ? (hpVal == 0 ? 1 : hpVal) : maxHP;

            hpImage.fillAmount = hpVal / hptemp;
            StartCoroutine(IEAnimateHPBarScale(hptemp));
        }
        
        #endregion
        #region IEnumerator
       
        private IEnumerator IEAnimateHPBarScale(float maxHP)
        {
            float tickRate = 0.5f / ((Mathf.Abs((hpVal / maxHP) - secondhpImage.fillAmount)) * 100);

            yield return new WaitForSeconds(0.5f);
            while (secondhpImage.fillAmount < hpVal / maxHP)
            {
                secondhpImage.fillAmount += 0.01f;
                yield return new WaitForSeconds(tickRate);
            }
            while (secondhpImage.fillAmount > hpVal / maxHP)
            {
                secondhpImage.fillAmount -= 0.01f;
                yield return new WaitForSeconds(tickRate);
            }

            if (hpVal <= 0)
            {
                Destroy(gameObject);
            }


            yield return null;
        }

        private IEnumerator OnModifyEnergyVisualUpdateCoroutine(CombatEntity caster, RuntimeMangicalEnergy.EnergySide side, int val)
        {
            DisplayHUDVisual();

            if (side == RuntimeMangicalEnergy.EnergySide.LightEnergy)
            {
                if (_owner.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.LightEnergy) + val >= 0)
                {
                    lightScale += val;
                    lightTmpText.text = lightScale.ToString();
                    if (val < 0)
                    {
                        yield return (SlotBreak(_owner.SpellCaster.GetPeakEnergyAmout(RuntimeMangicalEnergy.EnergySide.LightEnergy), (int)lightScale));
                    }
                    else
                    {
                        yield return (SlotRecovery());
                    }
                }
            }
            else
            {
                if (_owner.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.DarkEnergy) + val >= 0)
                {
                    darkScale += val;
                    darkTmpText.text = darkScale.ToString();
                    if (val < 0)
                    {
                        yield return (SlotBreak(_owner.SpellCaster.GetPeakEnergyAmout(RuntimeMangicalEnergy.EnergySide.DarkEnergy), (int)darkScale));
                    }
                    else
                    {
                        yield return (SlotRecovery());
                    }
                }
            }

            yield return new WaitForSeconds(0.5f); 

            HideHUDVisual(); 
        }
        #endregion
    }
}
