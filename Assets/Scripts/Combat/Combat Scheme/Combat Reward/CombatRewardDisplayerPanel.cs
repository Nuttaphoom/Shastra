using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vanaring
{
    public class CombatRewardDisplayerPanel : BaseRewardDisplayerPanel
    {
        [SerializeField] private TextMeshProUGUI expRunningText;
        [SerializeField] private CharacterEXPSocketGUI socketTemplate;
        [SerializeField] private GameObject hrzLayout;
        [SerializeField] private GameObject gfx;
        [SerializeField] private Button nextButton;
        private List<RuntimeCombatMemberData> memberList = new List<RuntimeCombatMemberData>();

        private int startRunningEXP = 0;

        public void SetUpReward(CombatRewardManager.CombatRewardData combat)
        {
            gfx.gameObject.SetActive(true);
            StartCoroutine(LoadCharacterEXPGainWindow(combat.RewardForEntities));
        }

        public override IEnumerator SettingUpNumber()
        {
            yield return null;
        }
        public override void ForceSetUpNumber()
        {
            _uiAnimationDone = true;
        }

        public override void OnContinueButtonClick()
        {
            if (IsSettingUpSucessfully)
                _displayingUIDone = true;

            else
                ForceSetUpNumber();
        }

        public IEnumerator LoadCharacterEXPGainWindow(List<CombatRewardManager.EntityRewardData> rewardList)
        {
            if (PersistentPlayerPersonalDataManager.Instance.CombatMemberDataLocator.GetRuntimeCombatMembers.Count != 0)
            {
                memberList = PersistentPlayerPersonalDataManager.Instance.CombatMemberDataLocator.GetRuntimeCombatMembers;
            }
            else
            {
                Debug.Log("Can't load member");
            }
            foreach (CombatRewardManager.EntityRewardData reward in rewardList)
            {
                CharacterEXPSocketGUI newSocket = Instantiate(socketTemplate, hrzLayout.transform);
                newSocket.Init(reward);
                newSocket.gameObject.SetActive(true);
                //reward.ControlEntity.CombatCharacterSheet;

            }
            socketTemplate.gameObject.SetActive(false);
            yield return new WaitForSeconds(1.0f);
            yield return RunNumberUp(0, rewardList[0].ReceivedExp, 1.0f);
            
            _uiAnimationDone = true;
            yield return null;
        }

        public IEnumerator RunNumberUp(float start, float end, float duration)
        {
            float timer = 0f;
            while (timer < duration)
            {
                float expVal = Mathf.Lerp(start, end, timer / duration);
                expRunningText.text = Mathf.Round(expVal).ToString();
                timer += Time.deltaTime;
                yield return null;
            }
            expRunningText.text = Mathf.Round(end).ToString();
        }

    }
}
