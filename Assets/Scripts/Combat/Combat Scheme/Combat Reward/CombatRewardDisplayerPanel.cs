using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;

namespace Vanaring
{
    public class CombatRewardDisplayerPanel : BaseRewardDisplayerPanel
    {
        [SerializeField] private TextMeshProUGUI expRunningText;
        [SerializeField] private TextMeshProUGUI cashRunningText;
        [SerializeField] private TextMeshProUGUI isItemCollectText;
        [SerializeField] private CharacterEXPSocketGUI socketTemplate;
        [SerializeField] private GameObject hrzLayout;
        [SerializeField] private GameObject gfx;
        [SerializeField] private Button nextButton;
        [SerializeField] private MissionItemRewardSocketGUI itemSocketTemplate;
        [SerializeField] private GameObject itemVerticalLayout;
        private List<RuntimeCombatMemberData> memberList = new List<RuntimeCombatMemberData>();
        private List<CharacterEXPSocketGUI> missionCharacterEXPSocketList = new List<CharacterEXPSocketGUI>();
        private List<MissionItemRewardSocketGUI> missionItemRewardSocketList = new List<MissionItemRewardSocketGUI>();
        [SerializeField] private PlayableDirector introDirector;

        private int startRunningEXP = 0;

        public void SetUpReward(CombatRewardManager.CombatRewardData combat)
        {
            gfx.gameObject.SetActive(true);
            isItemCollectText.gameObject.SetActive(false);
            StartCoroutine(LoadCharacterEXPGainWindow(combat));
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

        public IEnumerator LoadCharacterEXPGainWindow(CombatRewardManager.CombatRewardData rewardList)
        {
            if (PersistentPlayerPersonalDataManager.Instance.CombatMemberDataLocator.GetRuntimeCombatMembers.Count != 0)
            {
                memberList = PersistentPlayerPersonalDataManager.Instance.CombatMemberDataLocator.GetRuntimeCombatMembers;
            }
            else
            {
                Debug.Log("Can't load member");
            }
            foreach (CombatRewardManager.EntityRewardData reward in rewardList.RewardForEntities)
            {
                CharacterEXPSocketGUI newSocket = Instantiate(socketTemplate, hrzLayout.transform);
                newSocket.Init(reward, introDirector);
                newSocket.gameObject.SetActive(true);
                missionCharacterEXPSocketList.Add(newSocket);

                nextButton.Select();
                //reward.ControlEntity.CombatCharacterSheet;

            }
            if (rewardList.Rewards.Count > 0)
            {
                isItemCollectText.gameObject.SetActive(true);
            }
            int totalCashReward = 0;
            foreach (EventRewardData eventReward in rewardList.Rewards)
            {
                if (eventReward.RewardIsCash)
                {
                    totalCashReward = eventReward.CashReward.RewardAmount;
                }
                else
                {
                    MissionItemRewardSocketGUI newSocket = Instantiate(itemSocketTemplate, itemVerticalLayout.transform);
                    newSocket.InitSocket(eventReward);
                    missionItemRewardSocketList.Add(newSocket);
                }
            }

            

            itemSocketTemplate.gameObject.SetActive(false);
            socketTemplate.gameObject.SetActive(false);

            while (introDirector.state == PlayState.Playing)
            {
                yield return new WaitForEndOfFrame();
            }

            foreach (CharacterEXPSocketGUI socket in missionCharacterEXPSocketList)
            {
                socket.StartPlayeEXPBarAnimation();
            }

            yield return RunNumberUp(0, rewardList.RewardForEntities[0].ReceivedExp, 1.0f, expRunningText);
            yield return RunNumberUp(0, 100, 1.0f, cashRunningText);

            if (missionItemRewardSocketList.Count != 0)
            {
                yield return ShowItemList();
            }

            _uiAnimationDone = true;
            yield return null;
        }

        private IEnumerator ShowItemList()
        {
            if (missionItemRewardSocketList.Count == 1)
            {
                yield return missionItemRewardSocketList[0].PlayAnimationMoveIn();
            }
            else
            {
                yield return ShowItemAnimation();
            }
        }

        private IEnumerator ShowItemAnimation()
        {
            for (int i = missionItemRewardSocketList.Count-1; i >= 0; i--)
            {
                if (i != missionItemRewardSocketList.Count - 1)
                {
                    yield return MoveVerticalPanel(itemVerticalLayout.GetComponent<RectTransform>().localPosition.y,
                                    itemVerticalLayout.GetComponent<RectTransform>().localPosition.y - 160);
                }
                yield return missionItemRewardSocketList[i].PlayAnimationMoveIn();
            }
        }

        private IEnumerator MoveVerticalPanel(float start, float end)
        {
            Debug.Log("Move Vertic");
            float elapsedTime = 0.0f;
            while (elapsedTime < 0.15f)
            {
                float t = elapsedTime / 0.15f;
                float posY = Mathf.Lerp(start, end, t);

                Vector3 newPosition = itemVerticalLayout.GetComponent<RectTransform>().localPosition;
                newPosition.y = posY;
                itemVerticalLayout.GetComponent<RectTransform>().localPosition = newPosition;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            itemVerticalLayout.GetComponent<RectTransform>().localPosition = new Vector3(itemVerticalLayout.GetComponent<RectTransform>().localPosition.x,
                end, itemVerticalLayout.GetComponent<RectTransform>().localPosition.z);
        }

        public IEnumerator RunNumberUp(float start, float end, float duration, TextMeshProUGUI displayText)
        {
            float timer = 0f;
            while (timer < duration)
            {
                float expVal = Mathf.Lerp(start, end, timer / duration);
                displayText.text = "+" + Mathf.Round(expVal).ToString();
                timer += Time.deltaTime;
                yield return null;
            }
            displayText.text = "+" + Mathf.Round(end).ToString();
        }

    }
}
