 
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

        [SerializeField]
        private LectureSO _lecture_to_participate ; 

        private SceneDataSO _sceneDataSO;

        private string address = "TestLectureSceneDataSO" ;
        public LectureParticipationActionCommand(LectureParticipationActionCommand copied) : base(copied)
        {
            this._lecture_to_participate = copied._lecture_to_participate; 
            this._lectureSceenDataSO = copied._lectureSceenDataSO;  
        }
        public override void ExecuteCommand()
        {
            _sceneDataSO = PersistentAddressableResourceLoader.Instance.LoadResourceOperation<SceneDataSO>(_lectureSceenDataSO); 

            if (_sceneDataSO == null)
                throw new Exception(_lectureSceenDataSO + "resource can not be loaded");

            if (_lecture_to_participate == null)
                throw new Exception("_lecture_to_participated is null"); 

            PersistentSceneLoader.Instance.CreateLoaderDataUser<LectureSO>(_sceneDataSO.GetSceneID(), _lecture_to_participate);

            PersistentSceneLoader.Instance.LoadGeneralScene(_sceneDataSO);
        }

        





    }

}
