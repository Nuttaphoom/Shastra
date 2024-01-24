using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using NaughtyAttributes; 

namespace Vanaring
{
    [Serializable]
    public class LocationActionCommandRegister
    {
        [SerializeField]
        private ELoadLocationCommandType _commandType;

        [SerializeField,AllowNesting,NaughtyAttributes.ShowIf("_commandType", ELoadLocationCommandType.Activity)] 
        private ActivityActionCommand _loadLocationCommand;
        
        //[SerializeField,AllowNesting, NaughtyAttributes.ShowIf("_commandType", ELoadLocationCommandType.LoadCutscene)]
        //private LoadCutsceneCommand _loadCutsceneCommand;
    
        public BaseLocationActionCommand FactorizeLocationSelectionCommand()
        {
            if (_commandType == ELoadLocationCommandType.Activity)
            {
                return new ActivityActionCommand(_loadLocationCommand); 
            } 

            throw new Exception("_commandType is never set "); 
        }
    }

    public enum ELoadLocationCommandType
    {
        None,
        Activity,
       
    }

    public abstract class BaseLocationActionCommand
    {
        [SerializeField]
        private string _actionName;
        [SerializeField]
        private string _actionDescription;
        [SerializeField]
        private Sprite _actionIconAsset;
        

        public Sprite GetActionIconSprite => _actionIconAsset;
        public string GetActionName => _actionName;
        public string GetActionDescription => _actionDescription;

        public BaseLocationActionCommand(BaseLocationActionCommand copied)
        {
            _actionIconAsset = copied._actionIconAsset;
            _actionName = copied._actionName;
            _actionDescription = copied._actionDescription;
        }
        public abstract void ExecuteCommand () ;

    }


    //[Serializable]
    //public abstract class LoadSceneCommand : BaseLocationActionCommand
    //{
    //    [SerializeField]
    //    protected SceneDataSO _sceneField;
    //}
    


  
}
