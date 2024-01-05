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
        public ActivityActionCommand(ActivityActionCommand copied)
        {
            //_sceneField = copied._sceneField; 
        }
        public override void ExecuteCommand()
        {
            //PersistentSceneLoader.Instance.LoadLocation<LoadLocationCommandData>(_sceneField, new LoadLocationCommandData()) ; 
        }

        public struct LoadLocationCommandData
        {

        }
    }
    
}
