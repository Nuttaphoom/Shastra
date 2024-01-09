using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine; 
namespace Vanaring
{ 
    [Serializable]
    public  class ActivityActionCommand : BaseLocationActionCommand
    {
        [Header("Scene with activity inside it")]
        ///scene with ActivitySchoolActionScheme is considered Activity Scene 
        [SerializeField]
        private SceneDataSO _sceneDataSO ;

        public ActivityActionCommand(ActivityActionCommand copied) : base(copied)
        {
            _sceneDataSO = copied._sceneDataSO;
        }
        public override void ExecuteCommand()
        {
            PersistentSceneLoader.Instance.LoadGeneralScene(_sceneDataSO ) ; 
        }
       

        

      
    }
    
}
