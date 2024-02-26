
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Vanaring.Assets.Scripts.Utilities.Cutscene_System;

namespace Vanaring
{
    [Serializable]
    public class ActivityActionCommand : BaseLocationActionCommand
    {
        [SerializeField]
        private CutsceneContainerSO _activityCutscene; 

        //[Header("Scene with lecture inside it")]
        //[SerializeField]
        //private AssetReference _activitySceenDataSO;

        [SerializeField]
        public List<PersonalityRewardData> _personalityRewards;

        public ActivityActionCommand(ActivityActionCommand copied) : base(copied)
        {
            _personalityRewards = copied._personalityRewards;
            _activityCutscene = copied._activityCutscene; 

            if (_personalityRewards == null || _personalityRewards.Count == 0)
                throw new Exception("_personalityRewards is null or size equal to 0 in ");


        }
        public override void ExecuteCommand()
        {
            SceneDataSO _sceneDataSO = PersistentAddressableResourceLoader.Instance.LoadResourceOperation<SceneDataSO>(_activityCutscene.GetCutsceneSceneRef) ;

            PersistentSceneLoader.Instance.CreateLoaderDataUser<List<PersonalityRewardData>>(_sceneDataSO.GetSceneID(), _personalityRewards);
            PersistentSceneLoader.Instance.LoadGeneralScene(_sceneDataSO);

        }





    }

}
