using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vanaring
{
    public class PartyCharacterSocketGUI : MonoBehaviour
    {
        [SerializeField] private Image characterPortrait;
        [SerializeField] private Image hpBar;
        [SerializeField] private Image mpBar;
        [SerializeField] private TextMeshProUGUI hpNUM;
        [SerializeField] private TextMeshProUGUI mpNUM;
        private RuntimePartyMember member;

        public void Init(RuntimePartyMember member)
        {
            this.member = member;
            characterPortrait.sprite = member.GetRuntimeCombatMemberData.GetCharacterSheet.GetCharacterIcon;
            hpBar.fillAmount = 1.0f;
            mpBar.fillAmount = 1.0f;
            hpNUM.text = member.GetRuntimeCombatMemberData.LevelAttributeHandler.GetSecondaryAttribute_MaxHP.ToString();
            mpNUM.text = member.GetRuntimeCombatMemberData.LevelAttributeHandler.GetSecondaryAttribute_MaxMP.ToString();
        }

        private void Update()
        {
            //Chang to better solution soon
            if (member != null)
            {
                Init(member);
            }
        }
    }
}
