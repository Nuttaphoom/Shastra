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
        private List<SpellActionSO> spellRegiteredList = new List<SpellActionSO>();
        public override void ClearData()
        {
            
        }

        public override void LoadWindowData(CombatEntity entity)
        {
            characterName.text = PersistentPlayerPersonalDataManager.Instance.PartyMemberDataLocator.GetRuntimeData(entity.CombatCharacterSheet.CharacterName).GetMemberName;
            hpFillBar.fillAmount = entity.StatsAccumulator.GetHPAmount() / entity.StatsAccumulator.GetPeakHPAmount();
            mpFillBar.fillAmount = entity.SpellCaster.GetMP / entity.SpellCaster.GetPeakMP;
            characterPortrait.sprite = entity.CombatCharacterSheet.GetCharacterIcon;
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
                _pauseMenuWindowGUI.OpenWindow(EPauseWindowGUI.Party);
            }
            //else if (key == KeyCode.Space)
            //{

            //}
        }
    }
}
