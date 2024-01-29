 
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
    public class LectureParticipationActionCommand : BaseLocationActionCommand
    {
        [Header("Scene with lecture inside it")]
        [SerializeField]
        private AssetReference _lectureSceenDataSO  ;

        private SceneDataSO _sceneDataSO;

        public LectureParticipationActionCommand(LectureParticipationActionCommand copied) : base(copied)
        {
            this._lectureSceenDataSO = copied._lectureSceenDataSO; 

            _sceneDataSO = PersistentAddressableResourceLoader.Instance.LoadResourceOperation<SceneDataSO>(copied._lectureSceenDataSO);
             
        }
        public override void ExecuteCommand()
        {
            PersistentSceneLoader.Instance.LoadGeneralScene(_sceneDataSO);
        }





    }

}
