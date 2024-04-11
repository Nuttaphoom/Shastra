using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vanaring
{
    public class CharacterEXPSocketGUI : MonoBehaviour
    {
        [SerializeField] private Image portrait;
        [SerializeField] private Image expBar;
        [SerializeField] private TextMeshProUGUI expRemainingNumText;
        [SerializeField] private TextMeshProUGUI levelText;

        private RuntimeCombatMemberData member;
        void Start()
        {
        
        }

        public void Init(RuntimeCombatMemberData member)
        {
            this.member = member;
            //Debug.Log("Cur: " + member.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentEXP + " Cap: " + member.LevelAttributeHandler.GetCharacterUEXPSystem.GetEXPCap() + " Level:" +member.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentLevel);
            if(member.LevelAttributeHandler.GetCharacterUEXPSystem.GetEXPCap() <= 0)
            {
                expBar.fillAmount = 1.0f;
            }
            else
            {
                expBar.fillAmount = (float)member.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentEXP / member.LevelAttributeHandler.GetCharacterUEXPSystem.GetEXPCap();
            }
            expRemainingNumText.text = ((float)member.LevelAttributeHandler.GetCharacterUEXPSystem.GetEXPCap() - member.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentEXP).ToString();
            levelText.text = member.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentLevel.ToString();
            portrait.sprite = member.GetCharacterSheet.GetCharacterIcon;
        }
        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
