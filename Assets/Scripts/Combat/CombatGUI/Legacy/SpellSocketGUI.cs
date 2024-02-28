using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

namespace Vanaring 
{
     
    public class SpellSocketGUI : MonoBehaviour 
    {

      

        [Header("Main Skill Information")]
        [SerializeField]
        private Button _actionButton ;

        [SerializeField]
        private TextMeshProUGUI _textMeshProUGUI;

        [SerializeField]
        private Image _skillImage;

        [SerializeField]
        private Image _spellEnergyCostType;

        [SerializeField]
        private TextMeshProUGUI _spellCost;

        [Header("Spell Socket Layer")]
        [SerializeField] private Image _fadeBlack;
        [SerializeField] private GameObject highlightImage;
        [SerializeField] private GameObject unhighlightImage;

        [Header("Slot Layout")]
        [SerializeField] private Image _lightSlotReqImage;
        [SerializeField] private Image _darkSlotReqImage;

        [SerializeField] private GameObject VerticalSlotLayout;
        [SerializeField] private GameObject HorizontalSlotLayout;
        [SerializeField] private GameObject HorizontalSlotLayout2;

        private SpellActionSO _spellSO;
        private CombatEntity _caster;

        public void Init(SpellActionSO spell, CombatEntity combatEntity)
        {
            int _spellRequireAmout = (int) MathF.Abs(spell.RequiredAmout) ;
            _spellSO = spell;
            this._caster = combatEntity;
            _actionButton.onClick.AddListener(ChooseSpell);
            //Debug.Log("Spell : " + spell) ;
            //Debug.Log("spell.AbilityName : " + spell.AbilityName) ;  
            _textMeshProUGUI.text = spell.AbilityName.ToString();
            _spellCost.text = _spellRequireAmout.ToString();
            //_skillImage.sprite = spell.AbilityImage;
            _fadeBlack.gameObject.SetActive(false); ;

            //init slot layout
            if (spell.RequiredSide == RuntimeMangicalEnergy.EnergySide.LightEnergy)
            {
                for (int i = 0; i < _spellRequireAmout; i++)
                {
                    if(_spellRequireAmout <= 3)
                    {
                        Image slot = Instantiate(_lightSlotReqImage, HorizontalSlotLayout.transform);
                        slot.gameObject.SetActive(true);
                    }
                    else
                    {
                        if (i < 2)
                        {
                            Image slot = Instantiate(_lightSlotReqImage, HorizontalSlotLayout.transform);
                            slot.gameObject.SetActive(true);
                        }
                        else
                        {
                            HorizontalSlotLayout2.gameObject.SetActive(true);
                            Image slot = Instantiate(_lightSlotReqImage, HorizontalSlotLayout2.transform);
                            slot.gameObject.SetActive(true);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < _spellRequireAmout; i++)
                {
                    if (_spellRequireAmout <= 3)
                    {
                        Image slot = Instantiate(_darkSlotReqImage, HorizontalSlotLayout.transform);
                        slot.gameObject.SetActive(true);
                    }
                    else
                    {
                        if (i < 2)
                        {
                            Image slot = Instantiate(_darkSlotReqImage, HorizontalSlotLayout.transform);
                            slot.gameObject.SetActive(true);
                        }
                        else
                        {
                            HorizontalSlotLayout2.gameObject.SetActive(true);
                            Image slot = Instantiate(_darkSlotReqImage, HorizontalSlotLayout2.transform);
                            slot.gameObject.SetActive(true);
                        }
                    }
                }
            }
            HorizontalSlotLayout.gameObject.SetActive(true);
        }

        public string GetSpellDescription()
        {
            return _spellSO.Desscription.ToString();
        }

        private void ChooseSpell()
        {
            if (IsEnergySufficeientToUseThisSpell())
            {
                _caster.SpellCaster.CastSpell(_spellSO)  ; 
            }
        }

        public void CallButtonCallback()
        {
             _actionButton.onClick?.Invoke(); 
        }

        public void HightlightedButton()
        {
            //_fadeBlack.gameObject.SetActive(false);
            highlightImage.SetActive(true);
        }

        public void FadeBlackButton(bool black)
        {
            _fadeBlack.gameObject.SetActive(black);
        }

        public void RedHightlightedButton()
        {
            //_fadeBlack.gameObject.SetActive(true);
            unhighlightImage.SetActive(true);
        }

        public void UnHighlightedButton()
        {
            //_fadeBlack.gameObject.SetActive(true);
            highlightImage.SetActive(false);
            unhighlightImage.SetActive(false);
        }

        public void DisableHighlightedButton()
        {
            _fadeBlack.gameObject.SetActive(true);
            _actionButton.GetComponent<Image>().color = Color.gray ;
        }
        public bool IsEnergySufficeientToUseThisSpell()
        {
            return _caster.SpellCaster.IsEnergySufficient(_spellSO); 
        }
    }
}
