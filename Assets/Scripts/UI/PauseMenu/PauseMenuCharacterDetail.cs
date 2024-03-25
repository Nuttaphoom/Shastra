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
        [SerializeField] private Image hpFillBar;
        [SerializeField] private Image mpFillBar;
        [SerializeField] private TextMeshProUGUI characterName;
        [SerializeField] private TextMeshProUGUI characterDescription;
        [SerializeField] private GameObject gfx;
        [SerializeField] private GameObject spellPanel;
        private List<SpellActionSO> spellRegiteredList = new List<SpellActionSO>();
        private List<CombatCharacterSheetSO> combatCharacterSheet = new List<CombatCharacterSheetSO>();
        public override void ClearData()
        {
            
        }

        public override void LoadWindowData(CombatEntity entity)
        {
            CharacterSheetDatabaseSO charactersheetDataSO = PersistentAddressableResourceLoader.Instance.LoadResourceOperation<CharacterSheetDatabaseSO>(DatabaseAddressLocator.GetCharacterSheetDatabaseAddress);
            combatCharacterSheet = charactersheetDataSO.GetCombatCharacterShhets();
            foreach (CombatCharacterSheetSO character in combatCharacterSheet)
            {
                hpFillBar.fillAmount = entity.StatsAccumulator.GetHPAmount() / entity.StatsAccumulator.GetPeakHPAmount();
            }

            //characterName.text = runtimePartyPear.GetMemberName;
            //runtimePartyAsha.GetCharacterSheet.
            //hpFillBar.fillAmount = entity.StatsAccumulator.GetHPAmount() / entity.StatsAccumulator.GetPeakHPAmount();
            //mpFillBar.fillAmount = entity.SpellCaster.GetMP / entity.SpellCaster.GetPeakMP;
            //characterPortrait.sprite = entity.CombatCharacterSheet.GetCharacterIcon;
        }

        public void LoadWindowData()
        {
            
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
                
            }
        }
    }
}
