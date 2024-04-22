using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;

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
        private PlayableDirector introDirector;
        private float expGained;

        public void Init(CombatRewardManager.EntityRewardData rewardStruct, PlayableDirector director)
        {
            expGained = rewardStruct.ReceivedExp;
            introDirector = director;

            foreach (RuntimeCombatMemberData cmember in PersistentPlayerPersonalDataManager.Instance.CombatMemberDataLocator.GetRuntimeCombatMembers)
            {
                if(cmember.GetCharacterSheet.CharacterName == rewardStruct.ControlEntity.CombatCharacterSheet.CharacterName)
                {
                    member = cmember;
                    expBar.fillAmount = (float)cmember.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentEXP /
                            cmember.LevelAttributeHandler.GetCharacterUEXPSystem.GetEXPCap();
                    
                }
            }
            levelText.text = "Lv." + member.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentLevel.ToString();
            portrait.sprite = member.GetCharacterSheet.GetCharacterIcon;
            //Debug.Log("Cur: " + member.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentEXP + " Cap: " + member.LevelAttributeHandler.GetCharacterUEXPSystem.GetEXPCap() + " Level:" +member.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentLevel);
            

        }

        public void StartPlayeEXPBarAnimation()
        {
            StartCoroutine(PlayEXPNumberAnimation(member.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentEXP,
                        member.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentEXP + expGained,
                        expGained, (int)member.LevelAttributeHandler.GetCharacterUEXPSystem.GetEXPCap()));
        }

        private IEnumerator PlayEXPNumberAnimation(float start, float end, float gain, float max)
        {
            Debug.Log("curLv:" + member.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentLevel 
                + " " + member.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentEXP + "/" + member.LevelAttributeHandler.GetCharacterUEXPSystem.GetEXPCap());
            float overVal = max - (member.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentEXP + gain);
            seccondBar.fillAmount = (float)end / max;
            if(overVal <= 0)
            {
                end = max;
            }
            float timer = 0f;
            while (timer < 1)
            {
                float expVal = Mathf.Lerp(start, end, timer / 1);
                if(expBar.fillAmount >= 1)
                {
                    break;
                }
                expBar.fillAmount = Mathf.Round(expVal) / max;
                if ((Mathf.RoundToInt(max) - Mathf.RoundToInt(expVal)) <= 0)
                {
                    expRemainingNumText.text = "0";
                    seccondBar.fillAmount = 0f;
                }
                else 
                { 
                    expRemainingNumText.text = (Mathf.RoundToInt(max) - Mathf.RoundToInt(expVal)).ToString(); 
                }
                timer += Time.deltaTime;
                yield return null;
            }
            Debug.Log(overVal);
            if (overVal <= 0)
            {
                levelText.text = "Lv." + (member.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentLevel + 1).ToString();
                Debug.Log("New Level: " + member.LevelAttributeHandler.GetCharacterUEXPSystem.GetEXPCap(member.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentLevel + 1) + " lv:" + member.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentLevel);
                expBar.fillAmount = 0f;
                seccondBar.fillAmount = Mathf.Abs(overVal) / member.LevelAttributeHandler.GetCharacterUEXPSystem.GetEXPCap(member.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentLevel + 1);
                expRemainingNumText.text = Mathf.RoundToInt(member.LevelAttributeHandler.GetCharacterUEXPSystem.GetEXPCap(member.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentLevel + 1)).ToString();
                yield return PlayEXPNumberAnimation(0f, Mathf.Abs(overVal), Mathf.Abs(overVal), (float)member.LevelAttributeHandler.GetCharacterUEXPSystem.GetEXPCap(member.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentLevel + 1));
            }
            //expBar.fillAmount = (float)(member.LevelAttributeHandler.GetCharacterUEXPSystem.GetCurrentLevel + gain) / max;
        }
    }
}
