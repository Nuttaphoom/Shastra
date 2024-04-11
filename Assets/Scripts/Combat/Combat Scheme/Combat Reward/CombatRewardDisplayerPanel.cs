using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring
{
    public class CombatRewardDisplayerPanel : BaseRewardDisplayerPanel
    {
        [SerializeField] private CharacterEXPSocketGUI socketTemplate;
        [SerializeField] private GameObject hrzLayout;
        [SerializeField] private GameObject gfx;
        [SerializeField] private Button nextButton;
        private List<RuntimeCombatMemberData> memberList = new List<RuntimeCombatMemberData>();

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
            foreach (RuntimeCombatMemberData member in memberList)
            {
                CharacterEXPSocketGUI newSocket = Instantiate(socketTemplate, hrzLayout.transform);
                newSocket.Init(member);
                newSocket.gameObject.SetActive(true);
            }
            socketTemplate.gameObject.SetActive(false);
            yield return null;
        }

    }
}
