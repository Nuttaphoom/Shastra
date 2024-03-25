using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace Vanaring
{
    public class PauseMenuCharacterDetail : PauseMenuWindowGUI
    {
        [SerializeField] private Image characterPortrait;
        [SerializeField] private Image characterPortraitBG;
        [SerializeField] private Image hpFillBar;
        [SerializeField] private Image mpFillBar;
        [SerializeField] private TextMeshProUGUI atkStat;
        [SerializeField] private TextMeshProUGUI vitStat;
        [SerializeField] private TextMeshProUGUI intStat;
        [SerializeField] private TextMeshProUGUI lckStat;
        [SerializeField] private TextMeshProUGUI agiStat;
        [SerializeField] private TextMeshProUGUI characterName;
        [SerializeField] private TextMeshProUGUI characterDescription;
        [SerializeField] private GameObject gfx;
        [SerializeField] private GameObject spellPanel;
        private List<SpellActionSO> spellRegiteredList = new List<SpellActionSO>();
        private List<CombatCharacterSheetSO> combatCharacterSheet = new List<CombatCharacterSheetSO>();
        private int characterIndex = 0;
        public override void ClearData()
        {
            
        }

        public override void LoadWindowData(CombatEntity entity)
        {
            CharacterSheetDatabaseSO charactersheetDataSO = PersistentAddressableResourceLoader.Instance.LoadResourceOperation<CharacterSheetDatabaseSO>(DatabaseAddressLocator.GetCharacterSheetDatabaseAddress);
            combatCharacterSheet = charactersheetDataSO.GetCombatCharacterShhets();

            characterIndex = 0;
            hpFillBar.fillAmount = 1.0f;
            mpFillBar.fillAmount = 1.0f;
            vitStat.text = combatCharacterSheet[characterIndex].GetVitality.ToString();
            atkStat.text = combatCharacterSheet[characterIndex].GetStrength.ToString();
            intStat.text = combatCharacterSheet[characterIndex].GetIntellect.ToString();
            lckStat.text = combatCharacterSheet[characterIndex].GetLuck.ToString();
            agiStat.text = combatCharacterSheet[characterIndex].GetAgility.ToString();
            characterPortrait.sprite = combatCharacterSheet[characterIndex].GetCharacterIcon;
            characterPortraitBG.sprite = combatCharacterSheet[characterIndex].GetCharacterIcon;

            //characterName.text = runtimePartyPear.GetMemberName;
            //runtimePartyAsha.GetCharacterSheet.
            //hpFillBar.fillAmount = entity.StatsAccumulator.GetHPAmount() / entity.StatsAccumulator.GetPeakHPAmount();
            //mpFillBar.fillAmount = entity.SpellCaster.GetMP / entity.SpellCaster.GetPeakMP;
            //characterPortrait.sprite = entity.CombatCharacterSheet.GetCharacterIcon;
        }

        public override void OnWindowActive()
        {
            characterName.text = PersistentPlayerPersonalDataManager.Instance.PartyMemberDataLocator.GetRuntimeData("Asha").GetMemberName;
            spellRegiteredList = PersistentPlayerPersonalDataManager.Instance.PartyMemberDataLocator.GetRuntimeData("Asha").GetRegisteredSpellActionSO;
        }

        public override void OnWindowDeActive()
        {
  
        }

        public override void ReceiveKeysFromWindowManager(KeyCode key)
        {
            if (key == KeyCode.Escape)
            {
                _pauseMenuWindowGUI.OpenWindow(EPauseWindowGUI.Main);
            }
            else if (key == KeyCode.Q)
            {
                //if 
                //{

                //}
            }
            else if (key == KeyCode.W)
            {
                if (characterIndex > 0)
                {
                    characterIndex--;
                }
                SwitchCharacter();
            }
            else if (key == KeyCode.S)
            {
                if (characterIndex < combatCharacterSheet.Count - 1)
                {
                    characterIndex++;
                }
                SwitchCharacter();
            }
        }

        private void SwitchCharacter()
        {
            hpFillBar.fillAmount = 1.0f;
            mpFillBar.fillAmount = 1.0f;
            vitStat.text = combatCharacterSheet[characterIndex].GetVitality.ToString();
            atkStat.text = combatCharacterSheet[characterIndex].GetStrength.ToString();
            intStat.text = combatCharacterSheet[characterIndex].GetIntellect.ToString();
            lckStat.text = combatCharacterSheet[characterIndex].GetLuck.ToString();
            agiStat.text = combatCharacterSheet[characterIndex].GetAgility.ToString();
            characterPortrait.sprite = combatCharacterSheet[characterIndex].GetCharacterIcon;
            characterPortraitBG.sprite = combatCharacterSheet[characterIndex].GetCharacterIcon;
        }
    }
}
