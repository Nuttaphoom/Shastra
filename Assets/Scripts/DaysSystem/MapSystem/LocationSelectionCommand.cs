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
    public class LocationSelectionCommandRegister
    {
        [SerializeField]
        private ELoadLocationCommandType _commandType;

        [SerializeField,AllowNesting,NaughtyAttributes.ShowIf("_commandType", ELoadLocationCommandType.LoadLocation)] 
        private LoadClassroomCommand _loadLocationCommand;
        
        //[SerializeField,AllowNesting, NaughtyAttributes.ShowIf("_commandType", ELoadLocationCommandType.LoadCutscene)]
        //private LoadCutsceneCommand _loadCutsceneCommand;
    
        public BaseLocationSelectionCommand FactorizeLocationSelectionCommand()
        {
            if (_commandType == ELoadLocationCommandType.LoadLocation)
            {
                return new LoadClassroomCommand(_loadLocationCommand); 
            } 

            throw new Exception("_commandType is never set "); 
        }
    }

    public enum ELoadLocationCommandType
    {
        None,
        LoadLocation ,
       
    }

    public abstract class BaseLocationSelectionCommand
    {
        [SerializeField]
        private Sprite _actionIconAsset; 

        public Sprite GetActionIconSprite => _actionIconAsset;  

        public abstract void ExecuteCommand () ;
    }


    [Serializable]
    public abstract class LoadLocationCommand : BaseLocationSelectionCommand
    {
        [SerializeField]
        protected SceneDataSO _sceneField;


    }

    [Serializable]
    public class LoadClassroomCommand : LoadLocationCommand
    {
        public LoadClassroomCommand(LoadClassroomCommand copied)
        {

        }
        public override void ExecuteCommand()
        {
            PersistentSceneLoader.Instance.LoadLocation(_sceneField) ; 
        }
    }
}
