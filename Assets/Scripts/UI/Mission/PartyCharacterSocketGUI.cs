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
        [SerializeField] private TextMeshProUGUI characterName;
        [SerializeField] private Image hpBar;
        [SerializeField] private Image mpBar;
        [SerializeField] private TextMeshProUGUI hpNUM;
        [SerializeField] private TextMeshProUGUI mpNUM;
        private RuntimePartyMember member;

        public void Init(RuntimePartyMember member)
        {
            this.member = member;
            characterPortrait.sprite = member.GetRuntimeCombatMemberData.GetCharacterSheet.GetCharacterIcon;
            characterName.text = member.GetRuntimeCombatMemberData.GetCharacterSheet.CharacterName;
            hpBar.fillAmount = (float)member.GetCurrentPartyMemberHP / member.GetRuntimeCombatMemberData.LevelAttributeHandler.GetSecondaryAttribute_MaxHP;
            mpBar.fillAmount = (float)member.GetCurrentPartyMemberMP / member.GetRuntimeCombatMemberData.LevelAttributeHandler.GetSecondaryAttribute_MaxMP;
            int myInt = Mathf.CeilToInt(member.GetCurrentPartyMemberHP);
            hpNUM.text = myInt.ToString();
            mpNUM.text = Mathf.CeilToInt(member.GetCurrentPartyMemberMP).ToString();
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
