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
        [SerializeField]
        private SceneDataSO _sceneDataSO ; 
        public ActivityActionCommand(ActivityActionCommand copied)
        {
            _sceneDataSO = copied._sceneDataSO;
        }
        public override void ExecuteCommand()
        {
            PersistentSceneLoader.Instance.LoadLocation(_sceneDataSO, new ActivitySceneDataStruct()) ; 
        }

        public struct ActivitySceneDataStruct 
        {

        }
    }
    
}
