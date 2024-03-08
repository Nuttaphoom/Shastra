using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Vanaring
{
    public class BondingRewardDisplayer : BaseRewardDisplayerPanel
    {
        [SerializeField] private PlayableDirector introDirector;
        [SerializeField] private TextMeshProUGUI characterName;
        [SerializeField] private Image fillBar;
        [SerializeField] private Image characterPortrait;
        private List<GameObject> bondRewardShowList = new List<GameObject>();
        [SerializeField] private List<float> bondRatio = new List<float>();
        [SerializeField] private Image topBar;
        [SerializeField] private Image arrow;
        private BondingRewardData rewardData;
        private int currentExp;

        public void SetReceivedReward(BondingRewardData rewardData)
        {
            this.rewardData = rewardData;
            currentExp = rewardData.curExp;
            fillBar.fillAmount = bondRatio[rewardData.curExp - 1];
            characterName.text = rewardData.characterName;
            Debug.Log("Cur exp: " + currentExp + " " + rewardData.capExp);
            characterPortrait.sprite = rewardData.characterSprite;
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

        public override IEnumerator SettingUpNumber()
        {
            topBar.gameObject.SetActive(false);
            introDirector.Play();
            //yield return new WaitForSeconds(1.0f);
            while (introDirector.state == PlayState.Playing)
            {
                yield return new WaitForEndOfFrame();
            }
            introDirector.Stop();

            _uiAnimationDone = true;

            while (fillBar.fillAmount < bondRatio[currentExp])
            {
                topBar.gameObject.SetActive(true);
                fillBar.fillAmount += 0.001f;
                yield return new WaitForSeconds(0.01f);
            }
            topBar.gameObject.SetActive(false);
            arrow.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.5f);
        }

        private IEnumerator DisplayLevelUp()
        {
            yield return null;
        }
    }
}
