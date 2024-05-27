using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vanaring
{
    public class PauseMenuSpellPossesses : PauseMenuWindowGUI
    {
        [SerializeField] private Image spellImage;
        [SerializeField] private TextMeshProUGUI spellName;
        [SerializeField] private TextMeshProUGUI spellDescription;
        [SerializeField] private Image possessesCharacter;
        [SerializeField] private Image spellImgTemplate;
        [SerializeField] private GameObject gridTransform;
        [SerializeField] private GameObject leftArrow;
        [SerializeField] private GameObject rightArrow;
        //[SerializeField] private 
        private int spellIndex;
        private List<SpellActionSO> spellList = new List<SpellActionSO>();
        private List<Image> imgList = new List<Image>();

        public override void ClearData()
        {
            
        }

        public override void LoadWindowData(CombatEntity entity)
        {
            

        }

        public override void OnWindowActive()
        {
            spellIndex = 0;
            if (imgList.Count > 0)
            {
                foreach (Image image in imgList)
                {
                    if (image != null)
                    {
                        Destroy(image.gameObject);
                    }
                }
            }
            possessesCharacter.sprite = _pauseMenuWindowGUI.SelectCharacter.GetCharacterIcon;
            //Debug.Log(_pauseMenuWindowGUI.SelectCharacter.CharacterName);

            //if (_pauseMenuWindowGUI.SelectCharacter.GetCombatEntityPrefab.GetComponent<CombatEntity>() is ControlableEntity controlEntity)
            //{
            //    spellList = controlEntity.GetControlableEntityActionRegistry.GetSpellAction;
            //}

            spellImgTemplate.gameObject.SetActive(true);
            foreach (SpellActionSO spellAction in spellList)
            {
                Image newImg = Instantiate(spellImgTemplate, gridTransform.transform);
                newImg.sprite = spellAction.AbilityImage;
                imgList.Add(newImg);
            }
            spellImgTemplate.gameObject.SetActive(false);

            SwitchSpellIndex();
        }

        public override void OnWindowDeActive()
        {
            
        }

        public override void ReceiveKeysFromWindowManager(InputCode key)
        {
            if (key == InputCode.Up || key == InputCode.Escape)
            {
                _pauseMenuWindowGUI.OpenWindow(EPauseWindowGUI.Party);
            }
            if (key == InputCode.Left)
            {
                if(spellIndex > 0)
                {
                    spellIndex--;
                }
                SwitchSpellIndex();
            }
            if (key == InputCode.Right)
            {
                if(spellIndex < spellList.Count - 1)
                {
                    spellIndex++;
                }
                SwitchSpellIndex();
            }
        }

        private void SwitchSpellIndex()
        {
            rightArrow.gameObject.SetActive(false);
            leftArrow.gameObject.SetActive(false);
            if (spellIndex < spellList.Count - 1)
            {
                rightArrow.gameObject.SetActive(true);
            }
            if(spellIndex > 0)
            {
                leftArrow.gameObject.SetActive(true);
            }
            spellImage.sprite = spellList[spellIndex].AbilityImage;
            spellName.text = spellList[spellIndex].AbilityName;
            spellDescription.text = spellList[spellIndex].Desscription;
        }
    }
}
