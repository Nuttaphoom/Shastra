using System;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring 
{
    public abstract class BaseRewardDisplayer<RewardType, RewardPanelType> where RewardPanelType : BaseRewardDisplayerPanel 
    {  
        [SerializeField]
        protected RewardPanelType _rewardDisplayGOTemplate ;

        [SerializeField]
        protected RewardType _rewardType;

        protected GameObject _rewardPanel;

        public virtual IEnumerator DisplayRewardUICoroutine(List<RewardType> rewardType)
        {
            yield return null; 
        }
        public virtual IEnumerator DisplayRewardUICoroutine(RewardType rewardType)
        {
            yield return null;
        }
        protected abstract IEnumerator SettingUpRewardDisplayPanel(RewardPanelType rewardDisplayGOTemplate); 
        protected IEnumerator CreateRewardDisplayPanel()
        {
            _rewardPanel = MonoBehaviour.Instantiate(_rewardDisplayGOTemplate.gameObject);
            yield return SettingUpRewardDisplayPanel(_rewardPanel.GetComponent<RewardPanelType>());

            yield return WaitUntilDisplayFinish(); 


        }

        private IEnumerator WaitUntilDisplayFinish()
        {
            while (_rewardPanel.GetComponent<RewardPanelType>().IsFinishingDisplayUI == false)
            {
                yield return new WaitForEndOfFrame();
            }
        }
    } 

    

    
    
}
