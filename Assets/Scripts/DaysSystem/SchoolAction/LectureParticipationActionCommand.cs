 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Vanaring
{
    [Serializable]
    public class LectureParticipationActionCommand : BaseLocationActionCommand
    {
        [Serializable]
        public struct ParticpationLectureData
        {
            [SerializeField] 
            private LectureSO _availableLecture ;

            [Header("Scene with lecture inside it")]
            [SerializeField]
            private AssetReference _lectureSceenDataSO;

            public LectureSO GetAvailableLecture => _availableLecture ;

            public AssetReference GetSceneDataRef => _lectureSceenDataSO; 
        }

        [SerializeField] 
        private List<ParticpationLectureData> _availableLecture;

        public LectureParticipationActionCommand(LectureParticipationActionCommand copied) : base(copied)
        {
            this._availableLecture = copied._availableLecture ; 
         
            if (this._availableLecture == null)
                throw new Exception("_availableLecture is null");

        }
        public override void ExecuteCommand()
        {
            GameObject panel = MonoBehaviour.Instantiate(PersistentAddressableResourceLoader.Instance.LoadResourceOperation<GameObject>("LectureSelectPanel"));
            panel.GetComponent<LectureSelectPanel>().InitPanel(_availableLecture, this);
        }

        /// <summary>
        /// Call when user selects a Lecture to participate 
        /// </summary>
        /// <param name="lecture"></param>

        public void OnSelectLecture(ParticpationLectureData lecture)
        {
            Debug.Log("On Selct");
            SceneDataSO sceneDataSO = PersistentAddressableResourceLoader.Instance.LoadResourceOperation<SceneDataSO>(lecture.GetSceneDataRef);

            PersistentSceneLoader.Instance.CreateLoaderDataUser<LectureSO>(sceneDataSO.GetSceneID(), lecture.GetAvailableLecture) ;

            PersistentSceneLoader.Instance.LoadGeneralScene(sceneDataSO);
        }
        





    }

}
