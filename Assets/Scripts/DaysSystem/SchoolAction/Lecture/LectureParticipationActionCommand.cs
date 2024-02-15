 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Vanaring.Assets.Scripts.Utilities.Cutscene_System;

namespace Vanaring
{
    [Serializable]
    public class LectureParticipationActionCommand : BaseLocationActionCommand
    {
        [Serializable]
        public class ParticpationLectureData
        {
            [SerializeField] 
            private LectureSO _availableLecture ;

            [SerializeField] 
            private CutsceneContainerSO _cutsceneContainerSO ;


            public LectureSO GetAvailableLecture => _availableLecture ;

            public AssetReference GetSceneDataRef => _cutsceneContainerSO.GetCutsceneSceneRef ; 
        }

        [SerializeField] 
        private List<ParticpationLectureData> _availableLecture;
        private GameObject popUpPanel;


        public LectureParticipationActionCommand(LectureParticipationActionCommand copied) : base(copied)
        {
            this._availableLecture = copied._availableLecture ; 
         
            if (this._availableLecture == null)
                throw new Exception("_availableLecture is null");

        }
        public override void ExecuteCommand()
        {
            if(popUpPanel != null){
                popUpPanel.GetComponent<LectureSelectPanel>().OpenPanel();
                return;
            }
            popUpPanel = MonoBehaviour.Instantiate(PersistentAddressableResourceLoader.Instance.LoadResourceOperation<GameObject>("LectureSelectPanel"));
            popUpPanel.GetComponent<LectureSelectPanel>().InitPanel(_availableLecture, this);
        }

        /// <summary>
        /// Call when user selects a Lecture to participate 
        /// </summary>
        /// <param name="lecture"></param>

        public void OnSelectLecture(ParticpationLectureData lecture)
        {
            CutsceneSceneDataSO sceneDataSO = PersistentAddressableResourceLoader.Instance.LoadResourceOperation<CutsceneSceneDataSO>(lecture.GetSceneDataRef);

            PersistentSceneLoader.Instance.CreateLoaderDataUser<LectureSO>(sceneDataSO.GetSceneID(), lecture.GetAvailableLecture) ;

            PersistentSceneLoader.Instance.LoadGeneralScene(sceneDataSO);
        }
        





    }

}
