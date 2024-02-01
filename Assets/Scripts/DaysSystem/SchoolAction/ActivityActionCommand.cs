
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Vanaring
{
    [Serializable]
    public class ActivityActionCommand : BaseLocationActionCommand
    {
        [Header("Scene with lecture inside it")]
        [SerializeField]
        private AssetReference _activitySceenDataSO;

        [SerializeField]
        public List<PersonalityRewardData> _personalityRewards;

        private SceneDataSO _sceneDataSO;


        public ActivityActionCommand(ActivityActionCommand copied) : base(copied)
        {
            _personalityRewards = copied._personalityRewards;
            _activitySceenDataSO = copied._activitySceenDataSO; ;

            if (_personalityRewards == null || _personalityRewards.Count == 0)
                throw new Exception("_personalityRewards is null or size equal to 0 in ");


        }
        public override void ExecuteCommand()
        {
            _sceneDataSO = PersistentAddressableResourceLoader.Instance.LoadResourceOperation<SceneDataSO>(_activitySceenDataSO);

            if (_sceneDataSO == null)
                throw new Exception(_activitySceenDataSO + "resource can not be loaded");

            PersistentSceneLoader.Instance.CreateLoaderDataUser<List<PersonalityRewardData>>(_sceneDataSO.GetSceneID(), _personalityRewards);
            PersistentSceneLoader.Instance.LoadGeneralScene(_sceneDataSO);

        }





    }

}
