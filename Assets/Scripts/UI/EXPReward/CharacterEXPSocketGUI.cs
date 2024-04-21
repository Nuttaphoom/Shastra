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
        [SerializeField] private Image seccondBar;
        [SerializeField] private TextMeshProUGUI expRemainingNumText;
        [SerializeField] private TextMeshProUGUI levelText;

        private RuntimeCombatMemberData member;

        public void Init(CombatRewardManager.EntityRewardData rewardStruct)
        {
            float expGained = rewardStruct.ReceivedExp;

            foreach (RuntimeCombatMemberData cmember in PersistentPlayerPersonalDataManager.Instance.CombatMemberDataLocator.GetRuntimeCombatMembers)
            {
                if(cmember.GetCharacterSheet.CharacterName == rewardStruct.ControlEntity.CombatCharacterSheet.CharacterName)
                {
                    member = cmember;
                    expBar.fillAmount = (float)cmember.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentEXP /
                            cmember.LevelAttributeHandler.GetCharacterUEXPSystem.GetEXPCap();
                    StartCoroutine(PlayEXPNumberAnimation(cmember.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentEXP,
                        cmember.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentEXP + rewardStruct.ReceivedExp,
                        rewardStruct.ReceivedExp, (int)cmember.LevelAttributeHandler.GetCharacterUEXPSystem.GetEXPCap()));
                }
            }
            levelText.text = member.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentLevel.ToString();
            portrait.sprite = member.GetCharacterSheet.GetCharacterIcon;
            //Debug.Log("Cur: " + member.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentEXP + " Cap: " + member.LevelAttributeHandler.GetCharacterUEXPSystem.GetEXPCap() + " Level:" +member.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentLevel);
            

        }

        private IEnumerator PlayEXPNumberAnimation(float start, float end, float gain, float max)
        {
            yield return new WaitForSeconds(1.0f);
            seccondBar.fillAmount = (float)(member.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentLevel + gain) / max;
            float timer = 0f;
            while (timer < 1)
            {
                float expVal = Mathf.Lerp(start, end, timer / 1);
                expBar.fillAmount = Mathf.Round(expVal) / max;
                expRemainingNumText.text = (Mathf.RoundToInt(max) - Mathf.RoundToInt(expVal)).ToString();
                timer += Time.deltaTime;
                yield return null;
            }
            expBar.fillAmount = (float)(member.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentLevel + gain) / max;
        }
    }
}
