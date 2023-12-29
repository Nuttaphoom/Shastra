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

        [SerializeField,AllowNesting,NaughtyAttributes.ShowIf("_commandType", ELoadLocationCommandType.LoadScene)] 
        private LoadClassroomCommand _loadLocationCommand;
        
        //[SerializeField,AllowNesting, NaughtyAttributes.ShowIf("_commandType", ELoadLocationCommandType.LoadCutscene)]
        //private LoadCutsceneCommand _loadCutsceneCommand;
    
        public BaseLocationSelectionCommand FactorizeLocationSelectionCommand()
        {
            if (_commandType == ELoadLocationCommandType.LoadScene)
            {
                return new LoadClassroomCommand(_loadLocationCommand); 
            } 

            throw new Exception("_commandType is never set "); 
        }
    }

    public enum ELoadLocationCommandType
    {
        None,
        LoadScene ,
       
    }

    public abstract class BaseLocationSelectionCommand
    {
        [SerializeField]
        private Sprite _actionIconAsset; 

        public Sprite GetActionIconSprite => _actionIconAsset;  

        public abstract void ExecuteCommand () ;
    }


    [Serializable]
    public abstract class LoadSceneCommand : BaseLocationSelectionCommand
    {
        [SerializeField]
        protected SceneDataSO _sceneField;


    }
    [Serializable]
    public class LoadLocationMenuCommand : LoadSceneCommand
    {
        [SerializeField]
        private LoadLocationMenuCommandData data; 

        public LoadLocationMenuCommand(LoadLocationMenuCommand copied)
        {
            data = copied.data; 
        }
        public override void ExecuteCommand()
        {

        }

        public struct LoadLocationMenuCommandData
        {
            List<LoadSceneCommand> commmand; 
        }

       
    }


    [Serializable]
    public class LoadClassroomCommand : LoadSceneCommand
    {
        public LoadClassroomCommand(LoadClassroomCommand copied)
        {

        }
        public override void ExecuteCommand()
        {
            PersistentSceneLoader.Instance.LoadLocation<ClassroomLoadData>(_sceneField,new ClassroomLoadData() ) ; 
        }

        public struct ClassroomLoadData
        {

        }
    }
}
