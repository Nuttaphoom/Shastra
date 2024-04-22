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
        [SerializeField] private MissionItemRewardSocketGUI itemSocketTemplate;
        [SerializeField] private GameObject itemVerticalLayout;
        private List<RuntimeCombatMemberData> memberList = new List<RuntimeCombatMemberData>();
        private List<MissionItemRewardSocketGUI> missionItemRewardSocketList = new List<MissionItemRewardSocketGUI>();

        private int startRunningEXP = 0;

        public void SetUpReward(CombatRewardManager.CombatRewardData combat)
        {
            gfx.gameObject.SetActive(true);
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
                newSocket.Init(reward);
                newSocket.gameObject.SetActive(true);
                //reward.ControlEntity.CombatCharacterSheet;

            }
            foreach (EventRewardData eventReward in rewardList.Rewards)
            {
                MissionItemRewardSocketGUI newSocket = Instantiate(itemSocketTemplate, itemVerticalLayout.transform);
                newSocket.InitSocket(eventReward);
                missionItemRewardSocketList.Add(newSocket);
            }

            itemSocketTemplate.gameObject.SetActive(false);
            socketTemplate.gameObject.SetActive(false);

            

            
            yield return new WaitForSeconds(1.0f);
            yield return RunNumberUp(0, rewardList.RewardForEntities[0].ReceivedExp, 1.0f);

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
                Debug.Log("PlayAnimation");
                missionItemRewardSocketList[0].PlayAnimationMoveIn();
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
                Debug.Log(i);
                if (i != missionItemRewardSocketList.Count - 1)
                {
                    Debug.Log("d" + i);
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
