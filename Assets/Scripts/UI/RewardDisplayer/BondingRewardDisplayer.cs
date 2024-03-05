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
        [SerializeField] private Image botBar;
        private BondingRewardData rewardData;
        private int lv;

        public void SetReceivedReward(BondingRewardData rewardData)
        {
            this.rewardData = rewardData;
            //fillBar.fillAmount = rewardData.curExp / rewardData.capExp;
            lv = rewardData.bondLevel;
            fillBar.fillAmount = bondRatio[lv];
            characterName.text = rewardData.characterName;
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
            botBar.gameObject.SetActive(false);
            introDirector.Play();
            //yield return new WaitForSeconds(1.0f);
            while (introDirector.state == PlayState.Playing)
            {
                yield return new WaitForEndOfFrame();
            }
            introDirector.Stop();

            _uiAnimationDone = true;

            while (fillBar.fillAmount < bondRatio[lv + 1])
            {
                topBar.gameObject.SetActive(true);
                botBar.gameObject.SetActive(true);
                fillBar.fillAmount += 0.001f;
                yield return new WaitForSeconds(0.01f);
            }
            topBar.gameObject.SetActive(false);
            botBar.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }

        private IEnumerator DisplayLevelUp()
        {
            yield return null;
        }
    }
}
